using System;
using System.Runtime.Serialization;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX“Ï≥£¿‡
    /// </summary>
    [Serializable]
    public class BSPException : ApplicationException
    {
        public BSPException() { }

        public BSPException(string message) : base(message) { }

        public BSPException(string message, Exception inner) : base(message, inner) { }

        protected BSPException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
