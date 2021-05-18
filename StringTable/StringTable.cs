using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

public class StringTable {
	public string SortSyntax { get; set; }
	public int Indentation { get; set; } = 0;
	public bool AddIndexLineColumn { get; set; } = false;
	public bool DrawHeader { get; set; } = true;
	public bool UnderlineHeader { get; set; } = true;
	public int RedrawHeaderAfterRows { get; set; } = 60;
	public string LineBreakingText { get; set; } = "";

	public enum WrapText {
		Wrap,
		TrimStart,
		TrimEnd
	}

	private List<(string name, WrapText wrapText, int maxColWidth)> mColumns = new List<(string name, WrapText wrapText, int maxColWidth)>();

	public void UpdateColumn(int colIndex, WrapText wrapText, int maxColWidth) {
		if (colIndex < mColumns.Count && colIndex >= 0) {
			string name = mColumns[colIndex].name;
			mColumns[colIndex] = (name, wrapText, maxColWidth);			
		}
	}

	private readonly DataTable d;
	public StringTable(string[] columnNames, List<int> numericColumnIndexes = null) {
		d = new DataTable(); 
		for (int i = 0; i < columnNames.Length; i++) {
			mColumns.Add((columnNames[i], WrapText.TrimEnd, int.MaxValue));
			d.Columns.Add(columnNames[i], (numericColumnIndexes != null && numericColumnIndexes.Contains(i) ? typeof(double) : typeof(string)));
		}
	}

	public void AddRow(object[] row_elements) {
		d.Rows.Add(row_elements); 
	}

	public int RowCount { get { return d.Rows.Count; } }

