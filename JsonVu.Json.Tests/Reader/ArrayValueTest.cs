using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonVu.Json.Tests {
    [TestClass]
    public class ArrayValueTest {
        [TestMethod]
        public void 空の配列() {
            var target = @"[]";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 値が入った配列() {
            var target = @"[""ABCD"", 123, 0.0, true,0x123,  null ] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123", IsHexNumber = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 配列が入った配列() {
            var target = @"[""ABCD"", [123, 0.0], true,[0x123,  null]] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "true", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123", IsHexNumber = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 値にオブジェクト() {
            var target = @"[""ABCD"", {123: 0.0, true:[0x123,  null]}] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.StartObject, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0.0", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Boolean, Value = "true"},
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "0x123", IsHexNumber = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Null, Value = "null", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndObject, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, IsStrict = true}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 配列の末尾カンマ() {
            var target = @"[""ABCD"", 123 ,  ] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123", IsStrict = true},
                new ReaderExpect(){Token = JsonToken.EndArray, HasLastComma = true}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorArrayNotCompleted")]
        public void 完了していない配列1() {
            var target = @"[""ABC"", 123 123]";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type=ValueType.String, Quote = QuoteType.Double, Value= "ABC",IsStrict=true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123", IsStrict=true},
            };

            ReaderUtil.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorArrayNotCompleted")]
        public void 完了していないオブジェクト2() {
            var target = @"[""ABC"", 123";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray, IsStrict = true},
                new ReaderExpect(){Token = JsonToken.Value, Type=ValueType.String, Quote = QuoteType.Double, Value= "ABC",IsStrict=true},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123", IsStrict=true},
            };

            ReaderUtil.Check(target, expects);
        }      
    }
}
