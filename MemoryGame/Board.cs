using System;
using System.Text;

// ReSharper disable once CheckNamespace
namespace MemoryGameUI
{
    public class Board
    {
        private int m_NumberOfCharsInRow;
        private StringBuilder m_Board;

        private StringBuilder createBoard(int i_Row)
        {
            StringBuilder board = new StringBuilder();
            string rowLine = createRow();

            board.AppendLine(createLineOfLetters());
            m_NumberOfCharsInRow = rowLine.Length;
            for(int i = 0; i < i_Row; i++)
            {
                board.AppendLine(rowLine);
                board.AppendLine(createColumn(i_RowNumber: i + 1));
            }

            board.Append(rowLine);

            return board;
        }

        public void SetBoardSize(string i_BoardSize)
        {
            int column = int.Parse(i_BoardSize[0].ToString());
            int row = int.Parse(i_BoardSize[2].ToString());
            this.Row = row;
            this.Column = column;
            m_Board = createBoard(row);
        }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public void Draw()
        {
            Console.WriteLine(m_Board.ToString());
        }

        private string createLineOfLetters()
        {
            char letterToDraw = 'A';
            StringBuilder lettersRow = new StringBuilder();
            for(int i = 0; i < Column; i++)
            {
                lettersRow.Append("     " + letterToDraw);
                letterToDraw++;
            }

            lettersRow.Append("   ");

            return lettersRow.ToString();
        }

        private string createRow()
        {
            StringBuilder rowLine = new StringBuilder();
            for(int i = 0; i < Column; i++)
            {
                rowLine.Append("======");
            }

            rowLine.Append("===");

            return rowLine.ToString();
        }

        private string createColumn(int i_RowNumber)
        {
            StringBuilder columnLine = new StringBuilder();

            columnLine.Append(i_RowNumber + " ");
            for(int i = 0; i < Column; i++)
            {
                columnLine.Append("|     ");
            }

            columnLine.Append("|");

            return columnLine.ToString();
        }

        public void Replace(string i_PositionStr, string i_Value)
        {
            int position = calculatePosition(i_PositionStr);
            string oldValue = m_Board.ToString(position, length: 1);

            m_Board.Replace(oldValue, i_Value, position, count: 1);
            Console.Clear();
            Draw();
        }

        private int calculatePosition(string i_PositionOnBoard)
        {
            int rowLength = m_NumberOfCharsInRow + 2;
            int centerBox = 5;
            int rowNumber = int.Parse(i_PositionOnBoard[1].ToString());
            int columnNumber = i_PositionOnBoard[0] - 'A' + 1;
            int cardPosition = (2 * rowLength * rowNumber) + (centerBox * columnNumber) + (columnNumber - 1);

            return cardPosition;
        }

        public string ConvertIndexToStringPosition(int i_Row, int i_Column)
        {
            int index = (i_Row * this.Column) + i_Column;
            char column = 'A';
            column += (char)(index % this.Column);
            char row = '1';
            row += (char)(index / this.Column);
            return column + row.ToString();
        }
    }
}