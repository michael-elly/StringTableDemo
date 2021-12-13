using System;
using System.Collections;
using System.Collections.Generic;

namespace StringTableDemo {
	class Program {
		static void Main(string[] args) {
			StringTable t = new StringTable(new string[] { "Full Name", "Nickname", "Folder", "Score" }) {
				DrawHeader = true,
				Indentation = 0
			};
			t.AddRow(new object[] { "JJ Xi", "Smith", @"C:\temp\a\quick\obj", 27 });
			t.AddRow(new object[] { "David Monson", @"Nice", @"C:\dev\kafkaclient\McpsWorker\obj\NetCore31", 7 });
			t.AddRow(new object[] { "David Johnson", @"Nicer", @"C:\dev\kafkaclient\McpsWorker\obj\Debug", 41 });
			t.AddRow(new object[] { "PJ Lee", 4, @"C:\temp\Michael\quick\obj", 34 });
			t.AddRow(new object[] { "Li Ann", "Black", @"C:\temp\abc\quick\obj", 12 });
			t.AddRow(new object[] { "Pi Wa", "Doll", @"C:\temp\John\quick\obj", 32 });

			int n = 1;
			string NL = Environment.NewLine;
			Console.WriteLine($"Sample {n++}: Generating the table{NL}");
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Sort by name{NL}");
			t.SortSyntax = "Full Name";
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Updating 3rd column to warp after 30 chars, indenting in 2{NL}");
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
			t.Indentation = 2;
			t.SortSyntax = "[Full Name]";
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Updating also the 1st column to warp on 6 chars{NL}");
			t.UpdateColumn(0, StringTable.WrapText.Wrap, 6);
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Adding an index column{NL}");
			t.AddIndexLineColumn = true;
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Indenting by 4 chars and trimming cell values in the 2nd column{NL}");
			t.UpdateColumn(0, StringTable.WrapText.TrimEnd, int.MaxValue); 
			t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 30);
			t.Indentation = 4;
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample {n++}: Sort by numeric score{NL}");
			t = new StringTable(new string[] { "Full Name", "Nickname", "Folder", "Score" }, new List<int>() { 3 }) {
				SortSyntax = "Score",
				Indentation = 2
			};
			t.AddRow(new object[] { "JJ Xi", "Smith", @"C:\temp\a\quick\obj", 27 });
			t.AddRow(new object[] { "David Monson", @"Nice", @"C:\dev\kafkaclient\McpsWorker\obj\NetCore31", 7 });
			t.AddRow(new object[] { "David Johnson", @"Nicer", @"C:\dev\kafkaclient\McpsWorker\obj\Debug", 41 });
			t.AddRow(new object[] { "PJ Lee", 4, @"C:\temp\Michael\quick\obj", 34 });
			t.AddRow(new object[] { "Li Ann", "Black", @"C:\temp\abc\quick\obj", 12 });
			t.AddRow(new object[] { "Pi Wa", "Doll", @"C:\temp\John\quick\obj", 32 });
			Console.WriteLine(t.CompileTable());

			// Now plotting
			Dictionary<double, double> v = new Dictionary<double, double>();
			string plot;

			Console.WriteLine($"{NL}Sample {n++}: Sin Plot{NL}");
			v.Clear();
			for (int i = 0; i <= 360 * 2; i++) { v[i] = Math.Sin(i * 3.14 / 180); }
			plot = StringChart.Plot(v, new Options() { YLabelformat = "0.0", YMargin = 5, Rows = 30, Columns = 50, XTicks = 4, XLabelformat = "0" });
			Console.WriteLine(plot);

			Console.WriteLine($"{NL}Sample {n++}: Sin Plot{NL}");
			v.Clear();
			for (int i = 0; i <= 360 * 2; i++) { v[i] = Math.Sin(i * 3.14 / 180); }
			plot = StringChart.Plot(v, new Options() { YLabelformat = "0.0", YMargin = 5, Rows = 30, Columns = 30, XTicks = 4, XLabelformat = "0" });
			Console.WriteLine(plot);

			Console.WriteLine($"{NL}Sample {n++}: Sin Plot{NL}");
			v.Clear();
			for (int i = 0; i <= 360 * 2; i++) { v[i] = Math.Sin(i * 3.14 / 180); }
			plot = StringChart.Plot(v, new Options() { YLabelformat = "0.0", YMargin = 5, Rows = 30, Columns = 120, XTicks = 4, XLabelformat = "0" });
			Console.WriteLine(plot);

			Console.WriteLine($"{NL}Sample {n++}: Sin Plot{NL}");
			v.Clear();
			for (int i = 0; i < 100; i++) { v[i] = (Math.Sin(i * 3.14 / 180)); }
			plot = StringChart.Plot(v, new Options() { YLabelformat = "0.0", YMargin = 5, Rows = 30, Columns = 70, XTicks = 4, XLabelformat = "0.00" }); 
			Console.WriteLine(plot);

			Console.WriteLine($"{NL}Sample {n++}: Exp Plot{NL}");
			v.Clear();
			for (double i = 1; i < 10; i+=.1) { v[i] = (Math.Exp(i)); }
			plot = StringChart.Plot(v, new Options() { YLabelformat = "0.0", YMargin = 5, Rows = 30, Columns = 90, XTicks = 8, XLabelformat = "0.00" });
			Console.WriteLine(plot);
		}
	}
}
