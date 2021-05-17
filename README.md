# StringTableDemo
This project demonstrates how to use a class called *StringTable.cs*

StringTable.cs is an independent class which can be used to generate a string table for a mono spaced fonts text. 

Tables can be defined with: 
* Named columns
* Limited size of column width with trim or wrap display options
* Index column
* Sort 

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

And the output will look like 
<pre>
  # A   B                     C     D  
  - --- --------------------- ----- -- 
  1 34  David Davidson Monson Nice  8  
  2 132 Dora                  Smith DD 
  3 23  John                  Smith DD 
  4 432 Lisa                  Black DD 
  5 43  Paul                  4     DD 
</pre>

To updating column B to warp on 16 chars
```cs
t.UpdateColumn(1, StringTable.WrapText.Wrap, 15);
Console.WriteLine(t.CompileTable());
```

And the output will look like 
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

To remove the index column and trim column B to 15 chars
```cs
t.AddIndexLineColumn = false;
t.UpdateColumn(1, StringTable.WrapText.TrimEnd, 15);
Console.WriteLine(t.CompileTable());
```

And the output will look like 
<pre>
  A   B               C     D  
  --- --------------- ----- -- 
  34  David Davidso.. Nice  8  
  132 Dora            Smith DD 
  23  John            Smith DD 
  432 Lisa            Black DD 
  43  Paul            4     DD 
</pre>

