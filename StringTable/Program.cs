using System;
using System.Collections;
using System.Collections.Generic;

namespace StringTableDemo {
	class Program {
		static void Main(string[] args) {
			StringTable t = new StringTable(new string[] { "Full Name", "Nickname", "Folder", "Score" }) {
				DrawHeader = true,
				Indentation = 2
			};
			t.AddRow(new object[] { "JJ Xi", "Smith", @"C:\temp\a\quick\obj", 27 });
			t.AddRow(new object[] { "David Monson", @"Nice", @"C:\dev\kafkaclient\McpsWorker\obj\NetCore31", 7 });
			t.AddRow(new object[] { "David Johnson", @"Nicer", @"C:\dev\kafkaclient\McpsWorker\obj\Debug", 41 });
			t.AddRow(new object[] { "PJ Lee", 4, @"C:\temp\Michael\quick\obj", 34 });
			t.AddRow(new object[] { "Li Ann", "Black", @"C:\temp\abc\quick\obj", 12 });
			t.AddRow(new object[] { "Pi Wa", "Doll", @"C:\temp\John\quick\obj", 32 });

			string NL = Environment.NewLine;
			Console.WriteLine($"Sample 1: Generating the table sorted by name{NL}");
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample 2: Updating 3rd column to warp after 30 chars{NL}");
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
			t.SortSyntax = "[Full Name]";
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample 3: Updating also the 1st column to warp on 6 chars{NL}");
			t.UpdateColumn(0, StringTable.WrapText.Wrap, 6);
			t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample 4: Adding an index column, indenting by 4 chars, {NL}and trimming cell values in the 2nd column{NL}");
			t.AddIndexLineColumn = true;
			t.UpdateColumn(0, StringTable.WrapText.TrimEnd, int.MaxValue); 
			t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 30);
			t.Indentation = 4;
			Console.WriteLine(t.CompileTable());

			Console.WriteLine($"{NL}Sample 5: Sort by numeric score{NL}");
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
		}
	}
}
