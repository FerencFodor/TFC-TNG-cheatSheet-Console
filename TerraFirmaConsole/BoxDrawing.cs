using System.Collections.Generic;
using System.Linq;

namespace TerraFirmaConsole
{
    public class BoxDrawing
    {
        public BoxDrawing()
        {
            Box = new List<string>();
        }

        public BoxDrawing(int width, int height)
        {
            Box = new List<string>();
            Width = width;
            Height = height;
        }

        private List<string> Box { get; }
        public int Width { get; set; }
        public int Height { get; set; }

        public BoxDrawing DrawBox()
        {
            for (var i = 0; i < Height; i++)
            {
                if (i == 0 && Box.Count > 0)
                {
                    Box[^1] = (char) TerraFirmaConsole.Border.SplitVerticalRight +
                              string.Concat(Enumerable.Repeat((char) TerraFirmaConsole.Border.HorizontalLine, Width - 2)) +
                              (char) TerraFirmaConsole.Border.SplitVerticalLeft;
                    continue;
                }

                if (i == 0 && Box.Count == 0)
                {
                    Box.Add(
                        (char) TerraFirmaConsole.Border.CornerUpRight +
                        string.Concat(Enumerable.Repeat((char) TerraFirmaConsole.Border.HorizontalLine, Width - 2)) +
                        (char) TerraFirmaConsole.Border.CornerUpLeft);
                    continue;
                }

                if (i == Height - 1)
                {
                    Box.Add(
                        (char) TerraFirmaConsole.Border.CornerDownRight +
                        string.Concat(Enumerable.Repeat((char) TerraFirmaConsole.Border.HorizontalLine, Width - 2)) +
                        (char) TerraFirmaConsole.Border.CornerDownLeft);
                    continue;
                }

                Box.Add(
                    (char) TerraFirmaConsole.Border.VerticalLine +
                    string.Concat(Enumerable.Repeat(' ', Width - 2)) +
                    (char) TerraFirmaConsole.Border.VerticalLine);
            }
            
            CheckIntegrity();

            return this;
        }

        public BoxDrawing DrawRow(params string[] values)
        {
            //var numCell = values.Length;

            return this;
        }

        public BoxDrawing DrawRow(int numCell)
        {
            var cellSize = (Width - 2 - (numCell - 1)) / numCell;
            var remainder = (Width - 2 - (numCell - 1)) % numCell;

            Box.RemoveAt(Box.Count - 1);
            Box.Add("");

            for (var i = 0; i < Height - 2; i++)
                Box.Add("");

            Box.Add("");

            for (var i = 0; i < numCell; i++)
            {
                if (i == 0)
                {
                    Box[^Height] += Box.Count - Height > 0
                        ? (char) TerraFirmaConsole.Border.SplitVerticalRight
                        : (char) TerraFirmaConsole.Border.CornerUpRight;
                    for (var j = 0; j < Height - 2; j++) Box[^(2 + j)] += (char) TerraFirmaConsole.Border.VerticalLine;
                    Box[^1] += (char) TerraFirmaConsole.Border.CornerDownRight;
                }

                if (remainder > 0)
                    cellSize += 1;

                var index = 1 + (1 + i) * cellSize + i;

                Box[^Height] += string.Concat(Enumerable.Repeat((char) TerraFirmaConsole.Border.HorizontalLine, cellSize));
                for (var j = 0; j < Height - 2; j++) Box[^(2 + j)] += string.Concat(Enumerable.Repeat(" ", cellSize));
                Box[^1] += string.Concat(Enumerable.Repeat((char) TerraFirmaConsole.Border.HorizontalLine, cellSize));

                Box[^Height] +=
                    Box[^(Height + 1)][index] == (char) TerraFirmaConsole.Border.VerticalLine
                        ? (char) TerraFirmaConsole.Border.Cross
                        : (char) TerraFirmaConsole.Border.SplitHorizontalDown;
                for (var j = 0; j < Height - 2; j++) Box[^(2 + j)] += (char) TerraFirmaConsole.Border.VerticalLine;
                Box[^1] += (char) TerraFirmaConsole.Border.SplitHorizontalUp;

                if (remainder > 0)
                {
                    cellSize -= 1;
                    remainder--;
                }
                
            }

            Box[^Height] = Box[^Height].Remove(Box[^Height].Length - 1, 1) +
                           (Box.Count - Height > 0
                               ? (char) TerraFirmaConsole.Border.SplitVerticalLeft
                               : (char) TerraFirmaConsole.Border.CornerUpLeft);
            for (var j = 0; j < Height - 2; j++)
                Box[^(2 + j)] = Box[^(2 + j)].Remove(Box[^(2 + j)].Length - 1, 1) +
                                (char) TerraFirmaConsole.Border.VerticalLine;
            Box[^1] = Box[^1].Remove(Box[^1].Length - 1, 1) + (char) TerraFirmaConsole.Border.CornerDownLeft;

            CheckIntegrity();
            
            return this;
        }

