using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JsonVu.Json.Tests {

    [TestClass]
    public class SingleValueTest {

        [TestMethod]
        public void 二重引用符での文字列の読み込み() {
            var target = @"""ABCDE""";
            var expects = new[] {
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "ABCDE"}  
            };
            Util.Check(target, expects);

            target = @"""AB'CDE""";
            expects = new[] {
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "AB'CDE"}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 単一引用符での文字列の読み込み() {
            var target = @"'ABCDE'";
            var expects = new[] {
                new Expect(){Quote = QuoteType.Single, Token = JsonToken.Value, Type = ValueType.String, Value = "ABCDE"}  
            };
            Util.Check(target, expects);

            target = @"'AB""CDE'";
            expects = new[] {
                new Expect(){Quote = QuoteType.Single, Token = JsonToken.Value, Type = ValueType.String, Value = "AB\"CDE"}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 整数の読み込み() {
            var target = @"12345";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);

            target = @"009";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };

            Util.Check(target, expects);
        }

        [TestMethod]
        public void ゼロのみが連続しても整数() {
            var target = @"00000";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 小数の読み込み() {
            var target = @"12345.1234";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);

            target = @"0.1234";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };

            target = @".1234";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };

            Util.Check(target, expects);
        }

        [TestMethod]
        public void 小数でも先頭に0が複数続く場合は数値ではない() {
            var target = @"00.00";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.String, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void 整数の指数付の読み込み() {
            var target = @"12345e-10";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);

            target = @"12345E-10";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);

            target = @"12345e+10";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);

            target = @"12345e-10";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void ゼロの指数は可能() {
            var target = @"0e-0";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Number, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void ゼロが連続する指数は不可() {
            var target = @"00e-0";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.String, Value = target}  
            };
            Util.Check(target, expects);
        }

        //
        [TestMethod]
        public void nullの読み込み() {
            var target = @"null";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Null, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void booleanの読み込み() {
            var target = @"true";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Boolean, Value = target}  
            };
            Util.Check(target, expects);

            target = @"false";
            expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Boolean, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void NaNの読み込み() {
            var target = @"NaN";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.NaN, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void Infinityの読み込み() {
            var target = @"Infinity";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Infinitiy, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void undefinedの読み込み() {
            var target = @"undefined";
            var expects = new[] {
                new Expect(){Quote = QuoteType.None, Token = JsonToken.Value, Type = ValueType.Undefined, Value = target}  
            };
            Util.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorIncorrectValue")]
        public void 文字列の後ろにその他の値がある場合は例外() {
            var target = @"""abcde""abde";
            var expects = new Expect[]{};
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorIncorrectValue")]
        public void 文字列の後ろにその他の値がある場合は例外2() {
            var target = @"'abcde'abde";
            var expects = new Expect[]{};
            Util.Check(target, expects);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotCompletedStringValue")]
        public void 文字列中に改行がある場合は例外() {
            var target = "\"abc\rabc\"";
            var expects = new Expect[] { };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotCompletedStringValue")]
        public void 文字列中に改行がある場合は例外2() {
            var target = "\"abc\nabc\"";
            var expects = new Expect[] { };
            Util.Check(target, expects);
        }
        [TestMethod, ExpectedExceptionWithMessage(typeof(JsonReaderException), typeof(Properties.Resources), "ErrorNotCompletedStringValue")]
        public void 文字列中に改行がある場合は例外3() {
            var target = "\"abc\r\nabc\"";
            var expects = new Expect[] { };
            Util.Check(target, expects);
        }

        [TestMethod]
        public void エスケープされている場合はOK() {
            var target = "\"abc\\\rabc\"";
            var expects = new Expect[] { 
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "abc\\\rabc"}  
            };
            Util.Check(target, expects);

            target = "\"abc\\\nabc\"";
            expects = new Expect[] { 
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "abc\\\nabc"}  
            };
            Util.Check(target, expects);

            target = "\"abc\\\r\\\nabc\"";
            expects = new Expect[] { 
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "abc\\\r\\\nabc"}  
            };
            Util.Check(target, expects);

            target = "\"abc\\\"abc\"";
            expects = new Expect[] { 
                new Expect(){Quote = QuoteType.Double, Token = JsonToken.Value, Type = ValueType.String, Value = "abc\\\"abc"}  
            };
            Util.Check(target, expects);
        }
    }
}
