using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Network.Handlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HandlerAttribute : Attribute
    {
        public HandlerAttribute(uint messageId)
        {
            MessageId = messageId;
        }

        protected HandlerAttribute()
        {
            throw new NotImplementedException();
        }

        public uint MessageId
        {
            get;
            set;
        }

        public override string ToString()
        {
            return MessageId.ToString();
        }
    }
}
