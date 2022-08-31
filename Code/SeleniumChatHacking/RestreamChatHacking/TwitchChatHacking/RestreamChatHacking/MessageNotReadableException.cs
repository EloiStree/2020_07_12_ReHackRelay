
using System;


    [Serializable]
    public class MessageNotReadableException : Exception
    {
        public MessageNotReadableException()
        {
        }

        public MessageNotReadableException(string message) : base(message)
        {
        }

        public MessageNotReadableException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
    }
