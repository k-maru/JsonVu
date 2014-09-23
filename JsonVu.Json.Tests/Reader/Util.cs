using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json.Tests {
    public static class ReaderUtil {
        public static void Check(string json, ReaderExpect[] expects, Relaxations? relax = null) {
            var pos = 0;
            using (var reader = relax.HasValue ? new JsonReader(json, relax.Value) : new JsonReader(json)) {
                while (reader.Read()) {
                    var expect = expects[pos++];
                    Assert.AreEqual(expect.Value, reader.Value, string.Format("{0}-Value:{1}", pos, expect.AssertMessage));
                    Assert.AreEqual(expect.Quote, reader.Quote, string.Format("{0}-Quote:{1}", pos, expect.AssertMessage));
                    Assert.AreEqual(expect.Type, reader.Type, string.Format("{0}-Type:{1}", pos, expect.AssertMessage));
                    Assert.AreEqual(expect.Token, reader.Token, string.Format("{0}-Token:{1}", pos, expect.AssertMessage));

                    if (expect.Pos.HasValue) {
                        Assert.AreEqual(expect.Pos.Value, reader.Position, string.Format("{0}-Pos:{1}", pos, expect.AssertMessage));
                    }
                    if (expect.Line.HasValue) {
                        Assert.AreEqual(expect.Line.Value, reader.Line, string.Format("{0}-Line:{1}", pos, expect.AssertMessage));
                    }
                }
            }
        }
    }

    public  class ReaderExpect {
        public ReaderExpect() {
            this.Value = null;
            this.Token = JsonToken.Unknown;
            this.Type = ValueType.None;
            this.Quote = QuoteType.None;

            this.Pos = null;
            this.Line = null;

            this.AssertMessage = null;
        }

        public string Value { get; set; }
        public JsonToken Token { get; set; }

        public ValueType Type { get; set; }

        public QuoteType Quote { get; set; }

        public int? Pos { get; set; }

        public int? Line { get; set; }

        public string AssertMessage { get; set; }
    }
}
