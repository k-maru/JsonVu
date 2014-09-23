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
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.EndObject},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void キーバリュー一つだけ() {
            var target = @"{""aaa"": ""bbb""}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName, Type = ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new Expect(){Token = JsonToken.EndObject},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 複数のキーバリュー() {
            var target = @"{""aaa"": ""bbb"", ""ccc"": 123}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ccc"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new Expect(){Token = JsonToken.EndObject},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void バリューにオブジェクト() {
            var target = @"{ ""aaa"": {""bbb"": ""ccc""}, ""ddd"": {""eee"": 10 } } ";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName, Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "bbb"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ccc"},
                new Expect(){Token = JsonToken.EndObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ddd"},
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "eee"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new Expect(){Token = JsonToken.EndObject},
                new Expect(){Token = JsonToken.EndObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod]
        public void バリューに配列() {
            var target = @"{""aaa"":[""bbb"",""ccc""],""ddd"":""eee"",""fff"":{""ggg"": 10}}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "aaa"},
                new Expect(){Token = JsonToken.StartArray},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "ccc"},
                new Expect(){Token = JsonToken.EndArray},
                new Expect(){Token = JsonToken.PropertyName, Type=ValueType.String,  Quote = QuoteType.Double, Value= "ddd"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "eee"},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "fff"},
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ggg"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new Expect(){Token = JsonToken.EndObject},
                new Expect(){Token = JsonToken.EndObject}
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void キーに文字以外_正しくは値としてみなせるもの() {
            var target = @"{aaa:""bbb"",123:345,0.23e-5:10,true:false}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName, Type = ValueType.Unknown, Value= "aaa"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.String, Quote = QuoteType.Double, Value = "bbb"},
                new Expect(){Token = JsonToken.PropertyName, Type = ValueType.Number, Value= "123"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "345"},
                new Expect(){Token = JsonToken.PropertyName, Type = ValueType.Number, Value= "0.23e-5"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "10"},
                new Expect(){Token = JsonToken.PropertyName, Type = ValueType.Boolean, Value= "true"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Boolean, Value = "false"},
                new Expect(){Token = JsonToken.EndObject}
            };
            Util.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidObjectKey")]
        public void キーにオブジェクトはエラー() {
            var target = @"{{1:1}:""bbb""}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidObjectKey")]
        public void キーに配列はエラー() {
            var target = @"{[1,1]:""bbb""}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetPropertyKey")]
        public void キーがないはエラー() {
            var target = @"{:""bbb""}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorInvalidCharacter")]
        public void 値とプロパティの区切りの間違いもエラー() {
            var target = @"{ccc,""bbb""}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetPropertyValue")]
        public void 値とプロパティの区切りの間違いもエラー_プロパティで終端の場合() {
            var target = @"{ccc";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
            };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotSetPropertyValue")]
        public void プロパティだけで終わってる場合() {
            var target = @"{ccc:}";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.Unknown, Value= "ccc"},
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void オブジェクトの末尾カンマ() {
            var target = @"{""ABC"": 123, ""DEF"" : 456 , } ";
            var expects = new[] {
                new Expect(){Token = JsonToken.StartObject},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "ABC"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "123"},
                new Expect(){Token = JsonToken.PropertyName,  Type=ValueType.String, Quote = QuoteType.Double, Value= "DEF"},
                new Expect(){Token = JsonToken.Value, Type = ValueType.Number, Value = "456"},
                new Expect(){Token = JsonToken.EndObject}
            };
            Util.Check(target, expects);
        }
    }
}
