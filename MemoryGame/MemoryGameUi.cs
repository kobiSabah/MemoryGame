using System;
using System.Threading;
using GameLogic;

// ReSharper disable once CheckNamespace
namespace MemoryGameUI
{
    public class MemoryGameUi
    {
        private const int k_MaxPlayerNumber = 2;
        private const int k_DelayTime = 2000;
        private readonly Player[] r_Players;
        private MemoryGameLogic<Card> m_GameLogic;
        private Board m_GameBoard;
        private Card[] m_DeckOfCards;
        private int m_NumberOfPlayers;
        private MemoryGameLogic<Card>.eGameMode m_GameMode;
        
        public MemoryGameUi()
        {
            r_Players = new Player[k_MaxPlayerNumber];
            m_NumberOfPlayers = 0;
        }

        public void Initialize()
        {
            Console.WriteLine("====== Welcome to memory game ====== ");
            createPlayer();
            chooseGameMode();
            createBoard();
            initializeGameLogic();
            createDeckOfCards();
            startGame();
        }

        private void startGame()
        {
            m_GameBoard.Draw();
            if(m_GameMode == MemoryGameLogic<Card>.eGameMode.PlayerVsPlayer)
            {
                playerVsPlayer();
            }
            else
            {
                playerVsPc();
            }

            getWinner();
        }

        private string getPlayerGuess()
        {
            bool isValid;
            string guess;
            Position positionOnBoard = new Position();
            printPlayersScore();
            Console.WriteLine("{0}Enter card Position :", Environment.NewLine);
            do
            {
                guess = Console.ReadLine();
                isValid = GameRulesValidation.ValidateInput(guess);
                guess = guess?.ToUpper();
                if(isValid)
                {
                    positionOnBoard = Position.Parse(guess);
                    isValid = GameRulesValidation.ExceptionsCatch(
                            m_GameLogic.ValidateGuess(positionOnBoard));
                }
            }
            while (!isValid);

            Card card = m_GameLogic.GetCard(positionOnBoard);
            showCard(guess, card);

            return guess;
        }

        private void playerVsPlayer()
        {
            while(!m_GameLogic.GameRun())
            {
                playTurn();
            }
        }

        private void playTurn()
        {
            string firstGuess = getPlayerGuess();
            string secondGuess = getPlayerGuess();
            checkPairs(firstGuess, secondGuess);
        }

        private void playerVsPc()
        {
            while(!m_GameLogic.GameRun())
            {
                if(m_GameLogic.PlayerTurn() == 0)
                {
                    playTurn();
                }
                else
                {
                    getPcGuess();
                }
            }
        }

        private void initializeGameLogic()
        {
            Position boardSize = new Position(m_GameBoard.Row, m_GameBoard.Column);

            m_GameLogic = new MemoryGameLogic<Card>(
                r_Players, 
                boardSize, 
                m_GameMode);
        }

        private void printPlayersScore()
        {
            Console.Write("Score: ");
            for(int i = 0; i < m_NumberOfPlayers; i++)
            {
                Console.Write("{0}: {1}    ", r_Players[i].UserName, m_GameLogic.GetPlayerScore(i));
            }
        }

        private void checkPairs(string i_FirstGuess, string i_SecondGuess)
        {
            Position firsCardPosition = Position.Parse(i_FirstGuess);
            Position secondCardPosition = Position.Parse(i_SecondGuess);

            Thread.Sleep(k_DelayTime);
            if (!m_GameLogic.IsPair(firsCardPosition, secondCardPosition))
            {
                redraw(i_FirstGuess, i_SecondGuess);
            }
        }

        private void checkPairs(Position i_FirstGuess, Position i_SecondGuess)
        {
            string firstGuessStr = m_GameBoard.ConvertIndexToStringPosition(i_FirstGuess.Row, i_FirstGuess.Column);
            string secondGuessStr = m_GameBoard.ConvertIndexToStringPosition(i_SecondGuess.Row, i_SecondGuess.Column);

            checkPairs(firstGuessStr, secondGuessStr);
        }

        private void redraw(string i_Guess1, string i_Guess2)
        {
            m_GameBoard.Replace(i_Guess1, " ");
            m_GameBoard.Replace(i_Guess2, " ");
        }

        private void getWinner()
        {
            Player winner = m_GameLogic.GetWinner();

            if(winner.UserName == "Draw")
            {
                Console.WriteLine("It's Draw");
            }
            else
            {
                Console.WriteLine("The winner is {0} with {1} points", winner.UserName, winner.Score);
            }

            newGame();
        }

        private void newGame()
        {
            Console.WriteLine("Would you like to play again Y/N ?");
            string input;
            do
            {
                input = Console.ReadLine();
            }
            while (!GameRulesValidation.ValidateNewGame(input));

            createBoard();
            initializeGameLogic();
            createDeckOfCards();
            m_GameLogic.NewGame();
            startGame();
        }

        private void getPcGuess()
        {
            Position firstGuess = m_GameLogic.GetGuess();
            showCard(firstGuess);
            Position secondGuess = m_GameLogic.GetGuess(firstGuess);
            showCard(secondGuess);
            checkPairs(firstGuess, secondGuess);
        }

        private void showCard(string i_Position, Card i_CardToShow)
        {
            m_GameBoard.Replace(i_Position, i_CardToShow.Value);
        }

        private void showCard(Position i_Position)
        {
            string guess = m_GameBoard.ConvertIndexToStringPosition(i_Position.Row, i_Position.Column);

            Card card = m_GameLogic.GetCard(i_Position);
            m_GameBoard.Replace(guess, card.Value);
        }

        private void createBoard()
        {
            string boardSize;
            m_GameBoard = new Board();
            Console.WriteLine("Enter Row and Column : ");
            do
            {
                boardSize = Console.ReadLine();
            }
            while(!GameRulesValidation.ValidateBoardSize(boardSize));

            m_GameBoard.SetBoardSize(boardSize);
        }

        private void chooseGameMode()
        {
            Console.WriteLine(
@"Choose your game mode : 
1. Player vs Player.
2. Player vs PC.
");
            string modeNumber;
            do
            {
                modeNumber = Console.ReadLine();
            }
            while(!GameRulesValidation.ValidateGameMode(modeNumber));

            int result;
            int.TryParse(modeNumber, out result);
            m_GameMode = (MemoryGameLogic<Card>.eGameMode)result;

            if(m_GameMode == MemoryGameLogic<Card>.eGameMode.PlayerVsPlayer)
            {
                m_GameMode = MemoryGameLogic<Card>.eGameMode.PlayerVsPlayer;
                createPlayer();
            }
            else
            {
                m_GameMode = MemoryGameLogic<Card>.eGameMode.PlayerVsPc;
                r_Players[m_NumberOfPlayers] = new Player()
                                                   {
                                                       UserName = "PC"
                                                   };
                m_NumberOfPlayers++;
            }
        }

        private void createDeckOfCards()
        {
            int cardsNumber = m_GameLogic.GetNumbersOfCards();
            m_DeckOfCards = Card.CreatePairsDeck(cardsNumber);
            m_GameLogic.PutCardsOnBoard(m_DeckOfCards);
        }

        private void createPlayer()
        {
            string userName;
            Console.WriteLine("Enter Player {0} name : ", m_NumberOfPlayers + 1);
            do
            {
                userName = Console.ReadLine();
            }
            while (!GameRulesValidation.ValidateUser(userName));
            r_Players[m_NumberOfPlayers] = new Player()
                                               {
                                                   UserName = userName
                                               };
            m_NumberOfPlayers++;
        }
    }
}