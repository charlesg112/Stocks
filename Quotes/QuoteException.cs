using System;

namespace Quotes
{
    public class QuoteException : Exception
    {
        public QuoteException() { }
        public QuoteException(string p_context) : base(String.Format("Quote exception : {0}", p_context)) { }
    }
}
