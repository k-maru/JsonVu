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
            var expects = new ReaderExpect[] { 
                new ReaderExpect(){Token = JsonToken.StartObject, Pos = 0, Line = 1, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote=QuoteType.Double, Pos = 6, Line = 2, Value = "FOO" , IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "1234", Pos = 13, Line = 2, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote=QuoteType.Double, Pos = 18, Line = 2, Value = "BAR", IsStrict = true },
                new ReaderExpect(){Token = JsonToken.StartArray, Pos = 25, Line = 2, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "1.0", Pos = 35, Line = 3, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Single, Value = "ABCD", Pos = 41, Line = 3},
                new ReaderExpect(){Token = JsonToken.EndArray, Pos = 51, Line = 4, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndObject, Pos = 56, Line = 6, IsStrict = true},
  
            };
            ReaderUtil.Check(target, expects);
        }
    }
}
