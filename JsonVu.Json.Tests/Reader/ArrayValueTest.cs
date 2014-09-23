using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonVu.Json.Tests {
    [TestClass]
    public class ArrayValueTest {
        [TestMethod]
        public void 空の配列() {
            var target = @"[]";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.EndArray},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 値が入った配列() {
            var target = @"[""ABCD"", 123, 0.0, true,0x123,  null ] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new ReaderExpect(){Token = JsonToken.EndArray},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 配列が入った配列() {
            var target = @"[""ABCD"", [123, 0.0], true,[0x123,  null]] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new ReaderExpect(){Token = JsonToken.EndArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true"},
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new ReaderExpect(){Token = JsonToken.EndArray},
                new ReaderExpect(){Token = JsonToken.EndArray}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 値にオブジェクト() {
            var target = @"[""ABCD"", {123: 0.0, true:[0x123,  null]}] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0"},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Boolean, Value = "true"},
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null"},
                new ReaderExpect(){Token = JsonToken.EndArray},
                new ReaderExpect(){Token = JsonToken.EndObject},
                new ReaderExpect(){Token = JsonToken.EndArray}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 配列の末尾カンマ() {
            var target = @"[""ABCD"", 123 ,  ] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.EndArray}
            };
            ReaderUtil.Check(target, expects);
        }
    }
}
