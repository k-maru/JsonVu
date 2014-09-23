using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json {
    /// <summary>
    /// JSONで利用可能な緩和内容を表します。
    /// </summary>
    [Flags]
    public enum Relaxations {
        /// <summary>
        /// 許可しない
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 単一引用符の文字列を許可する
        /// </summary>
        AllowSingleQuoteString = 0x001,
        /// <summary>
        /// 文字列でないキー名を許可する
        /// </summary>
        AllowNonStringKeyName = 0x002,
        /// <summary>
        /// 16進数を許可する
        /// </summary>
        AllowHexNumber = 0x004,
        /// <summary>
        /// 8進数を許可する
        /// </summary>
        AllowOctalNumber = 0x008,
        /// <summary>
        /// 先頭0埋めの数値を許可する
        /// </summary>
        AllowLeftZeroPaddingNumber = 0x010,
        /// <summary>
        /// Infinityを許可する
        /// </summary>
        AllowInfinity = 0x020,
        /// <summary>
        /// undefinedを許可する
        /// </summary>
        AllowUndefined = 0x040,
        /// <summary>
        /// NaNを許可する
        /// </summary>
        AllowNaN = 0x080,
        /// <summary>
        /// 不明な型の値を許可する
        /// </summary>
        AllowUnknownType = 0x100,

        AllowLastComma = 0x200,
        /// <summary>
        /// すべて許可する
        /// </summary>
        AllowAll = AllowSingleQuoteString | 
                   AllowNonStringKeyName | 
                   AllowHexNumber |
                   AllowOctalNumber |
                   AllowLeftZeroPaddingNumber |
                   AllowInfinity |
                   AllowUndefined |
                   AllowNaN |
                   AllowUnknownType |
                   AllowLastComma
    }
}
