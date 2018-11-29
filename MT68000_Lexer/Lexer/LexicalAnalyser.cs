using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class <see cref="LexicalAnalyser"/>.
    /// </summary>
    public class LexicalAnalyser
    {
        /// <summary>
        ///     <see cref="Enum"/> Lexemes
        /// </summary>
        public enum Lexemes
        {
            /// <summary>
            ///     The none
            /// </summary>
            None,

            /// <summary>
            ///     The instruction
            /// </summary>
            Instruction,

            /// <summary>
            ///     The number
            /// </summary>
            Number,

            /// <summary>
            ///     The decimal
            /// </summary>
            Decimal,

            /// <summary>
            ///     The float
            /// </summary>
            Float,

            /// <summary>
            ///     The single quote string
            /// </summary>
            SingleQuoteString,

            /// <summary>
            ///     The double quote string
            /// </summary>
            DoubleQuoteString,

            /// <summary>
            ///     The square bracket string
            /// </summary>
            SquareBracketString,

            /// <summary>
            ///     The operator
            /// </summary>
            Operator,

            /// <summary>
            ///     The hexadecimal
            /// </summary>
            Hexadecimal,

            /// <summary>
            ///     The octal
            /// </summary>
            Octal,

            /// <summary>
            ///     The binary
            /// </summary>
            Binary,

            /// <summary>
            ///     The boolean
            /// </summary>
            Boolean,

            /// <summary>
            ///     The shift
            /// </summary>
            Shift,

            /// <summary>
            ///     The shift remainder
            /// </summary>
            ShiftRemainder,

            /// <summary>
            ///     The shift assignment
            /// </summary>
            ShiftAssignment,

            /// <summary>
            ///     The operation assignment
            /// </summary>
            OperationAssignment,

            /// <summary>
            ///     The comparison
            /// </summary>
            Comparison,

            /// <summary>
            ///     The equality
            /// </summary>
            Equality,

            /// <summary>
            ///     The logical and
            /// </summary>
            LogicalAnd,

            /// <summary>
            ///     The logical or
            /// </summary>
            LogicalOr,

            /// <summary>
            /// The separator
            /// </summary>
            Separator,

            /// <summary>
            /// The comment
            /// </summary>
            Comment,

            /// <summary>
            /// The label
            /// </summary>
            Label,

            /// <summary>
            /// The variable
            /// </summary>
            Variable,

            /// <summary>
            /// The end of line
            /// </summary>
            EOL,
        }

        /// <summary>
        ///     The current lexeme
        /// </summary>
        private Lexemes _currentLexeme = Lexemes.None;

        /// <summary>
        ///     The next lexeme
        /// </summary>
        private Lexemes _nextLexeme = Lexemes.None;

        /// <summary>
        ///     The sb
        /// </summary>
        private StringBuilder _sb = new StringBuilder();

        /// <summary>
        ///     The tokens
        /// </summary>
        private List<Token> _tokens = new List<Token>();

        /// <summary>
        /// Analyses the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>List</c>&lt;<see cref="Token"/>&gt;.</returns>
        public List<Token> AnalyseFile(string filePath)
        {
            var tokens = new List<Token>();
            Console.WriteLine("Lexical Analysis started...");
            var lines = File.ReadAllLines(filePath);
            foreach(var line in lines)
                tokens.AddRange(AnalyseString(line));
            Console.WriteLine("Lexical Analysis complete");
            return tokens;
        }

        /// <summary>
        /// Analyses the file.
        /// </summary>
        /// <param name="lines">The <paramref name="lines"/>.</param>
        /// <returns><c>List</c>&lt;<see cref="Token"/>&gt;.</returns>
        public List<Token> AnalyseResourceFile(string[] lines)
        {
            var tokens = new List<Token>();
            Console.WriteLine("Lexical Analysis started...");
            foreach(var line in lines)
                tokens.AddRange(AnalyseString($"{line}\n"));
            Console.WriteLine("Lexical Analysis complete");
            return tokens;
        }

        /// <summary>
        ///     Analyses the specified <paramref name="text" />.
        /// </summary>
        /// <param name="text">The <paramref name="text" />.</param>
        /// <returns>List&lt;<see cref="Token" />&gt;.</returns>
        public List<Token> AnalyseString(string text)
        {
            _tokens = new List<Token>();
            TokenizeString(text);
            return _tokens;
        }

        /// <summary>
        ///     Tokenizes the string.
        /// </summary>
        /// <param name="text">The <paramref name="text" />.</param>
        private void TokenizeString(string text)
        {
            if(text.Length > 0)
            {
                GetNextToken(text, 0);
            }
        }

        /// <summary>
        ///     Converts to ken.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="index">The <paramref name="index" />.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void GetNextToken(string str, int index)
        {
            while(true)
            {
                if(index > str.Length - 1)
                    return;

                var currentchar = str[index];
                var nextchar = ' ';

                if((index + 1) < str.Length)
                {
                    nextchar = str[index + 1];
                }

                switch(currentchar)
                {
                    case '\t':
                        index = index + 1;
                        continue;
                    case ' ' when (index + 1) >= str.Length:
                        return;
                    case ' ' when _currentLexeme != Lexemes.Comment &&
                                  _currentLexeme != Lexemes.SingleQuoteString &&
                                  _currentLexeme != Lexemes.SquareBracketString &&
                                  _currentLexeme != Lexemes.DoubleQuoteString &&
                                  _currentLexeme != Lexemes.Instruction:
                        index = index + 1;
                        continue;
                }

                var found = false;
                _sb.Append(currentchar);

                switch(_currentLexeme)
                {
                    case Lexemes.None:
                        if(currentchar == '\n' || currentchar == '\r')
                        {
                            _currentLexeme = Lexemes.EOL;
                            found = true;
                        }
                        else if(Helper.IsSymbol(currentchar))
                        {
                            found = true;
                            if(((currentchar == 'e') || (currentchar == 'E')) &&
                               (Helper.IsNumeric(nextchar) || (nextchar == '^')))
                            {
                                _nextLexeme = Lexemes.Float;
                                found = str.Length < index;
                                if(found)
                                    _nextLexeme = Lexemes.None;
                            }
                            else if((((currentchar == '=') || (currentchar == '!')) && (nextchar == '=')) ||
                                    ((currentchar == '<') && (nextchar == '>')))
                            {
                                _nextLexeme = Lexemes.Equality;
                                found = index == (str.Length - 1);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                            }
                            else if(currentchar == '.')
                            {
                                _nextLexeme = Lexemes.Instruction;
                                found = index == (str.Length - 1);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                            }
                            else
                            {
                                switch(currentchar)
                                {
                                    case ',':
                                        found = true;
                                        break;
                                    case ';':
                                        _nextLexeme = Lexemes.Comment;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '<' when (nextchar == '=') || (nextchar == '<'):
                                    case '>' when (nextchar == '=') || (nextchar == '>'):
                                        _nextLexeme = Lexemes.Comparison;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '&' when nextchar == '&':
                                        _nextLexeme = Lexemes.LogicalAnd;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '|' when nextchar == '|':
                                        _nextLexeme = Lexemes.LogicalOr;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '|' when nextchar == '=':
                                        _nextLexeme = Lexemes.OperationAssignment;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        _currentLexeme = Lexemes.Operator;
                                        break;
                                    case '%' when Helper.IsNumeric(nextchar):
                                        _currentLexeme = Lexemes.Operator;
                                        _nextLexeme = Lexemes.Binary;
                                        break;
                                    case '$' when Helper.IsHexadecimal(nextchar):
                                        _currentLexeme = Lexemes.Operator;
                                        _nextLexeme = Lexemes.Hexadecimal;
                                        break;
                                    case '@' when Helper.IsOctal(nextchar):
                                        _currentLexeme = Lexemes.Operator;
                                        _nextLexeme = Lexemes.Octal;
                                        break;
                                    case '#' when Helper.IsBinary(nextchar):
                                        _currentLexeme = Lexemes.Operator;
                                        _nextLexeme = Lexemes.Decimal;
                                        break;
                                    case '+' when nextchar == '+':
                                    case '-' when nextchar == '-':
                                    case '&':
                                    case '+':
                                    case '*':
                                    case '/':
                                    case '%':
                                    case '^':
                                    case '|':
                                    case '$':
                                    case '@':
                                    case '#':
                                    case '-':
                                        _currentLexeme = Lexemes.Operator;
                                        break;
                                    case '\'':
                                        _nextLexeme = Lexemes.SingleQuoteString;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '"':
                                        _nextLexeme = Lexemes.DoubleQuoteString;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                    case '[':
                                        _nextLexeme = Lexemes.SquareBracketString;
                                        found = index == (str.Length - 1);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                }
                            }
                        }
                        else if(Helper.IsNumeric(currentchar))
                        {
                            if(Helper.IsNumeric(nextchar))
                            {
                                _nextLexeme = Lexemes.Decimal;
                            }
                            else
                            {
                                found = true;
                                _nextLexeme = Lexemes.None;
                            }
                        }
                        else if(Helper.IsAlpha(currentchar))
                        {
                            if(Helper.IsLowerCase(currentchar))
                            {
                                _nextLexeme = Lexemes.Variable;
                            }

                            if(Helper.IsNumeric(nextchar))
                            {
                                _nextLexeme = Lexemes.Variable;
                            }
                            else
                            {
                                switch(nextchar)
                                {
                                    case '.':
                                        _nextLexeme = Lexemes.Instruction;
                                        break;
                                    case '_':
                                        _nextLexeme = Lexemes.Variable;
                                        break;
                                    case ':':
                                        _nextLexeme = Lexemes.Label;
                                        break;
                                    default:
                                        found = !Helper.IsAlpha(nextchar);
                                        if(found)
                                            _nextLexeme = Lexemes.None;
                                        break;
                                }
                            }
                        }

                        break;
                    default:
                        switch(_currentLexeme)
                        {
                            case Lexemes.Instruction:
                                found = !Helper.IsAlphanumeric(nextchar) || (nextchar == ' ') || (currentchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Variable when nextchar == ':':
                                _nextLexeme = Lexemes.Label;
                                found = !(Helper.IsAlphanumeric(nextchar) || (nextchar == ':'));
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Variable:
                                found = !(Helper.IsAlphanumeric(nextchar) || (nextchar == ':'));
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Equality:
                                found = (currentchar == '=') || (currentchar == '>') || nextchar == ',';
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.SingleQuoteString:
                                found = currentchar == '\'';
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.DoubleQuoteString:
                                found = currentchar == '"';
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.SquareBracketString:
                                found = currentchar == ']';
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Decimal:
                                found = !Helper.IsNumeric(nextchar);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Comparison when ((currentchar == '<') && (nextchar == '<')) ||
                                                         ((currentchar == '>') && (nextchar == '>')):
                                _nextLexeme = Lexemes.ShiftRemainder;
                                found = (currentchar == '<') || (currentchar == '=') || (currentchar == '>') ||
                                        (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Comparison
                                when ((currentchar == '<') || (currentchar == '>')) && (nextchar == '='):
                                _nextLexeme = Lexemes.ShiftAssignment;
                                found = (currentchar == '<') || (currentchar == '=') || (currentchar == '>') ||
                                        (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Comparison:
                                found = (currentchar == '<') || (currentchar == '=') || (currentchar == '>') ||
                                        (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Shift:
                                found = ((currentchar == '<') && (nextchar == '<')) ||
                                        ((currentchar == '>') && (nextchar == '>')) || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.ShiftRemainder:
                                found = (currentchar == '<') || (currentchar == '>') || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Operator:
                                found = true;
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.OperationAssignment:
                                found = (currentchar == '=') || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.ShiftAssignment:
                                found = (currentchar == '=') || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.LogicalAnd:
                                found = (currentchar == '&') || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.LogicalOr:
                                found = (currentchar == '|') || (nextchar == ',');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Hexadecimal:
                                found = !Helper.IsHexadecimal(nextchar);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Octal:
                                found = !Helper.IsOctal(nextchar);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Binary:
                                found = !Helper.IsBinary(nextchar);
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.None:
                                break;
                            case Lexemes.Number:
                                break;
                            case Lexemes.Boolean:
                                break;
                            case Lexemes.Label:
                                found = currentchar == ':';
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            case Lexemes.Comment:
                                found = index == (str.Length - 1) || (nextchar == '\r' || nextchar == '\n');
                                if(found)
                                    _nextLexeme = Lexemes.None;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                }

                if(found)
                {
                    //Finish current token + set up next token
                    var token = _tokens.Count > 0 ? _tokens[_tokens.Count - 1] : null;
                    _tokens.Add(Token.Get(_sb.ToString(), token));
                    _sb = new StringBuilder();
                }

                _currentLexeme = _nextLexeme;

                if((index + 1) < str.Length)
                {
                    index = index + 1;
                    continue;
                }

                break;
            }
        }
    }
}