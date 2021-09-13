using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace TerraFirmaConsole
{
    public class Table
    {
        
        
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

        private string HorizontalLine(bool header = false, bool isMiddle = false)
        {
            var table = string.Empty;

            var firstChar = (char) (header ? Border.CornerUpRight : isMiddle ? Border.SplitVerticalRight : Border.CornerDownRight);
            var spacerChar = (char) (header ? Border.HorizontalLine : isMiddle ? Border.Cross : Border.SplitHorizontalUp);
            var lastChar = (char) (header ? Border.CornerUpLeft : isMiddle ? Border.SplitVerticalLeft : Border.CornerDownLeft);
            
            table+=firstChar;
            
            Columns.ForEach(column =>
            {
                table+=Enumerable.Repeat((char)Border.HorizontalLine, column).ToString();
                table+=spacerChar;
            });

            table = table.Remove(table.Length - 1, 1) + lastChar;

            return table;
        }

        public string Build()
        {
            var table = new StringBuilder();
            
            if (Header != null)
            {
                //+====+
                table.Append(HorizontalLine(true));
                table.Append('\n');
                
                //| Header |
                table.Append((char)Border.VerticalLine);
                
                var space = Columns.Sum() + Column - 1 - Header.Length;
                var remainder = space % 2;
                space = (space - remainder) / 2 + remainder;
                table.Append(Header.PadLeft(space+Header.Length));
                for (var i = 0; i < space - remainder; i++)
                    table.Append(' ');
                
                table.Append((char)Border.VerticalLine);
                table.Append('\n');
            }
            // +===T===T===+
            // +===+===+===+
            for (var j = 0; j < Row; j++)
            {
                var rowData = TableList[j];

                if (Header != null)
                {
                    table.Append(Border.SplitVerticalRight);
                }
                else if (j == 0)
                {
                    table.Append(Border.CornerUpRight);
                }

                foreach (var column in Columns)
                {
                    for (var i = 0; i < column; i++)
                        table.Append(Border.HorizontalLine);
                    table.Append(Header != null && j == 0 ? Border.SplitHorizontalDown : Border.Cross);
                }

                if (Header != null)
                {
                    table[^1] = (char) Border.SplitVerticalLeft;
                }
                else if (j == 0)
                {
                    table[^1] = (char) Border.CornerUpLeft;
                }
                
                table.Append('\n');
                
                // | text | text | text |
    
                table.Append((char)Border.VerticalLine);

                for (var i = 0; i < Column; i++)
                {
                    var space = Columns[i] - rowData[i].Length - 1;
                    space = space < 0 ? 0 : space;
                    table.Append(' ');
                    table.Append(rowData[i].PadRight(space+rowData[i].Length));
                    table.Append((char)Border.VerticalLine);
                }

                table.Append('\n');
            }
            
            // +===+===+===+

            table.Append(Border.CornerDownRight);
            foreach (var column in Columns)
            {
                for (var i = 0; i < column; i++)
                    table.Append(Border.HorizontalLine);
                table.Append(Border.SplitHorizontalUp);
            }

            table[^1] = (char)Border.CornerDownLeft;

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