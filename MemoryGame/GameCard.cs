// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public class GameCard<TCard>
    {
        private readonly TCard r_Cards;
        private bool m_Visible;

        public GameCard(TCard i_Cards)
        {
            r_Cards = i_Cards;
            m_Visible = false;
        }

        public TCard Card
        {
            get
            {
                return r_Cards;
            }
        }

        public bool Visible
        {
            get
            {
                return m_Visible;
            }

            set
            {
                m_Visible = value;
            }
        }
    }
}