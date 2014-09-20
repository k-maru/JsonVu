using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonVu.Json.Tests {
    [TestClass]
    public class ArrayValueTest {
        [TestMethod]
        public void 空の配列() {
            var target = @"[]";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.EndArray},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 値が入った配列() {
            var target = @"[""ABCD"", 123, 0.0, true,0x123,  null ] ";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new Expect(){Token = JsonToken.EndArray},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 配列が入った配列() {
            var target = @"[""ABCD"", [123, 0.0], true,[0x123,  null]] ";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new Expect(){Token = JsonToken.EndArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true"},
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new Expect(){Token = JsonToken.EndArray},
                new Expect(){Token = JsonToken.EndArray}
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 値にオブジェクト() {
            var target = @"[""ABCD"", {123: 0.0, true:[0x123,  null]}] ";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName, Value = "123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new Expect(){Token = JsonToken.PropertyName, Value = "true"},
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new Expect(){Token = JsonToken.EndArray},
                new Expect(){Token = JsonToken.EndObject},
                new Expect(){Token = JsonToken.EndArray}
            };
            Util.Check(target, expects);
        }
    }
}
