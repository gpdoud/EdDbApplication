using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib.Exceptions {
    
    public class ReferentialIntegrityException : Exception {

        public ReferentialIntegrityException() 
            : base() { }

        public ReferentialIntegrityException(string Message)
            : base(Message) { }

        public ReferentialIntegrityException(string Message, Exception innerException)
            : base(Message, innerException) { }
    }
}
