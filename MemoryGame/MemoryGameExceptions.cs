// ReSharper disable once CheckNamespace
namespace GameLogic
{
    public static class GameValidation
    {
        public static eExceptions ValidateGuess(Position i_Guess, Position i_BoardSize)
        {
            eExceptions isValid = eExceptions.ValidGuess;

            if (isValid == eExceptions.ValidGuess)
            {
                if (i_BoardSize.Row <= i_Guess.Row || i_Guess.Row < 0 || i_BoardSize.Column <= i_Guess.Column || i_Guess.Column < 0)
                {
                        isValid = eExceptions.OutOfBound;
                }
            }

            return isValid;
        }
    }
}