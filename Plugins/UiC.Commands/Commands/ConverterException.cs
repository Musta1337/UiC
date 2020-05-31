using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UiC.Commands.Commands {
    public class ConverterException : Exception {
        public ConverterException() {
        }

        public ConverterException(string message) : base(message) {
        }

        public ConverterException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ConverterException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
