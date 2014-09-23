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
            this.NewLineObjectStartBraket = false;
            this.NewLineArrayStartBraket = false;
            this.NewLineObjectEndBraket = true;
            this.NewLineArrayEndBraket = true;
            this.SpaceAfterObjectKeyValueSeparator = true;
            this.SpaceBeforeObjectKeyValueSeparator = false;
            this.SpaceAfterComma = true;
            this.SpaceBeforeComma = false;
            this.SpaceAfterObjectStartBraket = true;
            this.SpaceBeforeObjectEndBraket = true;
            this.SpaceAfterArrayStartBraket = true;
            this.SpaceBeforeArrayEndBraket = true;
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
        /// オブジェクトの開始括弧を新しい行に配置するかどうかを取得または設定します。
        /// </summary>
        public bool NewLineObjectStartBraket { get; set; }
        /// <summary>
        /// オブジェクトの終了括弧を新しい行に配置するかどうかを取得または設定します。
        /// </summary>
        public bool NewLineObjectEndBraket { get; set; }
        /// <summary>
        /// オブジェクトの開始括弧の後ろにスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceAfterObjectStartBraket { get; set; }
        /// <summary>
        /// オブジェクトの終了括弧の前にスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceBeforeObjectEndBraket { get; set; }
        /// <summary>
        /// 配列の開始括弧を新しい行に配置するかどうかを取得または設定します。
        /// </summary>
        public bool NewLineArrayStartBraket { get; set; }
        /// <summary>
        /// 配列の終了括弧を新しい行に配置するかどうかを取得または設定します。
        /// </summary>
        public bool NewLineArrayEndBraket { get; set; }
        /// <summary>
        /// 配列の開始括弧の後ろにスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceAfterArrayStartBraket { get; set; }
        /// <summary>
        /// 配列の終了括弧の前にスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceBeforeArrayEndBraket { get; set; }
        /// <summary>
        /// オブジェクトのキーと値を区切るコロンの後ろにスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceAfterObjectKeyValueSeparator { get; set; }
        /// <summary>
        /// オブジェクトのキーと値を区切るコロンの前にスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceBeforeObjectKeyValueSeparator { get; set; }

        /// <summary>
        /// カンマの後ろにスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceAfterComma { get; set; }
        /// <summary>
        /// カンマの前にスペースを挿入するかどうかを取得または設定します。
        /// </summary>
        public bool SpaceBeforeComma { get; set; }
    }
}
