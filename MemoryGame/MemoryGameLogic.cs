using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public class MemoryGameLogic<TCard>
    {
        private readonly int r_TotalPlayers;
        private readonly eGameMode r_GameMode;
        private readonly Player[] r_Players;
        private readonly List<Memory<TCard>> r_Memory;
        private int m_NumberOfCardsLeft;
        private GameCard<TCard>[] m_GameCards;
        private Position m_BoardSize;
        private int m_PlayerNumber;
        private eExceptions m_Exceptions;
        private readonly Random r_RandomGuess = new Random();
        public enum eGameMode
        {
            PlayerVsPlayer = 1,
            PlayerVsPc = 2,
        }

        public MemoryGameLogic(Player[] i_Players, Position i_BoardSize, eGameMode i_GameMode)
        {
            r_TotalPlayers = i_Players.Length;
            m_BoardSize = i_BoardSize;
            r_Players = i_Players;
            r_GameMode = i_GameMode;
            m_NumberOfCardsLeft = GetNumbersOfCards();
            r_Memory = new List<Memory<TCard>>(GetNumbersOfCards());
        }

        public Player GetWinner()
        {
            Player winner = new Player();
            int highScore = int.MinValue;

            for(int i = 0; i < r_Players.Length; i++)
            {
                if(r_Players[i].Score > highScore)
                {
                    highScore = r_Players[i].Score;
                    winner = r_Players[i];
                }
                else if(r_Players[i].Score == highScore)
                {
                    winner.UserName = "Draw";
                }
            }

            return winner;
        }

        public int GetPlayerScore(int i_PlayerNumber)
        {
            return r_Players[i_PlayerNumber].Score;
        }

        private int getCardPositionInArray(Position i_Position)
        {
            return (i_Position.Row * m_BoardSize.Column) + i_Position.Column;
        }

        public void PutCardsOnBoard(TCard[] i_DeckOfCards)
        {
            int i = 0;
            m_GameCards = new GameCard<TCard>[i_DeckOfCards.Length];
            foreach (TCard card in i_DeckOfCards)
            {
                m_GameCards[i] = new GameCard<TCard>(card);
                i++;
            }
        }

        public void NewGame()
        {
            for(int i = 0; i < r_Players.Length; i++)
            {
                r_Players[i].Score = 0;
            }
        }

        public int GetNumbersOfCards()
        {
            return m_BoardSize.Row * m_BoardSize.Column;
        }

        private TCard pcGetCard(Position i_CardPosition)
        {
            int positionInArray = getCardPositionInArray(i_CardPosition);
            return m_GameCards[positionInArray].Card;
        }

        public TCard GetCard(Position i_Position)
        {
            int positionInArray = getCardPositionInArray(i_Position);
            m_GameCards[positionInArray].Visible = true;
            if (r_GameMode == eGameMode.PlayerVsPc)
            {
                saveCard(i_Position, m_GameCards[positionInArray].Card);
            }

            return m_GameCards[positionInArray].Card;
        }

        public eExceptions ValidateGuess(Position i_Position)
        {
            m_Exceptions = GameValidation.ValidateGuess(i_Position, m_BoardSize);
            if(m_Exceptions == eExceptions.ValidGuess)
            {
                int positionInArray = getCardPositionInArray(i_Position);
                if(m_GameCards[positionInArray].Visible)
                {
                    m_Exceptions = eExceptions.CardExpose;
                }
            }

            return m_Exceptions;
        }

        private int nextPlayer(int i_CurrentPlayer)
        {
            if(i_CurrentPlayer < r_TotalPlayers)
            {
                m_PlayerNumber++;
                if(m_PlayerNumber == r_TotalPlayers)
                {
                    m_PlayerNumber = 0;
                }
            }

            return m_PlayerNumber;
        }

        public bool IsPair(Position i_FirstPosition, Position i_SecondPosition)
        {
            bool isPair = true;
            if(!pcGetCard(i_FirstPosition).Equals(pcGetCard(i_SecondPosition)))
            {
                isPair = false;
                m_GameCards[getCardPositionInArray(i_FirstPosition)].Visible = false;
                m_GameCards[getCardPositionInArray(i_SecondPosition)].Visible = false;
                m_PlayerNumber = nextPlayer(m_PlayerNumber);
            }
            else
            {
                m_NumberOfCardsLeft -= 2;
                r_Players[m_PlayerNumber].Score++;
            }

            return isPair;
        }

        public int PlayerTurn()
        {
            return m_PlayerNumber;
        }

        private bool isExpose(Position i_CardPosition)
        {
            int positionInArray = getCardPositionInArray(i_CardPosition);

            return m_GameCards[positionInArray].Visible;
        }

        public bool GameRun()
        {
            return m_NumberOfCardsLeft == 0;
        }

        public Position GetGuess()
        {
            Position guessPosition = new Position();
            bool pairFound = false;
            foreach(Memory<TCard> memory in r_Memory)
            {
                if(findCard(memory.Position, out guessPosition))
                {
                    pairFound = true;
                    break;
                }
            }

            if(!pairFound)
            {
                do
                {
                    guessPosition = makeGuess();
                }
                while(!validateGuess(guessPosition));

                saveCard(guessPosition, m_GameCards[getCardPositionInArray(guessPosition)].Card);
            }

            return guessPosition;
        }

        public Position GetGuess(Position i_FirstGuess)
        {
            Position guessPosition;

            if(findCard(i_FirstGuess, out guessPosition))
            {
                if(!isExpose(guessPosition))
                {
                    return guessPosition;
                }
            }

            do
            {
                guessPosition = makeGuess();
            }
            while(!validateGuess(i_FirstGuess, guessPosition));

            return guessPosition;
        }

        private void saveCard(Position i_CardPosition, TCard i_Value)
        {
            bool cardFound = false;
            foreach(Memory<TCard> memory in r_Memory)
            {
                if(memory.Value.Equals(i_Value) && !memory.Position.IsEqual(i_CardPosition))
                {
                    cardFound = true;
                }
            }

            if(!cardFound)
            {
                Memory<TCard> memory = new Memory<TCard>(i_CardPosition, i_Value);
                r_Memory.Add(memory);
            }
        }

        private bool findCard(Position i_CardPosition, out Position o_Match)
        {
            bool cardFound = false;
            o_Match = default(Position);
            TCard cardValue = m_GameCards[getCardPositionInArray(i_CardPosition)].Card;
            foreach (Memory<TCard> memory in r_Memory)
            {
                if (memory.Value.Equals(cardValue) && !i_CardPosition.IsEqual(memory.Position))
                {
                    cardFound = true;
                    o_Match = memory.Position;
                }
            }

            return cardFound;
        }


        private Position makeGuess()
        {

            int column = r_RandomGuess.Next(0, m_BoardSize.Column);
            int row = r_RandomGuess.Next(0, m_BoardSize.Row);
            Position position = new Position(row, column);

            return position;
        }

        private bool validateGuess(Position i_Guess)
        {
            bool isValid = i_Guess.Row < m_BoardSize.Row && i_Guess.Column < m_BoardSize.Column;

            if(isValid)
            {
                isValid = !isExpose(i_Guess);
            }

            return isValid;
        }

        private bool validateGuess(Position i_FirstGuess, Position i_SecondGuess)
        { 
            bool isValid = validateGuess(i_SecondGuess);
            if(isValid)
            {
                isValid = !i_FirstGuess.IsEqual(i_SecondGuess);
            }

            return isValid;
        }
    }
}
