using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json.Tests {
    public static class Util {
        public static void Check(string json, Expect[] expects) {
            var pos = 0;
            using (var reader = new JsonReader(json)) {
                while (reader.Read()) {
                    var expect = expects[pos++];
                    Assert.AreEqual(expect.Value, reader.Value);
                    Assert.AreEqual(expect.Quote, reader.Quote);
                    Assert.AreEqual(expect.Type, reader.Type);
                    Assert.AreEqual(expect.Token, reader.Token);

                    if (expect.Pos.HasValue) {
                        Assert.AreEqual(expect.Pos.Value, reader.Position);
                    }
                    if (expect.Line.HasValue) {
                        Assert.AreEqual(expect.Line.Value, reader.Line);
                    }
                }
            }
        }
    }

    public  class Expect {
        public Expect() {
            this.Value = null;
            this.Token = JsonToken.Unknown;
            this.Type = ValueType.Unknown;
            this.Quote = QuoteType.None;

            this.Pos = null;
            this.Line = null;
        }

        public string Value { get; set; }
        public JsonToken Token { get; set; }

        public ValueType Type { get; set; }

        public QuoteType Quote { get; set; }

        public int? Pos { get; set; }

        public int? Line { get; set; }
    }
}