	public string CompileTable() {
		int i; // row index

		// check we have some value
		if (mColumns.Count <= 0) return "Error: No table columns defined.";
		if (d.Rows.Count <= 0) return "Error: No table rows defined.";

		// construct the string
		string CrLf = Environment.NewLine;
		int table_columns_count = d.Columns.Count;
		StringBuilder s = new StringBuilder(); // whole report text
		string h = ""; // header_text
		int[] col_width = new int[table_columns_count];
		int[] col_start = new int[table_columns_count];
		string indent_str = "".PadRight(Indentation);
		int index_column_width = d.Rows.Count.ToString().Length + 1;

		if (RedrawHeaderAfterRows < 5) RedrawHeaderAfterRows = 5;

		try {
			// evaluate the optimal column width
			if (DrawHeader) {
				for (int j = 0; j <= table_columns_count - 1; j++) {
					col_width[j] = d.Columns[j].ColumnName.Length;
				}
			}
			for (i = 0; i <= d.Rows.Count - 1; i++) {
				for (int j = 0; j <= table_columns_count - 1; j++) {
					int cl = d.Rows[i][j].ToString().Length;
					if (cl > mColumns[j].maxColWidth) {
						col_width[j] = mColumns[j].maxColWidth;
						continue;
					} else if (cl > col_width[j]) {
						col_width[j] = cl;
					}
				}
			}
			for (int j = 0; j <= table_columns_count - 1; j++) {
				col_width[j] += 1;
			}

			for (int j = 0; j <= table_columns_count - 1; j++) {
				if (j == 0) {
					col_start[j] = Indentation + (AddIndexLineColumn ? index_column_width : 0);
				} else {
					col_start[j] = col_start[j - 1] + col_width[j - 1];
				}
			}

			// now build the header string
			if (DrawHeader) {
				s.Append(indent_str);
				if (AddIndexLineColumn) s.Append("#".PadRight(index_column_width));
				for (int j = 0; j <= table_columns_count - 1; j++) {
					if (d.Columns[j].ColumnName.Length <= col_width[j]) {
						s.Append(d.Columns[j].ColumnName.ToString().PadRight(col_width[j]));
					} else {
						s.Append(d.Columns[j].ColumnName.ToString().Substring(0, col_width[j]-1).PadRight(col_width[j]));
					}
				}
				s.Append(CrLf);
				if (UnderlineHeader) {
					s.Append(indent_str);
					if (AddIndexLineColumn) s.Append("".PadRight(index_column_width - 1, '-').PadRight(index_column_width));
					for (int j = 0; j <= table_columns_count - 1; j++) {
						s.Append("".PadRight(col_width[j] - 1, '-').PadRight(col_width[j]));
					}
					s.Append(CrLf);
				}
				h = s.ToString();
				s.Clear(); // we're going to rebuild the report including the header
			}

			// create the dataview and sort it
			DataView v = d.DefaultView;
			if (!string.IsNullOrWhiteSpace(SortSyntax)) v.Sort = SortSyntax;

			// calculate lines per row			
			Dictionary<(int rowIndex, int colIndex), int> lines_per_cell = new Dictionary<(int rowIndex, int colIndex), int>();
			Dictionary<int, int> lines_per_row = new Dictionary<int, int>();
			i = -1; // row index zero based			
			foreach (DataRowView r in v) {
				i++;
				lines_per_row[i] = 1;
				for (int j = 0; j <= table_columns_count - 1; j++) {
					lines_per_cell[(i, j)] = 1;
					if (r[j].ToString().Length > col_width[j] && mColumns[j].wrapText == WrapText.Wrap) {
						int total_cell_len = r[j].ToString().Length;
						lines_per_cell[(i, j)] = Math.Max(1, (int)Math.Ceiling((double)total_cell_len / (col_width[j] - 1)));
						lines_per_row[i] = Math.Max(lines_per_row[i], lines_per_cell[(i, j)]);
					}
				}
			}

			// build the table string
			i = -1; // row index zero based			
			foreach (DataRowView r in v) {
				i++;
				if (i % RedrawHeaderAfterRows == 0) {
					if (i == 0) {
						s.Append(h);
					} else {
						s.Append($"{LineBreakingText}{CrLf}{h}");
					}
				}
				for (int u = 0; u < lines_per_row[i]; u++) {
					s.Append(indent_str);
					if (u == 0) {
						if (AddIndexLineColumn) s.Append((i + 1).ToString().PadRight(index_column_width));
						for (int j = 0; j <= table_columns_count - 1; j++) {
							if (r[j].ToString().Length > col_width[j]) {
								if (mColumns[j].wrapText == WrapText.Wrap) {
									s.Append(Token(r[j].ToString(), u, col_width[j] - 1));
								} else if (mColumns[j].wrapText == WrapText.TrimEnd) {
									s.Append($"{r[j].ToString().Substring(0, col_width[j] - 3)}.. ");
								} else {
									s.Append(($"..{r[j].ToString().Substring(r[j].ToString().Length - col_width[j] + 3)}").PadRight(col_width[j]));
								}
							} else {
								s.Append(r[j].ToString().PadRight(col_width[j]));
							}
						}
					} else {
						if (AddIndexLineColumn) s.Append("".PadRight(index_column_width));
						for (int j = 0; j <= table_columns_count - 1; j++) {
							if (mColumns[j].wrapText == WrapText.Wrap && lines_per_cell[(i, j)] > u) {
								s.Append(Token(r[j].ToString(), u, col_width[j] - 1));
							} else {
								s.Append("".PadRight(col_width[j]));
							}
						}
					}
					s.Append(CrLf);
				}
			}

			// we've completed it 
			return s.ToString();

		} catch (Exception ex) {
			return $"Error builing table: {ex.Message} {ex.StackTrace}";
		}
	}

	private string Token(string source, int segmentIndex, int segmentLength) {
		if (source.Length <= segmentLength) {
			return source.PadRight(segmentLength + 1);
		} else if (source.Length > segmentLength * segmentIndex) {
			return source.Substring(segmentIndex * segmentLength, source.Length - segmentIndex * segmentLength >= segmentLength ? segmentLength : source.Length - segmentIndex * segmentLength).PadRight(segmentLength + 1);
		} else {
			return "".PadRight(segmentLength + 1);
		}
	}
}