        public BoxDrawing DrawHeader1(string header)
        {
            DrawBox();

            var middleIndex = (Box.Count - Box.Count % 2) / 2;
            var middleLine = Box[middleIndex];

            var space = middleLine.Length;
            space -= header.Length;
            space = (space - space % 2) / 2;

            middleLine = middleLine.Remove(space, header.Length);
            middleLine = middleLine.Insert(space, header);

            Box[middleIndex] = middleLine;
            return this;
        }

        private void CheckIntegrity()
        {
            if (Box.Count - Height == 0)
                return;

            for (var i = 1; i < Width-1; i++)
            {
                if (Box[^(Height + 1)][i] == (char) TerraFirmaConsole.Border.VerticalLine)
                {
                    if (Box[^(Height - 1)][i] == (char) TerraFirmaConsole.Border.VerticalLine)
                    {
                        Box[^Height] = Box[^Height].Remove(i, 1);
                        Box[^Height] = Box[^Height].Insert(i, "" + (char) TerraFirmaConsole.Border.Cross);
                    }
                    else
                    {
                        Box[^Height] = Box[^Height].Remove(i, 1);
                        Box[^Height] = Box[^Height].Insert(i, "" + (char) TerraFirmaConsole.Border.SplitHorizontalUp);
                    }
                }
                else if(Box[^Height][i] == (char) TerraFirmaConsole.Border.Cross)
                {
                    if(Box[^(Height-1)][i] == (char) TerraFirmaConsole.Border.VerticalLine &&
                       Box[^(Height+1)][i] == (char) TerraFirmaConsole.Border.VerticalLine)
                        continue;
                    
                    Box[^Height] = Box[^Height].Remove(i, 1);

                    var insert = "";
                    insert += Box[^(Height - 1)][i] == (char) TerraFirmaConsole.Border.VerticalLine
                        ? (char) TerraFirmaConsole.Border.SplitHorizontalDown
                        : (char) TerraFirmaConsole.Border.SplitHorizontalUp;
                    Box[^Height] = Box[^Height].Insert(i, insert);
                }
            }
        }
        
        public BoxDrawing DrawHeader(string header)
        {
            DrawBox();

            int lastBox;

            if (Box.Count - Height == 0)
                lastBox = 0;
            else
                lastBox = Box.FindIndex(Box.Count - Height, x =>
                    x[0] == (char) TerraFirmaConsole.Border.CornerDownRight ||
                    x[0] == (char) TerraFirmaConsole.Border.SplitVerticalRight);

            var middleIndex = lastBox + (Height - Height % 2) / 2;
            var middleLine = Box[middleIndex];

            var space = middleLine.Length;
            space -= header.Length;
            space = (space - space % 2) / 2;

            middleLine = middleLine.Remove(space, header.Length);
            middleLine = middleLine.Insert(space, header);

            Box[middleIndex] = middleLine;
            return this;
        }

        public IEnumerable<string> ValidateDraw()
        {
            return Box;
        }
    }
}