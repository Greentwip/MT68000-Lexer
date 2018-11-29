using System;
using System.Linq;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class Token.
    /// </summary>
    public class Token
    {
        /// <summary>
        ///     <see cref="Enum" /> TokenTypes
        /// </summary>
        public enum TokenTypes
        {
            /// <summary>
            ///     The none
            /// </summary>
            None,

            /// <summary>
            ///     The string
            /// </summary>
            String,

            /// <summary>
            ///     The instruction
            /// </summary>
            Instruction,

            /// <summary>
            ///     The decimal
            /// </summary>
            Decimal,

            /// <summary>
            ///     The float
            /// </summary>
            Float,

            /// <summary>
            ///     The hexadecimal
            /// </summary>
            Hex,

            /// <summary>
            ///     The octal
            /// </summary>
            Octal,

            /// <summary>
            ///     The binary
            /// </summary>
            Binary,

            /// <summary>
            ///     The symbol
            /// </summary>
            Symbol,

            /// <summary>
            ///     The boolean
            /// </summary>
            Boolean,

            /// <summary>
            ///     The variable
            /// </summary>
            Variable,

            /// <summary>
            ///     The eol
            /// </summary>
            Eol,

            //Production function operations
            /// <summary>
            ///     The assignment
            /// </summary>
            Assignment,

            /// <summary>
            ///     The ternary
            /// </summary>
            Ternary,

            /// <summary>
            ///     The logical or
            /// </summary>
            LogicalOr,

            /// <summary>
            ///     The logical and
            /// </summary>
            LogicalAnd,

            /// <summary>
            ///     The bitwise or
            /// </summary>
            BitwiseOr,

            /// <summary>
            ///     The bitwise xor
            /// </summary>
            BitwiseXor,

            /// <summary>
            ///     The bitwise and
            /// </summary>
            BitwiseAnd,

            /// <summary>
            ///     The equality
            /// </summary>
            Equality,

            /// <summary>
            ///     The comparison
            /// </summary>
            Comparison,

            /// <summary>
            ///     The bit shift
            /// </summary>
            BitShift,

            /// <summary>
            ///     The addition
            /// </summary>
            Addition,

            /// <summary>
            ///     The multiplication
            /// </summary>
            Multiplication,

            /// <summary>
            ///     The unary
            /// </summary>
            Unary,

            /// <summary>
            ///     The comment
            /// </summary>
            Comment,

            /// <summary>
            ///     The label
            /// </summary>
            Label,

            /// <summary>
            ///     The label
            /// </summary>
            Separator,

            /// <summary>
            ///     The datatype
            /// </summary>
            AddressingMode,

            /// <summary>
            ///     The address register
            /// </summary>
            AddressRegister,

            /// <summary>
            ///     The data register
            /// </summary>
            DataRegister,

            /// <summary>
            ///     The other register
            /// </summary>
            OtherRegister,
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     The <see cref="Token" /> type.
        /// </summary>
        /// <value>The type of the <see cref="Token" />.</value>
        public TokenTypes TokenType { get; set; } = TokenTypes.None;

        /// <summary>
        ///     The <see cref="Token" /> value.
        /// </summary>
        /// <value>The <see cref="Token" /> value.</value>
        public dynamic TokenValue { get; set; }

        /// <summary>
        ///     Gets the specified <see cref="Token" /> type.
        /// </summary>
        /// <param name="tokenType">Type of the <see cref="Token" />.</param>
        /// <returns><see cref="Token" />.</returns>
        public static Token Get(TokenTypes tokenType)
        {
            return new Token {TokenType = tokenType};
        }

        /// <summary>
        ///     Gets the specified <see langword="object" />.
        /// </summary>
        /// <param name="obj">The <see langword="object" />.</param>
        /// <param name="lastToken"></param>
        /// <returns><see cref="Token" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Token Get(object obj, Token lastToken)
        {
            var    tokenType = TokenTypes.None;
            object tokenValue = null;

            if (obj.ToString().Length > 1)
            {
                var objstr = (string) obj;

                var isNum = true;
                foreach (var c in objstr)
                {
                    if (!Helper.IsNumeric(c) || (c == '-'))
                    {
                        isNum = false;
                    }
                }
                
                if (objstr.Substring (0, 1) == ".")
                {
                    if (Enum.IsDefined (typeof (AddressingModes.AddressMode), objstr.Substring(1, 1)))
                    {
                        tokenType = TokenTypes.AddressingMode;
                        tokenValue = objstr;
                    }
                }
                else if (objstr.Substring(0, 1) == ";")
                {
                    tokenType  = TokenTypes.Comment;
                    tokenValue = objstr;
                }
                else if (objstr.Substring(objstr.Length -1, 1) == ":")
                {
                    tokenType = TokenTypes.Label;
                    tokenValue = objstr;
                }
                else if (Helper.IsHexadecimal (objstr) && lastToken != null && Helper.IsUppercase(objstr))
                {
                    if (lastToken == null)
                        return null;
                    switch (lastToken.TokenValue.ToString ()[0])
                    {
                        case '$':
                        tokenType = TokenTypes.Hex;
                        tokenValue = obj.ToString ();
                        break;
                        case '%':
                        tokenType = TokenTypes.Binary;
                        tokenValue = obj.ToString ();
                        break;
                        case '@':
                        tokenType = TokenTypes.Octal;
                        tokenValue = obj.ToString ();
                        break;
                        case '#':
                        tokenType = TokenTypes.Decimal;
                        tokenValue = obj.ToString ();
                        break;
                    }
                }
                else if (isNum)
                {
                    try
                    {
                        tokenType  = TokenTypes.Decimal;
                        tokenValue = Convert.ToDouble(objstr);
                    }
                    catch
                    {
                        throw new ArgumentOutOfRangeException(nameof(obj));
                    }
                }
                else if (float.TryParse(objstr, out var tempFloat))
                {
                    tokenType  = TokenTypes.Float;
                    tokenValue = tempFloat;
                }
                else if (Enum.IsDefined (typeof (Registers.AddressRegisters), objstr))
                {
                    tokenType = TokenTypes.AddressRegister;
                    tokenValue = objstr;
                }
                else if (Enum.IsDefined (typeof (Registers.DataRegisters), objstr))
                {
                    tokenType = TokenTypes.DataRegister;
                    tokenValue = objstr;
                }
                else if (Enum.IsDefined (typeof (Registers.OtherRegisters), objstr))
                {
                    tokenType = TokenTypes.OtherRegister;
                    tokenValue = objstr;
                }
                else if (InstructionSet.Instructions.ContainsKey(objstr))
                {
                    tokenType  = TokenTypes.Instruction;
                    tokenValue = objstr;
                }
                else if ((objstr.ToLower() == "true") || (objstr.ToLower() == "false"))
                {
                    tokenType  = TokenTypes.Boolean;
                    tokenValue = Convert.ToBoolean(obj);
                }
                else if (objstr.Contains('"') || objstr.Contains('\''))
                {
                    tokenType  = TokenTypes.String;
                    tokenValue = objstr.Replace("\"", "").Replace("'", "");
                }
                //else if (objstr.Contains('.'))
                //{
                //    tokenType  = TokenTypes.Instruction;
                //    tokenValue = objstr;
                //}
                else
                {
                    bool lower = false;
                    foreach (char c in objstr)
                        if (!Helper.IsHexadecimal(c) || Helper.IsLowerCase (c))
                            lower = true;
                    tokenType = lower ? TokenTypes.Variable : TokenTypes.Hex;
                    tokenValue = objstr;

                    switch (objstr)
                    {
                        case "-=":
                        case "+=":
                        case "*=":
                        case "/=":
                        case "%=":
                        case "&=":
                        case "^=":
                        case "|=":
                        case "<<=":
                        case ">>=":
                        {
                            tokenType = TokenTypes.Assignment;
                            break;
                        }
                        case "||":
                        {
                            tokenType = TokenTypes.LogicalOr;
                            break;
                        }
                        case "&&":
                        {
                            tokenType = TokenTypes.LogicalAnd;
                            break;
                        }
                        case "==":
                        case "<>":
                        case "!=":
                        {
                            tokenType = TokenTypes.Equality;
                            break;
                        }
                        case "<=":
                        case ">=":
                        {
                            tokenType = TokenTypes.Comparison;
                            break;
                        }
                        case "<<":
                        case ">>":
                        case "<<<":
                        case ">>>":
                        {
                            tokenType = TokenTypes.BitShift;
                            break;
                        }
                    }

                    if (objstr.Contains('.'))
                    {
                        double num;
                        if (double.TryParse(objstr, out num))
                        {
                            tokenType  = TokenTypes.Float;
                            tokenValue = num;
                        }
                    }
                }
            }
            else
            {
                var objchar = obj.ToString()[0];
                    
                    //(char) obj;
                tokenValue = objchar;

                if (objchar == '\n' || objchar == '\r')
                {
                    tokenType = TokenTypes.Eol;
                    tokenValue = objchar;
                }
                else if (objchar == ',')
                {
                    tokenType = TokenTypes.Separator;
                    tokenValue = objchar;
                }
                else if (Helper.IsSymbol (objchar))
                {
                    tokenType = TokenTypes.Symbol;
                    tokenValue = objchar;
                }
                else if (Helper.IsAlpha (objchar))
                {
                    tokenType = TokenTypes.Variable;
                    tokenValue = objchar.ToString ();
                }
                else if (Helper.IsHexadecimal(objchar) && lastToken != null && Helper.IsUppercase(objchar))
                {
                    switch (lastToken.TokenValue) {
                        case '$':
                            tokenType  = TokenTypes.Hex;
                            tokenValue = obj.ToString ();
                            break;
                        case '%':
                            tokenType  = TokenTypes.Binary;
                            tokenValue = obj.ToString ();
                            break;
                        case '@':
                            tokenType  = TokenTypes.Octal;
                            tokenValue = obj.ToString ();
                            break;
                        case '#':
                            tokenType  = TokenTypes.Decimal;
                            tokenValue = obj.ToString ();
                            break;
                    }
                }
                else if (Helper.IsNumeric(objchar))
                {
                    tokenType  = TokenTypes.Decimal;
                    tokenValue = Convert.ToInt32(obj.ToString());
                }
                switch (objchar)
                {
                    case '=':
                    {
                        tokenType = TokenTypes.Assignment;
                        break;
                    }
                    case '?':
                    case '&':
                    {
                        tokenType = TokenTypes.BitwiseAnd;
                        break;
                    }
                    case '^':
                    {
                        tokenType = TokenTypes.BitwiseXor;
                        break;
                    }
                    case '|':
                    {
                        tokenType = TokenTypes.BitwiseOr;
                        break;
                    }
                    case '<':
                    case '>':
                    {
                        tokenType = TokenTypes.Comparison;
                        break;
                    }
                    case '+':
                    {
                        tokenType = TokenTypes.Addition;
                        break;
                    }
                    case '*':
                    case '/':
                    {
                        tokenType = TokenTypes.Multiplication;
                        break;
                    }
                    case '(':
                    case ')':
                    case '-':
                    case '!':
                    case '~':
                    case '#':
                    case '@':
                    case '%':
                    case '$':
                    {
                        tokenType = TokenTypes.Unary;
                        break;
                    }
                    case '\'':
                    case '"':
                    case '[':
                    case ']':
                    {
                        tokenType = TokenTypes.String;
                        break;
                    }
                    case '{':
                    case '}':
                    case '§':
                    case '±':
                    case '£':
                    case '_':
                    case '`':
                    case '\\':
                    case '.':
                    {
                        tokenType = TokenTypes.Symbol;
                        break;
                    }
                    case ',':
                    {
                        tokenType = TokenTypes.Separator;
                        break;
                    }
                    case ';':
                    {
                        tokenType = TokenTypes.Comment;
                        break;
                    }
                }
            }

            return new Token
            {
                TokenType  = tokenType,
                TokenValue = tokenValue
            };
        }
    }
}