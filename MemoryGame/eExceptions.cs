using System;

namespace GameLogic
{
    [Flags]
    public enum eExceptions
    {
        CardExpose,
        OutOfBound,
        InvalidGuess,
        ValidGuess
    }
}