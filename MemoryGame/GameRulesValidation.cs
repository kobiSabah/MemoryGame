using System;
using System.Threading;
using GameLogic;

// ReSharper disable once CheckNamespace
namespace MemoryGameUI
{
    public static class GameRulesValidation
    {
        /*
            Game Rules
        ====================
            1. BoardSize : Rows between 4 - 6
                           Column between 4 - 6
                           Total card can't be odd number

            2. UserName :  Have to contain at lest 2 character
                           Can't contain digit

            3. GameMode :  Number between 1 - 2
                           1 = PlayerVsPlayer
                           2 = PlayerVsPc
         */

        private const int k_MaxRowNumber = 6;
        private const int k_MinRowNumber = 4;
        private const int k_MaxColumnNumber = 6;
        private const int k_MinColumnNumber = 4;

        public static bool ValidateInput(string i_Guess)
        {
            bool isValid = i_Guess.Length == 2;
            validateQuit(i_Guess);
            if(isValid)
            {
                isValid = char.IsLetter(i_Guess[0]) && char.IsDigit(i_Guess[1]);
            }

            if(!isValid)
            {
                inValidGuessMsg();
            }

            return isValid;
        }

        public static bool ValidateBoardSize(string i_BoardSize)
        {
            int row = 0;
            int column;
            bool isValid = i_BoardSize.Length == 3;

            if (isValid)
            {
                isValid = int.TryParse(i_BoardSize[0].ToString(), out column)
                          && int.TryParse(i_BoardSize[2].ToString(), out row);
                if(isValid)
                {
                    isValid = char.ToUpper(i_BoardSize[1]) == 'X' || i_BoardSize[1] == ',';
                }

                if(isValid)
                {
                    isValid = row <= k_MaxRowNumber && row >= k_MinRowNumber && column <= k_MaxColumnNumber
                              && column >= k_MinColumnNumber;

                    if (isValid)
                    {
                        isValid = (row * column) % 2 == 0;
                    }
                }
            }

            if(!isValid)
            {
                inValidBoardSizeMsg();
            }

            return isValid;
        }

        public static bool ValidateUser(string i_UserName)
        {
            bool isValid = i_UserName.Length > 2;

            if (isValid)
            {
                for (int i = 0; i < i_UserName.Length; i++)
                {
                    isValid = char.IsLetter(i_UserName[i]);
                    if(!isValid)
                    {
                        break;
                    }
                }
            }

            if(!isValid)
            {
                inValidUserNameMsg();
            }

            return isValid;
        }

        public static bool ValidateGameMode(string i_GameMode)
        {
            bool isValid = i_GameMode.Length == 1;

            if(isValid)
            {
                isValid = (i_GameMode[0] == '1') || (i_GameMode[0] == '2' && i_GameMode.Length == 1);
            }

            if(!isValid)
            {
                inValidGameModeMsg();
            }

            return isValid;
        }

        // $G$ DSN-002 (-10) You should not make UI calls from your logic classes.
        private static void cardExposeMsg()
        {
            Console.WriteLine("Card expose try another one.");
        }

        // $G$ CSS-999 (-3) You should have used constants\enum here.
        public static bool ValidateNewGame(string i_Input)
        {
            bool isValid = i_Input.Length == 1;

            if(isValid)
            {
                isValid = char.ToUpper(i_Input[0]) != 'Y' || char.ToUpper(i_Input[0]) != 'N';
                if(isValid && char.ToUpper(i_Input[0]) == 'N')
                {
                    quitMsg();
                }
            }

            return isValid;
        }

        private static void validateQuit(string i_Input)
        {
            if(i_Input.Length == 1)
            {
                if(char.ToUpper(i_Input[0]) == 'Q')
                {
                    quitMsg();
                }
            }
        }


        // $G$ DSN-002 (-10) You should not make UI calls from your logic classes.
        private static void positionOutOfBoundMsg()
        {
            Console.WriteLine("ERROR : Out of bound position. {0}Try Again.", Environment.NewLine);
        }

        private static void quitMsg()
        {
            Console.WriteLine("GoodBye");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        private static void inValidBoardSizeMsg()
        {
            Console.WriteLine("ERROR: Board Size have to be with rows and column between 4 - 6 except 5X5. {0} Try again.", Environment.NewLine);
        }

        private static void inValidGameModeMsg()
        {
            Console.WriteLine("ERROR : Please Choose correct game mode {0}Try again.", Environment.NewLine);
        }

        private static void inValidUserNameMsg()
        {
            Console.WriteLine("ERROR: Please enter a proper name consisting of English letters only  {0} Try again.", Environment.NewLine);
        }

        private static void inValidGuessMsg()
        {
            Console.WriteLine("ERROR : Can't find the position.{0}Try Again.", Environment.NewLine);
        }

        public static bool ExceptionsCatch(eExceptions i_Exception)
        {
            bool isValid = true;

            if(i_Exception == eExceptions.CardExpose)
            {
                isValid = false;
                cardExposeMsg();
            }
            else if(i_Exception == eExceptions.OutOfBound)
            {
                isValid = false;
                positionOutOfBoundMsg();
            }
            else if(i_Exception == eExceptions.InvalidGuess)
            {
                isValid = false;
                inValidGuessMsg();
            }

            return isValid;
        }
    }
}