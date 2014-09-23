using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonVu.Json.Tests {
    [TestClass]
    public class PosLineTest {

        [TestMethod]
        public void ポジションとライン() {
            var target = "{    \r\"FOO\": 1234,\"BAR\": [\n        1.0,  'ABCD'\r\n  ]\r\n\r\n}";
            var expects = new Expect[] { 
                new Expect(){Token = JsonToken.StartObject, Pos = 0, Line = 1},
                new Expect(){Token = JsonToken.PropertyName, Quote=QuoteType.Double, Pos = 6, Line = 2, Value = "FOO" },
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "1234", Pos = 13, Line = 2},
                new Expect(){Token = JsonToken.PropertyName, Quote=QuoteType.Double, Pos = 18, Line = 2, Value = "BAR" },
                new Expect(){Token = JsonToken.StartArray, Pos = 25, Line = 2},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "1.0", Pos = 35, Line = 3},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Single, Value = "ABCD", Pos = 41, Line = 3},
                new Expect(){Token = JsonToken.EndArray, Pos = 51, Line = 4},
                new Expect(){Token = JsonToken.EndObject, Pos = 56, Line = 6},
  
            };
            Util.Check(target, expects);
        }
    }
}
