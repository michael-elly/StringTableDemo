# StringTable
This project demonstrates how to use a class called *StringTable.cs*

StringTable.cs is a single independent class that can be copied to you C# project. This class formats a table as a plain text. It will automatically calculate the best width of each column and format the table to be nicely displayed in terminal or any monospaced-font display. 

Tables can be defined with: 
* Underlined column headers
* Index column
* Limited size of column width with trim or wrap display options
* Sort options
* Indent the whole table

Here's an example of creation a table with 4 columns, entering the values, and generating the text for the table to be displayed:
```cs
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

Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
  Full Name     Nickname Folder                                      Score
  ------------- -------- ------------------------------------------- -----
  JJ Xi         Smith    C:\temp\a\quick\obj                         27
  David Monson  Nice     C:\dev\kafkaclient\McpsWorker\obj\NetCore31 7
  David Johnson Nicer    C:\dev\kafkaclient\McpsWorker\obj\Debug     41
  PJ Lee        4        C:\temp\Michael\quick\obj                   34
  Li Ann        Black    C:\temp\abc\quick\obj                       12
  Pi Wa         Doll     C:\temp\John\quick\obj                      32
</pre>

### Sample 2
Updating 3rd column to warp after 30 chars:
```cs
t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
t.SortSyntax = "[Full Name]";
Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
  Full Name     Nickname Folder                         Score
  ------------- -------- ------------------------------ -----
  David Johnson Nicer    C:\dev\kafkaclient\McpsWorker\ 41
                         obj\Debug
  David Monson  Nice     C:\dev\kafkaclient\McpsWorker\ 7
                         obj\NetCore31
  JJ Xi         Smith    C:\temp\a\quick\obj            27
  Li Ann        Black    C:\temp\abc\quick\obj          12
  Pi Wa         Doll     C:\temp\John\quick\obj         32
  PJ Lee        4        C:\temp\Michael\quick\obj      34
</pre>


### Sample 3
Updating also the 1st column to warp on 6 chars:
```cs
t.UpdateColumn(0, StringTable.WrapText.Wrap, 6);
t.UpdateColumn(2, StringTable.WrapText.Wrap, 30);
Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
  Full N Nickname Folder                         Score
  ------ -------- ------------------------------ -----
  David  Nicer    C:\dev\kafkaclient\McpsWorker\ 41
  Johnso          obj\Debug
  n
  David  Nice     C:\dev\kafkaclient\McpsWorker\ 7
  Monson          obj\NetCore31
  JJ Xi  Smith    C:\temp\a\quick\obj            27
  Li Ann Black    C:\temp\abc\quick\obj          12
  Pi Wa  Doll     C:\temp\John\quick\obj         32
  PJ Lee 4        C:\temp\Michael\quick\obj      34
</pre>

### Sample 4
Adding an index column, indenting by 4 chars, and trimming cell values in the 2nd column:
```cs
t.AddIndexLineColumn = true;
t.UpdateColumn(0, StringTable.WrapText.TrimEnd, int.MaxValue); 
t.UpdateColumn(2, StringTable.WrapText.TrimEnd, 30);
t.Indentation = 4;
Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
    # Full Name     Nickname Folder                         Score
    - ------------- -------- ------------------------------ -----
    1 David Johnson Nicer    C:\dev\kafkaclient\McpsWorke.. 41
    2 David Monson  Nice     C:\dev\kafkaclient\McpsWorke.. 7
    3 JJ Xi         Smith    C:\temp\a\quick\obj            27
    4 Li Ann        Black    C:\temp\abc\quick\obj          12
    5 Pi Wa         Doll     C:\temp\John\quick\obj         32
    6 PJ Lee        4        C:\temp\Michael\quick\obj      34
</pre>

### Sample 5
Sort by a numeric column:
```cs
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
```

And the output will look like: 
<pre>
  Full Name     Nickname Folder                                      Score
  ------------- -------- ------------------------------------------- -----
  David Monson  Nice     C:\dev\kafkaclient\McpsWorker\obj\NetCore31 7
  Li Ann        Black    C:\temp\abc\quick\obj                       12
  JJ Xi         Smith    C:\temp\a\quick\obj                         27
  Pi Wa         Doll     C:\temp\John\quick\obj                      32
  PJ Lee        4        C:\temp\Michael\quick\obj                   34
  David Johnson Nicer    C:\dev\kafkaclient\McpsWorker\obj\Debug     41
</pre>

