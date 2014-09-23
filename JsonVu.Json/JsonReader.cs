﻿using JsonVu.Json.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonVu.Json {

    public sealed class JsonReader : IDisposable {

        private enum State {
            Start,
            InObject,
            InArray,
            Finished
        }

        private sealed class StateModel {
            private StateModel(State state) {
                this.State = state;
                this.IsValue = false;
                this.BeforeComma = false;
            }

            public State State { get; private set; }

            public bool IsValue { get; set; }
            
            public bool BeforeComma { get; set; }

            public static readonly StateModel Start = new StateModel(State.Start);

            public static readonly StateModel Finished = new StateModel(State.Finished);
            public static StateModel InObject() {
                return new StateModel(State.InObject);
            }

            public static StateModel InArray() {
                return new StateModel(State.InArray);
            }
        }

        private TextReader reader;
        private Stack<StateModel> stateStack = new Stack<StateModel>();
        private Regex unquotedTailRegex = new Regex(@"[\""\'\r\n\,\ \}\]\:]");
        private Regex jsonValidNumExpRegex = new Regex(@"^((0|[1-9][0-9]*)(\.[0-9]*)?|(\.[0-9]+))([eE][-+]?[0-9]+)?$");
        private Regex hexNumRegex = new Regex(@"^0x[0-9a-fA-F]+$");
        private Regex octNumRegex = new Regex(@"^0[0-7]+$");

        private int readPos = -1;
        private int readLine = 1;

        private Relaxations relax = Relaxations.AllowAll;

        public JsonReader(string target, Relaxations relax = Relaxations.AllowAll)
            : this(new StringReader(target), relax) {
        }

        public JsonReader(TextReader reader, Relaxations relax = Relaxations.AllowAll) {
            this.Position = -1;
            this.Line = -1;
            this.reader = reader;
            this.relax = relax;

            this.stateStack.Push(StateModel.Start);
        }

        public bool Read() {
            this.Value = null;
            this.Token = JsonToken.Unknown;
            this.Type = ValueType.None;
            this.Quote = QuoteType.None;

            Skip();
            if (reader.Peek() <= 0) {
                return false;
            }

            var state = stateStack.Peek();

            if (state.State == State.InObject) {
                return ReadObject(state);
            }
            if (state.State == State.InArray) {
                return ReadArray(state);
            }
            var result = ReadValue(state);
            //オブジェクトもしくは配列の中でない場合に、値を読み取った後で後続する何かがあった場合はエラー
            if (stateStack.Peek().State == State.Start && Skip()) {
                throw CreateException(Resources.ErrorIncorrectValue);
            }

            return result;
        }

        private bool ReadValue(StateModel state) {

            var ch = Next();

            this.Position = readPos;
            this.Line = readLine;

            var result = false;
            if (ch == '{') {
                PushState(StateModel.InObject());
                this.Token = JsonToken.StartObject;
                result = true;
            } else if (ch == '[') {
                PushState(StateModel.InArray());
                this.Token = JsonToken.StartArray;
                result = true;
            } else if (ch == '"' || ch == '\'') {
                //単一引用符の文字列は許可しない
                if (ch == '\'' && (relax & Relaxations.AllowSingleQuoteString) != Relaxations.AllowSingleQuoteString) {
                    throw CreateException(Resources.DisallowSingleQuoteString);
                }
                this.Quote = ch == '"' ? QuoteType.Double : QuoteType.Single;
                result = ReadString(state);
            } else {
                result = ReadUnquoted(state, ch);
            }

            return result;
        }

        private bool ReadObject(StateModel state) {
            var ch = reader.Peek();
            var result = false;

            this.Position = readPos + 1; //peek のため
            this.Line = readLine;
            
            if (state.State == State.InObject) {
                if (!state.IsValue) {
                    if (ch == '}') {
                        if (state.BeforeComma && (relax & Relaxations.AllowLastComma) != Relaxations.AllowLastComma) {
                            throw CreateException(Resources.DisallowLastComma);
                        }
                        state.BeforeComma = false;
                        Next();
                        PopState();
                        PrepareTail(state);
                        this.Token = JsonToken.EndObject;
                        result = true;
                    } else {
                        state.BeforeComma = false;
                        result = ReadValue(state);
                        //文字列以外のプロパティは許可しない
                        if (this.Type != ValueType.String &&
                            (relax & Relaxations.AllowNonStringPropertyName) != Relaxations.AllowNonStringPropertyName) {
                            throw CreateException(Resources.DisallowNonStringProperty);
                        }

                        //値以外はプロパティのキーにできない
                        if (this.Token != JsonToken.Value) {
                            throw CreateException(Resources.ErrorInvalidObjectKey);
                        }

                        PrepareTail(state);
                        this.Token = JsonToken.PropertyName;
                        state.IsValue = true;
                    }
                } else {
                    state.BeforeComma = false;
                    result = ReadValue(state);
                    if (this.Token == JsonToken.Value) {
                        PrepareTail(state);
                        state.IsValue = false;
                    }
                }
            }

            return result;
        }

        private bool ReadArray(StateModel state) {
            var ch = reader.Peek();
            var result = false;

            this.Position = readPos + 1; //peek のため
            this.Line = readLine;

            if (state.State == State.InArray) {
                if (ch == ']') {
                    if (state.BeforeComma && (relax & Relaxations.AllowLastComma) != Relaxations.AllowLastComma) {
                        throw CreateException(Resources.DisallowLastComma);
                    }
                    state.BeforeComma = false;
                    Next();
                    PopState();
                    PrepareTail(state);
                    this.Token = JsonToken.EndArray;
                    result = true;
                } else {
                    state.BeforeComma = false;
                    result = ReadValue(state);
                    if (this.Token == JsonToken.Value) {
                        PrepareTail(state);
                    }
                }
            }
            return result;
        }

        private void PrepareTail(StateModel beforeState) {
            var state = stateStack.Peek();
            if (state.State == State.InArray) {
                if (!Skip()) {
                    throw new Exception("配列が完了していません。");
                }
                var ch = reader.Peek();
                if (ch != ',' && ch != ']') {
                    throw new Exception("配列が完了していません。");
                }
                if (ch == ',') {
                    Next();
                    state.BeforeComma = true;
                }
                return;
            }
            if (state.State == State.InObject) {

                if (state.IsValue) {
                    if (!Skip()) {
                        throw new Exception("オブジェクトが完了していません。");
                    }
                    var ch = reader.Peek();
                    if (ch != ',' && ch != '}') {
                        throw new Exception("オブジェクトが完了していません。");
                    }
                    if (ch == ',') {
                        Next();
                        state.BeforeComma = true;
                    }
                } else {
                    if (!Skip()) {
                        throw CreateException(Resources.ErrorNotSetPropertyValue);
                    }
                    if (Next() != ':') {
                        throw CreateException(Resources.ErrorInvalidCharacter);
                    }
                }
                if (state != beforeState && (beforeState.State == State.InObject || beforeState.State == State.InArray)) {
                    state.IsValue = false;
                }
                return;
            }
        }

        private bool ReadString(StateModel state) {
            var isEscaped = false;
            var builder = new StringBuilder();

            while (reader.Peek() > 0) {
                var ch = Next();
                if (isEscaped) {
                    isEscaped = false;
                    builder.Append(ch);
                } else if (ch == (this.Quote == QuoteType.Double ? '"' : '\'')) {
                    this.Value = builder.ToString();
                    this.Type = ValueType.String;
                    this.Token = JsonToken.Value;
                    return true;
                } else if (ch == '\\') {
                    isEscaped = true;
                    builder.Append(ch);
                } else if (ch == '\r' || ch == '\n') {
                    throw CreateException(Resources.ErrorNotCompletedStringValue);
                } else {
                    builder.Append(ch);
                }
            }
            throw CreateException(Resources.ErrorNotCompletedStringValue);
        }

        private bool ReadUnquoted(StateModel state, char first) {
            
            if (unquotedTailRegex.IsMatch(first.ToString())) {
                if (state.State == State.InObject && state.IsValue) {
                    throw CreateException(Resources.ErrorNotSetPropertyValue);
                }
                if (state.State == State.InObject && !state.IsValue && first == ':') {
                    throw CreateException(Resources.ErrorNotSetPropertyKey);
                }
                throw CreateException(Resources.ErrorInvalidCharacter);
            }
            var builder = new StringBuilder();

            builder.Append(first);
            while (reader.Peek() > 0) {
                var ch = (char)reader.Peek();
                if (unquotedTailRegex.IsMatch(ch.ToString())) {
                    break;
                }
                builder.Append(Next());
            }
            this.Value = builder.ToString();
            this.Type = JudgeUnquotedValue(this.Value);
            this.Token = JsonToken.Value;
            return true;
        }

        private ValueType JudgeUnquotedValue(string value) {
            if (value == "true" || value == "false") {
                return ValueType.Boolean;
            }
            if (value == "null") {
                return ValueType.Null;
            }
            if (value == "undefined") {
                if ((relax & Relaxations.AllowUndefined) != Relaxations.AllowUndefined) {
                    throw CreateException(Resources.DisallowUndefined);
                }
                return ValueType.Undefined;
            }
            if (value == "NaN") {
                if ((relax & Relaxations.AllowNaN) != Relaxations.AllowNaN) {
                    throw CreateException(Resources.DisallowNaN);
                }
                return ValueType.NaN;
            }
            if (value == "Infinity" || value == "-Infinity") {
                if ((relax & Relaxations.AllowInfinity) != Relaxations.AllowInfinity) {
                    throw CreateException(Resources.DisallowInfinity);
                }
                return ValueType.Infinitiy;
            }
            if (IsNumeric(value)) {
                return ValueType.Number;
            }
            if ((relax & Relaxations.AllowUnknownType) != Relaxations.AllowUnknownType) {
                throw CreateException(Resources.DisallowUnknownType);
            }
            return ValueType.Unknown;
        }

        private bool IsNumeric(string value) {
            if (value[0] == '-') {
                value = value.Substring(1);
            }
            //JSON valid
            if (jsonValidNumExpRegex.IsMatch(value)) {
                return true;
            }
            //16進
            if (hexNumRegex.IsMatch(value)) {
                if ((relax & Relaxations.AllowHexNumber) != Relaxations.AllowHexNumber) {
                    throw CreateException(Resources.DisallowHexNumber);
                }
                return true;
            }
            //全部0
            if (value.Trim('0').Length == 0) {
                if ((relax & Relaxations.AllowLeftZeroPaddingNumber) != Relaxations.AllowLeftZeroPaddingNumber) {
                    throw CreateException(Resources.DisallowLeftZeroPaddingNumber);
                }
                return true;
            }
            //8進
            if (octNumRegex.IsMatch(value)) {
                if ((relax & Relaxations.AllowOctalNumber) != Relaxations.AllowOctalNumber) {
                    throw CreateException(Resources.DisallowOctalNumber);
                }
                return true;
            }
            //先頭0列挙の数値
            var trimZeroValue = value.TrimStart('0');
            if (trimZeroValue[0] >= 49 && trimZeroValue[0] <= 57) {
                if (jsonValidNumExpRegex.IsMatch(trimZeroValue)) {
                    if ((relax & Relaxations.AllowLeftZeroPaddingNumber) != Relaxations.AllowLeftZeroPaddingNumber) {
                        throw CreateException(Resources.DisallowLeftZeroPaddingNumber);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool Skip() {
            while (true) {
                var ch = reader.Peek();
                if (ch <= 0) {
                    return false;
                }
                if (ch > ' ') {
                    return true;
                }

                ch = Next();
                if (ch == '\r') {
                    if ((char)reader.Peek() == '\n') {
                        Next();
                    }
                    readLine++;
                } else if (ch == '\n') {
                    readLine++;
                }
            }
        }

        private char Next() {
            var ch = (char)reader.Read();
            readPos++;
            return ch;
        }

        private void PushState(StateModel newState) {
            stateStack.Push(newState);
        }

        private void PopState() {
            stateStack.Pop();
        }

        private JsonReaderException CreateException(string message) {
            this.Token = JsonToken.Unknown;
            this.Type = ValueType.None;
            this.Quote = QuoteType.None;

            return new JsonReaderException(message, readPos, readLine);
        }

        public string Value { get; private set; }

        public JsonToken Token { get; private set; }

        public ValueType Type { get; private set; }

        public QuoteType Quote { get; private set; }

        public int Position { get; private set; }

        public int Line { get; private set; }

        void IDisposable.Dispose() {
            if (reader != null) {
                try {
                    reader.Dispose();
                } catch { }
            }
        }
    }
}
