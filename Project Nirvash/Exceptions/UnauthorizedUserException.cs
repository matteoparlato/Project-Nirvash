using System;

namespace Project_Nirvash.Exceptions
{
    /// <summary>
    /// UnauthorizedUserException class
    /// </summary>
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException(string message) : base(message)
        {
            //
        }

        public UnauthorizedUserException(string message, Exception innerException) : base(message, innerException)
        {
            //
        }
    }
}
