using System;

namespace CryptOverseeMobileApp.Exceptions
{
    public class RestCallException : Exception
    {
        public RestCallException(string s, Exception exception) : base(s, exception)
        {
        }

        public RestCallException(string s) : base(s)
        {
        }
    }
}