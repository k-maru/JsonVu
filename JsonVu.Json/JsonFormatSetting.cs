using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    public sealed class JsonFormatSetting {

        public JsonFormatSetting(){
            this.Relaxations = Relaxations.None;
            this.IndentString = "    ";
            this.NewLineString = "\r\n";
        }
        /// <summary>
        /// JSONで利用可能な緩和内容を取得または設定します。
        /// </summary>
        public Relaxations Relaxations { get; set; }
        /// <summary>
        /// インデントで利用する文字列を取得または設定します。
        /// </summary>
        public string IndentString { get; set; }
        /// <summary>
        /// 改行で利用する文字列を取得または設定します。
        /// </summary>
        public string NewLineString { get; set; }
    }
}
