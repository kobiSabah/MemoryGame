// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public struct Player
    {
        private string m_UserName;
        private int m_Score;

        public string UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                m_UserName = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }
    }
}
