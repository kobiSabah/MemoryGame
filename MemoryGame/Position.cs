// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public struct Position
    {
        private int m_Row;
        private int m_Column;

        public Position(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public static Position Parse(string i_Position)
        {
            Position convertedPosition = new Position();

            if (char.IsLetter(i_Position[0]) && char.IsDigit(i_Position[1]) && i_Position.Length == 2)
            {
                convertedPosition.Row = int.Parse(i_Position[1].ToString()) - 1;
                convertedPosition.Column = int.Parse((i_Position[0] - 'A').ToString());
            }

            return convertedPosition;
        }

        public bool IsEqual(Position i_PositionToCheck)
        {
            return this.Column == i_PositionToCheck.m_Column && this.Row == i_PositionToCheck.m_Row;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }
    }
}