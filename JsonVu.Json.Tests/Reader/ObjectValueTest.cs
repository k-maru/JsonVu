using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json.Tests {
    [TestClass]
    public class ObjectValueTest {
        [TestMethod]
        public void 空のオブジェクト() {
            var target = @"{}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.EndObject},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void キーバリュー一つだけ() {
            var target = @"{""aaa"": ""bbb""}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key, Type = ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new ReaderExpect(){Token = JsonToken.EndObject},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void 複数のキーバリュー() {
            var target = @"{""aaa"": ""bbb"", ""ccc"": 123}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ccc"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.EndObject},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void バリューにオブジェクト() {
            var target = @"{ ""aaa"": {""bbb"": ""ccc""}, ""ddd"": {""eee"": 10 } } ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key, Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "bbb"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ccc"},
                new ReaderExpect(){Token = JsonToken.EndObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ddd"},
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "eee"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new ReaderExpect(){Token = JsonToken.EndObject},
                new ReaderExpect(){Token = JsonToken.EndObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod]
        public void バリューに配列() {
            var target = @"{""aaa"":[""bbb"",""ccc""],""ddd"":""eee"",""fff"":{""ggg"": 10}}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new ReaderExpect(){Token = JsonToken.StartArray},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ccc"},
                new ReaderExpect(){Token = JsonToken.EndArray},
                new ReaderExpect(){Token = JsonToken.Key, Type=ValueType.String,  Quote = QuoteType.Double, Value= "ddd"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "eee"},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "fff"},
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ggg"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new ReaderExpect(){Token = JsonToken.EndObject},
                new ReaderExpect(){Token = JsonToken.EndObject}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void キーに文字以外_正しくは値としてみなせるもの() {
            var target = @"{aaa:""bbb"",123:345,0.23e-5:10,true:false}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key, Type = ValueType.Unknown, Value= "aaa"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new ReaderExpect(){Token = JsonToken.Key, Type = ValueType.Number, Value= "123"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "345"},
                new ReaderExpect(){Token = JsonToken.Key, Type = ValueType.Number, Value= "0.23e-5"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new ReaderExpect(){Token = JsonToken.Key, Type = ValueType.Boolean, Value= "true"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "false"},
                new ReaderExpect(){Token = JsonToken.EndObject}
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidObjectKey")]
        public void キーにオブジェクトはエラー() {
            var target = @"{{1:1}:""bbb""}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidObjectKey")]
        public void キーに配列はエラー() {
            var target = @"{[1,1]:""bbb""}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetObjectKey")]
        public void キーがないはエラー() {
            var target = @"{:""bbb""}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidCharacter")]
        public void 値とプロパティの区切りの間違いもエラー() {
            var target = @"{ccc,""bbb""}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetObjectValue")]
        public void 値とプロパティの区切りの間違いもエラー_プロパティで終端の場合() {
            var target = @"{ccc";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
            };
            ReaderUtil.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetObjectValue")]
        public void プロパティだけで終わってる場合() {
            var target = @"{ccc:}";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.Unknown, Value= "ccc"},
            };
            ReaderUtil.Check(target, expects);
        }

        [TestMethod]
        public void オブジェクトの末尾カンマ() {
            var target = @"{""ABC"": 123, ""DEF"" : 456 , } ";
            var expects = new[] {
                new ReaderExpect(){Token = JsonToken.StartObject},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ABC"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new ReaderExpect(){Token = JsonToken.Key,  Type=ValueType.String, Quote = QuoteType.Double, Value= "DEF"},
                new ReaderExpect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "456"},
                new ReaderExpect(){Token = JsonToken.EndObject}
            };
            ReaderUtil.Check(target, expects);
        }
    }
}
