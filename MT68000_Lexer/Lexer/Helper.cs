using System;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class Helper.
    /// </summary>
    public class Helper
    {
        /// <summary>
        ///     Determines whether the specified <paramref name="character" />
        ///     is alpha.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is alpha; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsAlpha(char character)
        {
            return IsUppercase(character) || IsLowerCase(character);
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" />
        ///     is uppercase.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is uppercase; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsUppercase(char character)
        {
            return (character >= 'A') && (character <= 'Z');
        }

        /// <summary>
        ///     Determines whether [is lower case] [the specified
        ///     <paramref name="character" />].
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if [is lower case] [the
        ///     specified <paramref name="character" />]; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsLowerCase(char character)
        {
            return (character >= 'a') && (character <= 'z');
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     numeric.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is numeric; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsNumeric(char character)
        {
            return (character >= '0') && (character <= '9');
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     alphanumeric.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is alphanumeric; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsAlphanumeric(char character)
        {
            return IsAlpha(character) || IsNumeric(character) || character == '_';
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     symbol.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is symbol; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsSymbol(char character)
        {
            return ((character >= '!') && (character <= '/')) ||
                   ((character >= ':') && (character <= '@')) ||
                   ((character >= '[') && (character <= '`')) ||
                   ((character >= '{') && (character <= '~'));
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is a
        ///     bracket.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is a bracket; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsBracket(char character)
        {
            return (character == '(') || (character == ')') ||
                   (character == '[') || (character == ']') ||
                   (character == '{') || (character == '}');
        }

        /// <summary>
        ///     Determines whether [is less or greater symbol] [the specified
        ///     <paramref name="character" />].
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if [is less or greater
        ///     symbol] [the specified <paramref name="character" />]; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsLessOrGreaterSymbol(char character)
        {
            return (character == '<') || (character == '>');
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     hexadecimal.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is hexadecimal; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsHexadecimal(char character)
        {
            return ((character >= '0') && (character <= '9')) ||
                   ((character >= 'A') && (character <= 'F')) ||
                   ((character >= 'a') && (character <= 'f'));
        }
        
        /// <summary>
        /// Determines whether the specified string is hexadecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><see langword="true" /> if the specified string is hexadecimal; otherwise, <see langword="false" />.</returns>
        public static bool IsHexadecimal(string str)
        {
            foreach (char character in str)
                if (!IsHexadecimal (character))
                    return false;
            return true;
        }

        /// <summary>
        /// Determines whether the specified string is uppercase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><see langword="true" /> if the specified string is uppercase; otherwise, <see langword="false" />.</returns>
        public static bool IsUppercase(string str)
        {
            foreach (char character in str)
                if (!IsUppercase (character))
                    return false;
            return true;
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     octal.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is octal; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsOctal(char character)
        {
            return (character >= '0') && (character <= '7');
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="character" /> is
        ///     binary.
        /// </summary>
        /// <param name="character">The <paramref name="character" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="character" /> is binary; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsBinary(char character)
        {
            return (character == '0') || (character == '1');
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="token" /> is
        ///     numeric.
        /// </summary>
        /// <param name="token">The <paramref name="token" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="token" /> is numeric; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsNumeric(Token token)
        {
            return (token.TokenType == Token.TokenTypes.Decimal) ||
                   (token.TokenType == Token.TokenTypes.Float) ||
                   (token.TokenType == Token.TokenTypes.Binary) ||
                   (token.TokenType == Token.TokenTypes.Octal) ||
                   (token.TokenType == Token.TokenTypes.Hex);
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="token" /> is
        ///     comparison.
        /// </summary>
        /// <param name="token">The <paramref name="token" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the specified
        ///     <paramref name="token" /> is comparison; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsComparison(Token token)
        {
            if (token.TokenType != Token.TokenTypes.Symbol)
            {
                return false;
            }

            var character = (char) token.TokenValue;
            // ReSharper disable once PossibleInvalidCastException
            var str = new string(value: token.TokenValue);
            return (character == '<') ||
                   (character == '>') ||
                   (str == "<=") ||
                   (str == ">=") ||
                   (str == "<>") ||
                   (str == "==");
        }

        // ReSharper disable once UnusedMember.Local
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if [is valid
        ///     <see langword="type" />] [the specified <see langword="type" />]; otherwise,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     .
        /// </returns>
        private bool IsValidType(Type type)
        {
            return (type == typeof(bool)) || (type == typeof(byte)) || (type == typeof(short)) ||
                   (type == typeof(int)) ||
                   (type == typeof(long)) || (type == typeof(ushort)) || (type == typeof(uint)) ||
                   (type == typeof(ulong)) ||
                   (type == typeof(float)) || (type == typeof(double)) || (type == typeof(string)) ||
                   (type == typeof(byte[]));
        }

        /// <summary>
        ///     Gets the bytes.
        /// </summary>
        /// <param name="test">The <paramref name="test" />.</param>
        /// <returns><see cref="byte" />[].</returns>
        public static byte[] GetBytes(byte[] test)
        {
            var numArray  = new byte[test.Length * 2];
            var num       = 0;
            var numArray1 = test;
            foreach (var num1 in numArray1)
            {
                numArray[num]     = 0;
                numArray[num + 1] = num1;
                num               = num + 2;
            }

            return numArray;
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <param name="obj">The <see langword="object" />.</param>
        /// <returns>Type.</returns>
        public static Type GetType(object obj)
        {
            return obj.GetType();
        }

        /// <summary>
        ///     Converts to <see langword="decimal" />.
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <returns><see cref="long" />.</returns>
        public static long HexToDec(string hexString)
        {
            return Convert.ToInt64(hexString
                                      .Replace("0x", ""),
                                   16);
        }

        /// <summary>
        ///     Converts to <see langword="decimal" />.
        /// </summary>
        /// <param name="octString">The octal string.</param>
        /// <returns><see cref="long" />.</returns>
        public static long OctToDec(string octString)
        {
            return Convert.ToInt64(octString
                                      .Replace("0o", ""),
                                   8);
        }

        /// <summary>
        ///     Converts to <see langword="decimal" />.
        /// </summary>
        /// <param name="binString">The binary string.</param>
        /// <returns><see cref="long" />.</returns>
        public static long BinToDec(string binString)
        {
            return Convert.ToInt64(binString.Replace("0b", ""), 2);
        }

        /// <summary>
        ///     Shifts the string left.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="shiftAmount">The shift amount.</param>
        /// <returns>System.<see cref="String" />.</returns>
        public static string ShiftStringLeft(string str, int shiftAmount)
        {
            shiftAmount = shiftAmount % str.Length;
            return $"{str.Substring(shiftAmount)}" +
                   $"{str.Substring(0, shiftAmount)}";
        }

        /// <summary>
        ///     Shifts the string right.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="shiftAmount">The shift amount.</param>
        /// <returns>System.<see cref="String" />.</returns>
        public static string ShiftStringRight(string str, int shiftAmount)
        {
            shiftAmount = str.Length - (shiftAmount % str.Length);
            return $"{str.Substring(shiftAmount)}" +
                   $"{str.Substring(0, shiftAmount)}";
        }

        /// <summary>
        ///     Duplicates the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="times">The amount of <paramref name="times" />.</param>
        /// <returns>System.<see cref="String" />.</returns>
        public static string DuplicateString(string str, int times)
        {
            var result = string.Empty;
            for (var i = 0; i < times; i++)
            {
                result += str;
            }

            return result;
        }
    }
}