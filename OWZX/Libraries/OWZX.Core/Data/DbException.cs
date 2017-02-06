using System;
using System.Runtime.Serialization;

namespace OWZX.Core
{
    /// <summary>
    /// OWZXÊý¾Ý¿âÒì³£
    /// </summary>
    [Serializable]
    public class DbException : BSPException
    {
        public DbException() : base() { }

        public DbException(string message) : base(message) { }

        public DbException(string message, Exception inner) : base(message, inner) { }

        public DbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
