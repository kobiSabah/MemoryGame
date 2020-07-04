// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public class Memory<TValue>
    {
        private readonly Position r_Position;
        private readonly TValue r_Value;

        public Memory(Position i_Position, TValue i_Value)
        {
            r_Position = new Position { Column = i_Position.Column, Row = i_Position.Row };
            r_Value = i_Value;
        }

        public TValue Value
        {
            get
            {
                return r_Value;
            }
        }

        public Position Position
        {
            get
            {
                return r_Position;
            }
        }
    }
}