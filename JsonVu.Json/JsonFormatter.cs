using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    public sealed class JsonFormatter {

        private JsonReader jsonReader;
        private JsonFormatSetting setting;
        private int depth = 0;

        public JsonFormatter(string json, JsonFormatSetting setting = null)
            : this(new StringReader(json), setting) {

        }

        public JsonFormatter(TextReader reader, JsonFormatSetting setting = null) {
            if (setting == null) {
                setting = new JsonFormatSetting();
            }
            this.setting = setting;
            this.jsonReader = new JsonReader(reader, setting.Relaxations);
        }

        public string Format() {
            var builder = new StringBuilder();

            while (jsonReader.Read()) {
                if (jsonReader.Token == JsonToken.StartObject) {
                    FormatObject(builder);
                } else if (jsonReader.Token == JsonToken.StartArray) {
                    FormatArray(builder);
                } else {
                    builder.Append(GetValue());
                }
            }

            return builder.ToString();
        }

        private void FormatObject(StringBuilder builder){
            if (jsonReader.Token != JsonToken.StartObject) {
                throw new Exception("");
            }

            var hasKeyValue = false;

            builder.Append("{");
            builder.Append(setting.NewLineString);
            depth++;
            while (jsonReader.Read()) {
                if (jsonReader.Token == JsonToken.EndObject) {
                    if (jsonReader.HasLastComma) {
                        builder.Append(",");
                    }

                    builder.Append(setting.NewLineString);
                    depth--;
                    builder.Append(GetIndentString(depth));
                    builder.Append("}");
                    return;
                }
                if (hasKeyValue) {
                    builder.Append(",");
                    builder.Append(setting.NewLineString);
                }
                //key
                builder.Append(GetIndentString(depth));
                builder.Append(GetValue());
                builder.Append(": ");

                //value
                jsonReader.Read();
                if (jsonReader.Token == JsonToken.StartObject) {
                    FormatObject(builder);
                } else if (jsonReader.Token == JsonToken.StartArray) {
                    FormatArray(builder);
                } else {
                    builder.Append(GetValue());
                }

                hasKeyValue = true;
            }

        }

        private void FormatArray(StringBuilder builder) {
            if (jsonReader.Token != JsonToken.StartArray) {
                throw new Exception("");
            }

            var hasValue = false;

            builder.Append("[");
            builder.Append(setting.NewLineString);
            depth++;
            while (jsonReader.Read()) {
                if (jsonReader.Token == JsonToken.EndArray) {
                    if (jsonReader.HasLastComma) {
                        builder.Append(",");
                    }

                    builder.Append(setting.NewLineString);
                    depth--;
                    builder.Append(GetIndentString(depth));
                    builder.Append("]");
                    return;
                }
                if (hasValue) {
                    builder.Append(",");
                    builder.Append(setting.NewLineString);
                }
                
                if (jsonReader.Token == JsonToken.StartObject) {
                    FormatObject(builder);
                } else if (jsonReader.Token == JsonToken.StartArray) {
                    FormatArray(builder);
                } else {
                    builder.Append(GetIndentString(depth));
                    builder.Append(GetValue());
                }

                hasValue = true;
            }

        }

        private string GetIndentString(int depth) {
            return string.Join("", Enumerable.Repeat(setting.IndentString, depth));
        }

        private string GetValue() {
            return string.Format("{0}{1}{0}", GetQuotaString(), jsonReader.Value);
        }

        private string GetQuotaString() {
            return jsonReader.Quote == QuoteType.Double ? "\"" :
                   jsonReader.Quote ==  QuoteType.Single ? "'" : "";
        }
    }
}
