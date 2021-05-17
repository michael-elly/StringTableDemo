using System;

namespace StringTableDemo {
	class Program {
		static void Main(string[] args) {
			StringTable t = new StringTable(new string[] { "A", "BBB", "C", "D" }) { SortSyntax = "A", AddIndexLineColumn = true, Indentation = 10 };
			t.AddRow(new object[] { "assdf", "sd", "Save your favorite articles to read offline.", "DD" });
			t.AddRow(new object[] { "csdfsdf", "We are the World", "The English Wikipedia is the English language edition of the free online encyclopedia.", "DD" });
			t.AddRow(new object[] { "dsdfsdf", 4, "4", "DD" });
			t.AddRow(new object[] { "esdfsdf", 5, "", "DD" });
			t.AddRow(new object[] { "bsdfsdf", "Lez Zepplin", "22111324", "DD" });

			int i = 0;
			Console.WriteLine($"Sample {++i}");
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 22);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.TrimStart, 22);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 22);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 22);
			t.UpdateColumn(1, StringTable.WrapText.Wrap, 5);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 22);
			t.UpdateColumn(1, StringTable.WrapText.Wrap, 8);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 200);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"Sample {++i}");
			t.AddIndexLineColumn = false;
			Console.WriteLine(t.CompileTable());
		}
	}
}
