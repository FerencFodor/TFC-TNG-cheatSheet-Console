using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraFirmaConsole
{
    public class Table
    {
        public static readonly char[] Box =
        {
            '\u2500', //─
            '\u2502', //│
            '\u250C', //┌
            '\u2510', //┐
            '\u2514', //└
            '\u2518', //┘
            '\u251C', //├
            '\u2524', //┤
            '\u252C', //┬
            '\u2534', //┴
            '\u253C'  //┼
        };
        
        private int CellSize { get; set; }
        private int Row { get; set; }
        private int Column { get; set; }
        private List<List<string>> TableList { get;  set; }
        private string Header { get; set; }
        private List<int> Columns { get; set; }

        public Table()
        {
            CellSize = 7; 
            Columns = new List<int>();
            TableList = new List<List<string>>();
            Row = 0;
            Column = 0;
        }

        public Table AddHeader(string name)
        {
            Header = name;
            return this;
        }

        public Table SetColumn(int num)
        {
            Column = num;
            return this;
        }

        public Table SetRow(int num)
        {
            Row = num;
            return this;
        }

        public Table AddRow(params string[] data)
        {
            if (Column == 1)
            {
                TableList.Add(new List<string>(data.ToList()));
                Column = TableList[0].Count;
            }
            else if (data.Length > Column)
            {
                var list = new List<string>();
                for (var i = 0; i < Row; i++)
                {
                    list.Add(data[i]);
                }
                TableList.Add(list);
  
            }
            else if (data.Length < Column)
            {
                var remaining = Row - data.Length;
                var list = new List<string>(data.ToList());
                for (var i = 0; i < remaining; i++)
                    list.Add("");
                
                TableList.Add(list);
            }
            else
            {
                TableList.Add(data.ToList());
            }

            Row++;
            return this;
        }

        public Table ChangeCell(int column, int row, string value)
        {
            TableList[row][column] = value;
            return this;
        }

        public Table Validate()
        {
            foreach (var row in TableList)
            {
                CheckCellSize(row);
            }

            return this;
        }

        public string Build()
        {
            var table = new StringBuilder();
            
            if (Header != null)
            {
                //+====+
                table.Append(Box[2]);
                
                foreach (var column in Columns)
                {
                    for (var i = 0; i < column; i++)
                        table.Append(Box[0]);
                    table.Append(Box[0]);
                }
                table[^1] = Box[3];
                table.Append('\n');
                
                //| Header |
                table.Append(Box[1]);
                
                var space = Columns.Sum() + Column - 1 - Header.Length;
                var remainder = space % 2;
                space = (space - remainder) / 2 + remainder;
                table.Append(Header.PadLeft(space+Header.Length));
                for (var i = 0; i < space - remainder; i++)
                    table.Append(' ');
                
                table.Append(Box[1]);
                table.Append('\n');
            }
            // +===T===T===+
            // +===+===+===+
            for (var j = 0; j < Row; j++)
            {
                var rowData = TableList[j];

                if (Header != null)
                {
                    table.Append(Box[6]);
                }
                else if (j == 0)
                {
                    table.Append(Box[2]);
                }

                foreach (var column in Columns)
                {
                    for (var i = 0; i < column; i++)
                        table.Append(Box[0]);
                    table.Append(Header != null && j == 0 ? Box[8] : Box[10]);
                }

                if (Header != null)
                {
                    table[^1] = Box[7];
                }
                else if (j == 0)
                {
                    table[^1] = Box[3];
                }
                
                table.Append('\n');
                
                // | text | text | text |
    
                table.Append(Box[1]);

                for (var i = 0; i < Column; i++)
                {
                    var space = Columns[i] - rowData[i].Length - 1;
                    space = space < 0 ? 0 : space;
                    table.Append(' ');
                    table.Append(rowData[i].PadRight(space+rowData[i].Length));
                    table.Append(Box[1]);
                }

                table.Append('\n');
            }
            
            // +===+===+===+

            table.Append(Box[4]);
            foreach (var column in Columns)
            {
                for (var i = 0; i < column; i++)
                    table.Append(Box[0]);
                table.Append(Box[9]);
            }

            table[^1] = Box[5];

            return table.ToString();
        }
        
        private void CheckCellSize(IReadOnlyList<string> row)
        {
            var offset = 2;
            
            if (row.Count > Columns.Count)
            {
                Column = row.Count;
                foreach (var t in row)
                    Columns.Add(t.Length > CellSize ? t.Length + offset: CellSize + offset);
            }
            else
            {
                for (var i = 0; i < Columns.Count; i++)
                {
                    var value = Columns[i];
                    value = value > CellSize ? value : CellSize;
                    Columns[i] = value > row[i].Length ? value : row[i].Length + offset;
                }
            }
        }
        
    }
}