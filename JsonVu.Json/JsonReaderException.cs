using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    public class JsonReaderException: Exception{

        public JsonReaderException(string message, int position, int lineNumber)
            : this(message, position, lineNumber, null) {

        }

        public JsonReaderException(string message, int position, int lineNumber, Exception inner)
            : base(message, inner) {

                this.Position = position;
                this.LineNumber = lineNumber;
        }

        public int Position { get; private set; }

        public int LineNumber { get; private set; }
    }
}
