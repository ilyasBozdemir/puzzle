using System;
using System.Runtime.Serialization;

namespace puzzle_game
{
    [Serializable]
    public class PuzzleException : Exception
    {
        public PuzzleException()
        {

        }
        public PuzzleException(string message)
            : base(message)
        {

        }
        public PuzzleException(string message, Exception inner)
            : base(message, inner)
        {

        }
        protected PuzzleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
