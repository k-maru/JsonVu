using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonVu.Json.Tests {
    /// <summary>
    /// StrictTest の概要の説明
    /// </summary>
    [TestClass]
    public class StrictTest {

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowSingleQuoteString")]
        public void 単一引用符の文字列はだめ() {
            var target = "'abcde'";
            var expects = new ReaderExpect[] { 
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowSingleQuoteString);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowNonStringKey")]
        public void 文字列以外のプロパティ名は駄目() {
            var target = "{12345:'HOGE'}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowNonStringKeyName);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowHexNumber")]
        public void 数値の16進数は駄目1() {
            var target = "0xFF";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowHexNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowHexNumber")]
        public void 数値の16進数は駄目2() {
            var target = "{0x1F:'HOGE'}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowHexNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowHexNumber")]
        public void 数値の16進数は駄目3() {
            var target = "{Foo:0x254}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Unknown, Value = "Foo"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowHexNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowOctalNumber")]
        public void 数値の8進数は駄目1() {
            var target = "019";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.Value,  Type=ValueType.Number, Value = "019"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowOctalNumber);

            target = "012";
            expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowOctalNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowOctalNumber")]
        public void 数値の8進数は駄目2() {
            var target = "{013:'HOGE'}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowOctalNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowOctalNumber")]
        public void 数値の8進数は駄目3() {
            var target = "{Foo:016}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Unknown, Value = "Foo"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowOctalNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLeftZeroPaddingNumber")]
        public void 数値の0左埋めは駄目1() {
            var target = "012";  //8進数だからOK
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.Value,  Type=ValueType.Number, Value = "012"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLeftZeroPaddingNumber);

            target = "0019";
            expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLeftZeroPaddingNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLeftZeroPaddingNumber")]
        public void 数値の0左埋めは駄目1_1() {
            var target = "0000";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLeftZeroPaddingNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLeftZeroPaddingNumber")]
        public void 数値の0左埋めは駄目2() {
            var target = "{009:'HOGE'}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLeftZeroPaddingNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLeftZeroPaddingNumber")]
        public void 数値の0左埋めは駄目3() {
            var target = "{Foo:009}";
            var expects = new ReaderExpect[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Unknown, Value = "Foo"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLeftZeroPaddingNumber);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowInfinity")]
        public void Infinityは駄目1() {
            var target = "Infinity";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowInfinity);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowInfinity")]
        public void Infinityは駄目2() {
            var target = "-Infinity";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowInfinity);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowUndefined")]
        public void undefinedは駄目() {
            var target = "undefined";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowUndefined);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowNaN")]
        public void NaNは駄目() {
            var target = "NaN";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowNaN);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowUnknownType")]
        public void 不明な型の値は駄目() {
            var target = "+0iajoljc";
            var expects = new ReaderExpect[] {
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowUnknownType);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLastComma")]
        public void 配列の末尾カンマは駄目() {
            var target = @"[""ABCD"", 123, ] ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ABCD"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"}
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLastComma);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "DisallowLastComma")]
        public void オブジェクトの末尾カンマは駄目() {
            var target = @"{""ABC"": 123, ""DEF"" : 456, } ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ABC"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "DEF"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "456"},
            };
            ReaderUtil.Check(target, expects, Relaxations.AllowAll ^ Relaxations.AllowLastComma);
        }
    }
}
