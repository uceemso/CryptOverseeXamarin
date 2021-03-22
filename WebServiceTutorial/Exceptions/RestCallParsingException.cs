using System;

namespace CryptOverseeMobileApp.Exceptions
{
    public class RestCallParsingException : Exception
    {
        public RestCallParsingException(string s, Exception exception): base(s, exception)
        {
        }
    }
}