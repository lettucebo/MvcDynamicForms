using System;

namespace MvcDynamicForms.NetCore.Exceptions
{
    class DuplicateException : Exception
    {
        private string _message;

        public override string Message
        {
            get { return this._message; }
        }

        public DuplicateException(string message)
        {
            this._message = message;
        }
    }
}