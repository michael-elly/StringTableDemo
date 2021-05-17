# StringTableDemo
This project demonstrates how to use a class called *StringTable.cs*

StringTable.cs is a single independent class that can be copied to you C# project. This class generates a simple string which displays a table. It will automatically calculate the best width of each column and format the table to be nicely displayed in terminal or any monospaced-font based display. 

Tables can be defined with: 
* Underlined column headers
* Index column
* Limited size of column width with trim or wrap display options
* Sort options
* Indent the whole table

Here's an example of creation a table with 4 columns, entering the values, and generating the text for the table to be displayed:
```cs
StringTable t = new StringTable(new string[] { "A", "B", "C", "D" }) {
	SortSyntax = "B",
	AddIndexLineColumn = true,
	Indentation = 2
	};
t.AddRow(new object[] { 23, "John", "Smith", "DD" });
t.AddRow(new object[] { 34, "David Davidson Monson", "Nice", 8 });
t.AddRow(new object[] { 43, "Paul", 4, "DD" });
t.AddRow(new object[] { 132, "Dora", "Smith", "DD" });
t.AddRow(new object[] { 432, "Lisa", "Black", "DD" });
  
// Generating the table and displaying it
Console.WriteLine(t.CompileTable());

```

And the output will look like: 
<pre>
  # A   B                     C     D  
  - --- --------------------- ----- -- 
  1 34  David Davidson Monson Nice  8  
  2 132 Dora                  Smith DD 
  3 23  John                  Smith DD 
  4 432 Lisa                  Black DD 
  5 43  Paul                  4     DD 
</pre>

To updating column B to warp on 16 chars:
```cs
t.UpdateColumn(1, StringTable.WrapText.Wrap, 15);
Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
  # A   B               C     D  
  - --- --------------- ----- -- 
  1 34  David Davidson  Nice  8  
        Monson                   
  2 132 Dora            Smith DD 
  3 23  John            Smith DD 
  4 432 Lisa            Black DD 
  5 43  Paul            4     DD 
</pre>


To remove the index column, trim column B to 15 chars, and indent the table in 8 chars:
```cs
t.AddIndexLineColumn = false;
t.UpdateColumn(1, StringTable.WrapText.TrimEnd, 15);
t.Indentation = 8;
Console.WriteLine(t.CompileTable());
```

And the output will look like: 
<pre>
        A   B               C     D  
        --- --------------- ----- -- 
        34  David Davidso.. Nice  8  
        132 Dora            Smith DD 
        23  John            Smith DD 
        432 Lisa            Black DD 
        43  Paul            4     DD 
</pre>

