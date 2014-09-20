using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    public enum JsonToken {
        PropertyName,
        Value,
        StartObject,
        EndObject,
        StartArray,
        EndArray,
        Unknown
    }
}
