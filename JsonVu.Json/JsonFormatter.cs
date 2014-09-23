using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    public sealed class JsonFormatter {

        private JsonReader jsonReader;

        public JsonFormatter(string json)
            : this(new StringReader(json)) {

        }

        public JsonFormatter(TextReader reader) {
            this.jsonReader = new JsonReader(reader);
        }
    
    }
}
