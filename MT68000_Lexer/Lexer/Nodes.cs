using System;
using System.Diagnostics.CodeAnalysis;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class Nodes.
    /// </summary>
    public class Nodes
    {
        /// <summary>
        ///     Class Node.
        /// </summary>
        public class Node
        {
            /// <summary>
            ///     The evaluated type
            /// </summary>
            public Type EvaluatedType;

            /// <summary>
            ///     The evaluated value
            /// </summary>
            public object EvaluatedValue = "Not Evaluated";

            /// <summary>
            ///     Converts to ken.
            /// </summary>
            public Token Token;

            /// <summary>
            ///     The value
            /// </summary>
            public object Value;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            public Node() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            /// <param name="token">The token.</param>
            public Node(Token token)
            {
                Token = token;
                Value = token.TokenValue;
            }

            /// <summary>
            ///     Evaluates <see langword="this" /> instance.
            /// </summary>
            /// <returns><see cref="System.Object" />.</returns>
            /// <exception cref="MissingFieldException"></exception>
            public virtual object Evaluate()
            {
                switch (Token.TokenType)
                {
                    case Token.TokenTypes.Variable:
                        if (Variables.VariableList.ContainsKey(Value.ToString()))
                        {
                            EvaluatedValue = Variables.VariableList[Value.ToString()];
                        }
                        else
                        {
                            throw new MissingFieldException(
                                $"Variable '{Value}' not initialised - does not exist in variable list");
                        }

                        break;
                    case Token.TokenTypes.Hex:
                        EvaluatedValue = Helper.HexToDec(Value.ToString());
                        break;
                    case Token.TokenTypes.Octal:
                        EvaluatedValue = Helper.OctToDec(Value.ToString());
                        break;
                    case Token.TokenTypes.Binary:
                        EvaluatedValue = Helper.BinToDec(Value.ToString());
                        break;
                }

                if (EvaluatedValue.ToString() == "Not Evaluated")
                {
                    EvaluatedValue = Value;
                }

                EvaluatedType = Helper.GetType(EvaluatedValue);
                return EvaluatedValue;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Class Monadic.
        ///     Implements the <see cref="T:MT68000_Lexer.Lexer.Nodes.Node" />
        /// </summary>
        /// <seealso cref="T:MT68000_Lexer.Lexer.Nodes.Node" />
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class Monadic : Node
        {
            /// <summary>
            ///     The LHS
            /// </summary>
            public Node LHS;

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Monadic" /> class.
            /// </summary>
            public Monadic() { }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Monadic" /> class.
            /// </summary>
            /// <param name="token">The token.</param>
            public Monadic(Token token)
            {
                Token = token;
                Value = token.TokenValue;
            }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Monadic" /> class.
            /// </summary>
            /// <param name="lhs">The LHS.</param>
            /// <param name="token">The token.</param>
            public Monadic(Node lhs, Token token)
            {
                LHS = lhs;

                Token = token;
                Value = token.TokenValue;
            }

            /// <inheritdoc />
            /// <summary>
            ///     Evaluates <see langword="this" /> instance.
            /// </summary>
            /// <returns><see cref="T:System.Object" />.</returns>
            /// <exception cref="T:System.Exception"></exception>
            public override object Evaluate()
            {
                LHS.Evaluate();
                if (LHS.EvaluatedValue == null)
                {
                    throw new Exception("LHS null value");
                }

                EvaluatedValue = Token.TokenType == Token.TokenTypes.Unary
                                     ? ExpressionEvaluation.CarryOutOperation(this)
                                     : LHS.Evaluate();
                EvaluatedType = Helper.GetType(EvaluatedValue);
                return EvaluatedValue;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Class Diadic.
        ///     Implements the <see cref="T:MT68000_Lexer.Lexer.Nodes.Monadic" />
        /// </summary>
        /// <seealso cref="T:MT68000_Lexer.Lexer.Nodes.Monadic" />
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class Diadic : Monadic
        {
            /// <summary>
            ///     The RHS
            /// </summary>
            public Node RHS;

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Diadic" /> class.
            /// </summary>
            public Diadic() { }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Diadic" /> class.
            /// </summary>
            /// <param name="token">The token.</param>
            public Diadic(Token token)
            {
                Token = token;
                Value = token.TokenValue;
            }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Diadic" /> class.
            /// </summary>
            /// <param name="lhs">The LHS.</param>
            /// <param name="token">The token.</param>
            /// <param name="rhs">The RHS.</param>
            public Diadic(Node lhs, Token token, Node rhs)
            {
                LHS = lhs;
                RHS = rhs;

                Token = token;
                Value = token.TokenValue;
            }

            /// <inheritdoc />
            /// <summary>
            ///     Evaluates <see langword="this" /> instance.
            /// </summary>
            /// <returns><see cref="T:System.Object" />.</returns>
            /// <exception cref="T:System.Exception">
            /// </exception>
            public override object Evaluate()
            {
                LHS.Evaluate();
                RHS.Evaluate();
                if (LHS.EvaluatedValue == null)
                {
                    throw new Exception("LHS null value");
                }

                if (RHS.EvaluatedValue == null)
                {
                    throw new Exception("RHS null value");
                }

                EvaluatedValue = ExpressionEvaluation.CarryOutOperation(this);
                EvaluatedType  = Helper.GetType(EvaluatedValue);
                return EvaluatedValue;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Class Triadic.
        ///     Implements the <see cref="T:MT68000_Lexer.Lexer.Nodes.Diadic" />
        /// </summary>
        /// <seealso cref="T:MT68000_Lexer.Lexer.Nodes.Diadic" />
        public class Triadic : Diadic
        {
            /// <summary>
            ///     The test
            /// </summary>
            public Node Test;

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Triadic" /> class.
            /// </summary>
            public Triadic() { }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Triadic" /> class.
            /// </summary>
            /// <param name="test">The test.</param>
            /// <param name="lhs">The LHS.</param>
            /// <param name="rhs">The RHS.</param>
            public Triadic(Node test, Node lhs, Node rhs)
            {
                LHS   = lhs;
                RHS   = rhs;
                Test  = test;
                Value = "?";
            }

            /// <inheritdoc />
            /// <summary>
            ///     Evaluates <see langword="this" /> instance.
            /// </summary>
            /// <returns><see cref="T:System.Object" />.</returns>
            /// <exception cref="T:System.Exception">
            /// </exception>
            public override object Evaluate()
            {
                Test.Evaluate();
                LHS.Evaluate();
                RHS.Evaluate();
                if (Test.EvaluatedValue == null)
                {
                    throw new Exception("Test null value");
                }

                if (LHS.EvaluatedValue == null)
                {
                    throw new Exception("LHS null value");
                }

                if (RHS.EvaluatedValue == null)
                {
                    throw new Exception("RHS null value");
                }

                EvaluatedValue = Convert.ToBoolean(Test.Evaluate()) ? LHS.Evaluate() : RHS.Evaluate();
                EvaluatedType  = Helper.GetType(EvaluatedValue);
                return EvaluatedValue;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Class <see cref="Error"/>.
        ///     Implements the <see cref="T:MT68000_Lexer.Lexer.Nodes.Node" />
        /// </summary>
        /// <seealso cref="T:MT68000_Lexer.Lexer.Nodes.Node" />
        public class Error : Node
        {
            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Error" /> class.
            /// </summary>
            public Error() { }

            /// <inheritdoc />
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:MT68000_Lexer.Lexer.Nodes.Error" /> class.
            /// </summary>
            /// <param name="errorMessage">The error message.</param>
            public Error(string errorMessage)
            {
                Value = errorMessage;
            }
        }
    }
}