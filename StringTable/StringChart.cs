using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringChart {
	public static string Plot(Dictionary<double, double> d, Options o) {
		char[,] cells = new char[o.Rows, o.Columns];
		for (int c = 0; c < o.Columns; c++) {
			for (int r = 0; r < o.Rows; r++) {
				cells[r, c] = ' ';
			}
		}

		double min_x = d.Keys.ToArray().Min();
		double max_x = d.Keys.ToArray().Max();
		double min_y = d.Values.ToArray().Min();
		double max_y = d.Values.ToArray().Max();
		double dx = (max_x - min_x) / o.Columns;
		double dy = (max_y - min_y) / o.Rows;
		max_x += dx;
		max_y += dy;

		// calculate it
		double x1, y1;
		double x2, y2;
		SplineInterpolator sp = new SplineInterpolator(d);
		for (int i1 = 0, i2 = 1; i1 < o.Columns - 1; i1++, i2++) {
			x1 = min_x + i1 * dx + dx / 2;
			y1 = sp.GetValue(x1);
			x2 = min_x + (i2) * dx + dx / 2;
			y2 = sp.GetValue(x2);
			int j1 = (int)Math.Round((o.Rows) * (y1 - min_y) / (max_y - min_y), MidpointRounding.AwayFromZero);
			int j2 = (int)Math.Round((o.Rows) * (y2 - min_y) / (max_y - min_y), MidpointRounding.AwayFromZero);
			if (j2 > j1) {
				for (int j = j1; j < j2; j++) SetCell(cells, j, i1, o.Rows);
			} else {
				for (int j = j2; j < j1; j++) SetCell(cells, j, i1, o.Rows);
			}
			SetCell(cells, j2, i2, o.Rows);
		}

		// print it
		int left_pad = o.YLabelformat.Length + o.YMargin;
		for (int r = 0; r < o.Rows; r++) {
			int pl = (max_y - dy * r).ToString(o.YLabelformat).PadLeft(o.YLabelformat.Length + 2).Length;
			if (pl > left_pad) left_pad = pl;
		}
		StringBuilder s = new StringBuilder();
		if (!string.IsNullOrWhiteSpace(o.Title)) {
			s.Append("".PadLeft(left_pad + 2));
			if (o.Title.Length < o.Columns / 2) {
				s.Append("".PadLeft((int)(o.Columns + 2 - o.Title.Length) / 2) + o.Title);
			} else {
				s.Append(o.Title);
			}
			s.AppendLine();
		}
		s.Append("".PadLeft(left_pad + 2));
		for (int c = 0; c <= o.Columns; c++) {
			//s.Append($"{c % 10}");
			s.Append($"─");
		}
		s.Append("".PadLeft(3, '─'));
		s.AppendLine();
		for (int r = 0; r < o.Rows; r++) {
			//s.Append($"{r % 10}  ");
			s.Append($"{(max_y - dy * r).ToString(o.YLabelformat).PadLeft(left_pad)} | ");
			for (int c = 0; c < o.Columns; c++) {
				s.Append(cells[r, c]);
			}
			s.Append("   |");
			s.AppendLine();
		}
		// xlabels			
		int delta_ticks = o.Columns / (o.XTicks - 1);
		s.Append("".PadRight(left_pad + 2));
		s.Append("─");
		for (int c = 0; c <= o.Columns; c += delta_ticks) {
			string label = "|";
			bool last_label = o.Columns - delta_ticks >= c;
			if (label.Length < delta_ticks || last_label) {
				s.Append(label);
				int dd = delta_ticks - label.Length;
				if (c + dd < o.Columns) {
					s.Append("".PadRight(dd, '─'));
				} else {
					s.Append("".PadRight(o.Columns - c + 2, '─'));
				}
			}
		}
		s.AppendLine();
		s.Append("".PadRight(left_pad + 3));
		for (int c = 0; c <= o.Columns; c += delta_ticks) {
			double x = min_x + dx * c;
			string label = x.ToString(o.XLabelformat);
			bool last_label = o.Columns - delta_ticks >= c;
			if (label.Length < delta_ticks || last_label) {
				s.Append(label.PadRight(delta_ticks));
			}
		}
		return s.ToString();

		// Bug in the XLabels !!!!
	}

	private static void SetCell(char[,] cells, int y, int x, int rows) {
		cells[rows - 1 - y, x] = '─';
	}

	// Akima Spline Interpolation
	private class SplineInterpolator {
		private readonly double[] _keys;

		private readonly double[] _values;

		private readonly double[] _h;

		private readonly double[] _a;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="nodes">Collection of known points for further interpolation.
		/// Should contain at least two items.</param>
		public SplineInterpolator(IDictionary<double, double> nodes) {
			if (nodes == null) {
				throw new ArgumentNullException("nodes");
			}

			var n = nodes.Count;

			if (n < 2) {
				throw new ArgumentException("At least two point required for interpolation.");
			}

			_keys = nodes.Keys.ToArray();
			_values = nodes.Values.ToArray();
			_a = new double[n];
			_h = new double[n];

			for (int i = 1; i < n; i++) {
				_h[i] = _keys[i] - _keys[i - 1];
			}

			if (n > 2) {
				var sub = new double[n - 1];
				var diag = new double[n - 1];
				var sup = new double[n - 1];

				for (int i = 1; i <= n - 2; i++) {
					diag[i] = (_h[i] + _h[i + 1]) / 3;
					sup[i] = _h[i + 1] / 6;
					sub[i] = _h[i] / 6;
					_a[i] = (_values[i + 1] - _values[i]) / _h[i + 1] - (_values[i] - _values[i - 1]) / _h[i];
				}

				SolveTridiag(sub, diag, sup, ref _a, n - 2);
			}
		}

		/// <summary>
		/// Gets interpolated value for specified argument.
		/// </summary>
		/// <param name="key">Argument value for interpolation. Must be within 
		/// the interval bounded by lowest ang highest <see cref="_keys"/> values.</param>
		public double GetValue(double key) {
			int gap = 0;
			var previous = double.MinValue;

			// At the end of this iteration, "gap" will contain the index of the interval
			// between two known values, which contains the unknown z, and "previous" will
			// contain the biggest z value among the known samples, left of the unknown z
			for (int i = 0; i < _keys.Length; i++) {
				if (_keys[i] < key && _keys[i] > previous) {
					previous = _keys[i];
					gap = i + 1;
				}
			}

			var x1 = key - previous;
			var x2 = _h[gap] - x1;

			return ((-_a[gap - 1] / 6 * (x2 + _h[gap]) * x1 + _values[gap - 1]) * x2 +
				(-_a[gap] / 6 * (x1 + _h[gap]) * x2 + _values[gap]) * x1) / _h[gap];
		}


		/// <summary>
		/// Solve linear system with tridiagonal n*n matrix "a"
		/// using Gaussian elimination without pivoting.
		/// </summary>
		private static void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n) {
			int i;

			for (i = 2; i <= n; i++) {
				sub[i] = sub[i] / diag[i - 1];
				diag[i] = diag[i] - sub[i] * sup[i - 1];
				b[i] = b[i] - sub[i] * b[i - 1];
			}

			b[n] = b[n] / diag[n];

			for (i = n - 1; i >= 1; i--) {
				b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
			}
		}
	}
}

public class Options {
	public int Rows = 40;
	public int Columns = 80;
	public string YLabelformat = "0.0";
	public string XLabelformat = "0.0";
	public int XTicks = 3;
	public int YMargin = 2;
	public string Title = "Chart Title";
}



