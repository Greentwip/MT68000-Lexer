using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class <see cref="SyntacticalAnalyser"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleInvalidCastException")]
    public class SyntacticalAnalyser
    {
        /// <summary>
        ///     The <see cref="AbstractSyntaxTree" />.
        /// </summary>
        public static AbstractSyntaxTree AbstractSyntaxTree = new AbstractSyntaxTree();

        /// <summary>
        ///     The list of Tokens.
        /// </summary>
        public static List<Token> Tokens = new List<Token>();

        /// <summary>
        ///     The current <see cref="Token" /> index.
        /// </summary>
        public static int TokenIndex;

        /// <summary>
        ///     The current <see cref="Token" />.
        /// </summary>
        public static Token CurrentToken = new Token();

        /// <summary>
        ///     Gets the tree.
        /// </summary>
        /// <returns><see cref="AbstractSyntaxTree" />.</returns>
        public AbstractSyntaxTree GetTree()
        {
            return AbstractSyntaxTree;
        }

        /// <summary>
        ///     Parses the expression.
        /// </summary>
        /// <param name="tokens">The <paramref name="tokens" />.</param>
        /// <returns><see cref="AbstractSyntaxTree" />.</returns>
        /// <exception cref="Exception"></exception>
        public AbstractSyntaxTree ParseExpression(List<Token> tokens)
        {
            Console.WriteLine("Syntactical Analysis started...");

            //Error - no tokens passed in argument
            if (tokens.Count == 0)
            {
                throw new Exception("No tokens to parse.");
            }

            //Set up tokens and starting index
            Tokens = new List<Token>();
            foreach (var token in tokens)
            {
                //Add addition signs before all the negative symbols
                if (token.TokenValue.ToString() == "-")
                {
                    Tokens.Add(new Token
                    {
                        TokenValue = '+',
                        TokenType  = Token.TokenTypes.Addition
                    });
                }

                Tokens.Add(token);
            }

            TokenIndex   = 0;
            CurrentToken = Tokens[TokenIndex];

            //Create new Abstract Syntax Tree
            AbstractSyntaxTree = new AbstractSyntaxTree();

            //Start the syntax analysis
            AsmExprn();
            AbstractSyntaxTree.Nodes.Reverse();
            Console.WriteLine("Syntactical Analysis complete");
            return AbstractSyntaxTree;
        }

        /// <summary>
        ///     Consumes the current <see cref="Token" />.
        /// </summary>
        private static void ConsumeToken()
        {
            //Debug output to show which tokens are being consumed
            Console.WriteLine($"Consuming token: {Tokens[TokenIndex].TokenValue}");

            //If not at the last token
            if (TokenIndex != (Tokens.Count - 1))
            {
                //Increment the current token
                TokenIndex++;
                CurrentToken = Tokens[TokenIndex];
            }
            else
            {
                CurrentToken = null;
            }
        }

        /// <summary>
        ///     Returns the previous <see cref="Token" />
        /// </summary>
        private static void ReturnToken()
        {
            //If there's no tokens to return, do nothing
            if (TokenIndex == 0)
            {
                return;
            }

            //Decrement token index and set the current token back to the last one
            TokenIndex--;
            CurrentToken = Tokens[TokenIndex];
        }

        #region ProductionFunctions

        /// <summary>
        ///     <see cref="AsmExprn" /> := {"*"}[":"Instruction] {[<see cref="Exprn" />
        ///     |<see cref="AsmExprn" />]|"?"};
        /// </summary>
        /// <returns><see cref="Nodes" />.<see cref="Nodes.Node" />.</returns>
        private static Nodes.Node AsmExprn()
        {
            //Stringbuilder for storing our ASM expression whilst we parse it
            var sb = new StringBuilder();
            
            //Consumes the token
            sb.Append(CurrentToken.TokenValue);
            ConsumeToken();

            //Get all idents
            if (CurrentToken.TokenType == Token.TokenTypes.Instruction)
            {
                //If ident found, consume it
                sb.Append(CurrentToken.TokenValue);
                ConsumeToken();
            }

            //Check for a GET command
            if (CurrentToken.TokenValue.ToString() == "?")
            {
                //Consume GET command token
                sb.Append(CurrentToken.TokenValue);
                ConsumeToken();

                //Return error if EOL not found
                if (CurrentToken.TokenType != Token.TokenTypes.Eol)
                {
                    return Error("Could not find end of line token");
                }

                //Consume EOL token if found
                sb.Append(CurrentToken.TokenValue);
                ConsumeToken();

                //Create our node and return
                return AddNode(new Nodes.Node(new Token {TokenValue = sb.ToString()})
                {
                    Value = sb.ToString(),
                    Token = {TokenValue = sb.ToString()}
                });
            }

            //Try to analyse the left hand side
            var lhs = Exprn();
            if (lhs == null)
            {
                return new Nodes.Error("LHS could not be evaluated");
            }

            //Add left hand side to a new monadic - WHY?!?!
            var monadic = new Nodes.Monadic(lhs, new Token
            {
                TokenType  = Token.TokenTypes.None,
                TokenValue = sb.ToString()
            });

            //Check for EOL Token and consume
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.Eol))
            {
                return Error("Could not find end of line token");
            }

            ConsumeToken();

            return AddNode(monadic);
        }

        /// <summary>
        ///     The start point of the order of precedence
        /// </summary>
        /// <returns><see cref="Nodes" />.<see cref="Nodes.Node" />.</returns>
        private static Nodes.Node Exprn()
        {
            var node = AssignmentExprn();
            return AddNode(node);
        }

        /// <summary></summary>
        /// <returns>[<see cref="Nodes" />].[<see cref="Nodes.Node" />]. </returns>
        /// <remarks>
        ///     <para>
        ///         <span class="selflink">
        ///             AssignmentExprn
        ///             <span class="languageSpecificText">
        ///                 <span class="cs">()</span>
        ///                 <span class="cpp">()</span><span class="nu">()</span>
        ///                 <span class="fs">()</span>
        ///             </span>
        ///         </span>
        ///         :=
        ///         <span class="nolink">
        ///             TernaryExprn
        ///             <span class="languageSpecificText">
        ///                 <span class="cs">()</span>
        ///                 <span class="cpp">()</span><span class="nu">()</span>
        ///                 <span class="fs">()</span>
        ///             </span>
        ///         </span>
        ///         { { "=" | "+=" | "-=" |
        ///         "*=" | "/=" | "%=" | "&lt;&lt;=" | "&gt;&gt;=" | "&amp;=" | "^="
        ///         | "|=" }
        ///         <span class="selflink">
        ///             AssignmentExprn
        ///             <span class="languageSpecificText">
        ///                 <span class="cs">()</span>
        ///                 <span class="cpp">()</span><span class="nu">()</span>
        ///                 <span class="fs">()</span>
        ///             </span>
        ///         </span>
        ///         };
        ///     </para>
        ///     <para>
        ///         <see cref="Nodes.Monadic" /> | { <see cref="Nodes.Diadic" />
        ///         }
        ///     </para>
        /// </remarks>
        private static Nodes.Node AssignmentExprn()
        {
            //Go further down the order of precedence
            var node = TernaryExprn();

            //Return if at the end of the expression or not Assignment operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.Assignment))
            {
                return node;
            }

            //Create a new diadic node for the Assignment operation
            //Set left hand side and consume the Assignment token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = AssignmentExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="TernaryExprn" /> := <see cref="LogicalOrExprn" /> { "?"
        ///     <see cref="TernaryExprn" /> : "<see cref="TernaryExprn" />" };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic | Triadic }
        /// </returns>
        private static Nodes.Node TernaryExprn()
        {
            //Go further down the order of precedence
            var node = LogicalOrExprn();

            //Return if at the end of the expression or not Ternary operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.Ternary))
            {
                return node;
            }

            //If ternary colon detected here, ternary recursion is probably already in progress
            if (CurrentToken?.TokenValue == ':')
            {
                return node;
            }

            //Return if not ternary ? symbol detected
            if (CurrentToken?.TokenValue != '?')
            {
                return new Nodes.Error("Ternary symbol detected but not correct format");
            }

            //Set the test branch and consume the Ternary token
            var triadic = new Nodes.Triadic(node, null, null);
            ConsumeToken();

            //Try to analyse the left hand side
            var lhs = Exprn();
            if (lhs == null)
            {
                return new Nodes.Error("LHS could not be evaluated");
            }

            //If analysis successful, add to the left hand side of the triadic
            triadic.LHS = lhs;

            //Check for the Ternary colon token and consume it
            if (CurrentToken?.TokenValue != ':')
            {
                return new Nodes.Error("Ternary colon token not found");
            }

            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = Exprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            //Also temporarily "evaluate" triadic value as it currently has no value assigned
            triadic.RHS = rhs;
            return AddNode(triadic);
        }

        /// <summary>
        ///     <see cref="LogicalOrExprn" /> := <see cref="LogicalAndExprn" /> {
        ///     { "||" } <see cref="LogicalOrExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node LogicalOrExprn()
        {
            //Go further down the order of precedence
            var node = LogicalAndExprn();

            //Return if at the end of the expression or not Logical OR operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.LogicalOr))
            {
                return node;
            }

            //Create a new diadic node for the Logical OR operation
            //Set left hand side and consume the Logical OR token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = LogicalOrExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="LogicalAndExprn" /> := <see cref="BitwiseOrExprn" /> {
        ///     { "&amp;&amp;" } <see cref="LogicalAndExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node LogicalAndExprn()
        {
            //Go further down the order of precedence
            var node = BitwiseOrExprn();

            //Return if at the end of the expression or not Logical AND operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.LogicalAnd))
            {
                return node;
            }

            //Create a new diadic node for the Logical AND operation
            //Set left hand side and consume the Logical AND token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = LogicalAndExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="BitwiseOrExprn" /> := <see cref="BitwiseXorExprn" /> {
        ///     { "|" } <see cref="BitwiseOrExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node BitwiseOrExprn()
        {
            //Go further down the order of precedence
            var node = BitwiseXorExprn();

            //Return if at the end of the expression or not Bitwise OR operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.BitwiseOr))
            {
                return node;
            }

            //Create a new diadic node for the Bitwise OR operation
            //Set left hand side and consume the Bitwise OR token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = BitwiseOrExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="BitwiseXorExprn" /> := <see cref="BitwiseAndExprn" /> {
        ///     { "^" } <see cref="BitwiseXorExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node BitwiseXorExprn()
        {
            //Go further down the order of precedence
            var node = BitwiseAndExprn();

            //Return if at the end of the expression or not Bitwise XOR operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.BitwiseXor))
            {
                return node;
            }

            //Create a new diadic node for the Bitwise XOR operation
            //Set left hand side and consume the Bitwise XOR token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = BitwiseXorExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="BitwiseAndExprn" /> := <see cref="EqualityExprn" /> { { "&amp;" } <see cref="BitwiseAndExprn" /> };
        /// </summary>
        /// <returns><see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | { Diadic }</returns>
        private static Nodes.Node BitwiseAndExprn()
        {
            //Go further down the order of precedence
            var node = EqualityExprn();

            //Return if at the end of the expression or not Bitwise AND operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.BitwiseAnd))
            {
                return node;
            }

            //Create a new diadic node for the Bitwise AND operation
            //Set left hand side and consume the Bitwise AND token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = BitwiseAndExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="EqualityExprn" /> := <see cref="ComparisonExprn" /> { {
        ///     "== " | "&lt;>" | "!=" } <see cref="EqualityExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node EqualityExprn()
        {
            //Go further down the order of precedence
            var node = ComparisonExprn();

            //Return if at the end of the expression or not Equality operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.Equality))
            {
                return node;
            }

            //Create a new diadic node for the Equality operation
            //Set left hand side and consume the Equality token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = EqualityExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="ComparisonExprn" />:= <see cref="BitShiftExprn" /> { { ">"
        ///     | ">=" | "&lt;" | "&lt;=" } <see cref="ComparisonExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node ComparisonExprn()
        {
            //Go further down the order of precedence
            var node = BitShiftExprn();

            //Return if at the end of the expression or not Comparison operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.Comparison))
            {
                return node;
            }

            //Create a new diadic node for the Comparison operation
            //Set left hand side and consume the Comparison token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = ComparisonExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="BitShiftExprn" /> := <see cref="AddExprn" /> { {
        ///     "&lt;&lt; | ">>" | "&lt;&lt;&lt;" | ">>>"}
        ///     <see cref="BitShiftExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node BitShiftExprn()
        {
            //Go further down the order of precedence
            var node = AddExprn();

            //Return if at the end of the expression or not BitShift operation
            if ((CurrentToken == null) || (CurrentToken?.TokenType != Token.TokenTypes.BitShift))
            {
                return node;
            }

            //Create a new diadic node for the Bitshift operation
            //Set left hand side and consume the Bitshift token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = BitShiftExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="AddExprn" /> := <see cref="MulExprn" /> { { "+" | "-"
        ///     }
        ///     <see cref="AddExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node AddExprn()
        {
            //Go further down the order of precedence
            var node = MulExprn();

            //Return if at the end of the expression or not Addition / Subtraction operation
            if ((CurrentToken == null) ||
                (CurrentToken?.TokenType != Token.TokenTypes.Addition))
            {
                return node;
            }

            //Create a new diadic node for the Addition operation
            //Set left hand side and consume the Addition token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = AddExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="MulExprn" /> := <see cref="UnaryExprn" /> { { "*" | "/"
        ///     | "%" } <see cref="MulExprn" /> };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic | {
        ///     Diadic }
        /// </returns>
        private static Nodes.Node MulExprn()
        {
            //Go further down the order of precedence
            var node = UnaryExprn();

            //Return if at the end of the expression or not Multiplication / Division operation
            if ((CurrentToken == null) ||
                (CurrentToken?.TokenType != Token.TokenTypes.Multiplication))
            {
                return node;
            }

            //Create a new diadic node for the Multiplication operation
            //Set left hand side and consume the Multiplication token
            var diadic = new Nodes.Diadic(node, CurrentToken, null);
            ConsumeToken();

            //Try to analyse the right hand side
            var rhs = MulExprn();
            if (rhs == null)
            {
                return new Nodes.Error("RHS could not be evaluated");
            }

            //If analysis successful, add to the diadic and append to the Abstract Syntax Tree
            diadic.RHS = rhs;
            return AddNode(diadic);
        }

        /// <summary>
        ///     <see cref="UnaryExprn" />:= {"(" <see cref="Exprn" /> ")" |
        ///     <see cref="BaseExprn" /> | "!" <see cref="UnaryExprn" /> | "+"
        ///     <see cref="UnaryExprn" /> | "-" <see cref="UnaryExprn" /> | "~"
        ///     <see cref="UnaryExprn" /> }; NOTE: This function is purposefully
        ///     different from the layout of the other production functions.
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic
        /// </returns>
        private static Nodes.Node UnaryExprn()
        {
            if (CurrentToken.TokenValue == '(')
            {
                //Consume the left bracket
                ConsumeToken();
                //Spins off everything inside the brackets as a new expression to evaluate
                var lhs = Exprn();
                //Consume the right bracket
                if (CurrentToken.TokenValue != ')')
                {
                    return AddNode(new Nodes.Error("Matching closing bracket not found"));
                }

                ConsumeToken();
                return lhs;
                //Throw error if right bracket not found
            }

            if ((CurrentToken != null) && (CurrentToken.TokenType == Token.TokenTypes.Unary))
            {
                //Create a new monadic node for the Unary operation and consume the token
                var monadic = new Nodes.Monadic(null, CurrentToken);
                ConsumeToken();

                //Set left hand side
                var mlhs = BaseExprn();
                if (mlhs == null)
                {
                    return new Nodes.Error("Value LHS could not be evaluated");
                }

                //If analysis successful, add to the monadic and return the monadic (will be added later)
                monadic.LHS = mlhs;
                return monadic;
            }

            //Create a new node, attempt to find a last-resort base expression
            var node = BaseExprn();
            return node == null ? new Nodes.Error("Could not find base expression") : AddNode(node);
        }

        /// <summary>
        ///     <see cref="BaseExprn" /> := { ident | numeric | string | bool };
        /// </summary>
        /// <returns>
        ///     <see cref="Nodes" />.<see cref="Nodes.Node" />. Monadic
        /// </returns>
        private static Nodes.Node BaseExprn()
        {
            //Go further down the order of precedence
            //Return if at the end of the expression or not Base value
            if ((CurrentToken == null) ||
                ((CurrentToken?.TokenType != Token.TokenTypes.Instruction) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.String) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Boolean) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Binary) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Float) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Hex) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Octal) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Decimal) &&
                 (CurrentToken?.TokenType != Token.TokenTypes.Variable) &&
                 !Helper.IsNumeric(CurrentToken)))
            {
                return new Nodes.Error("Base value not found");
            }

            //Create node for base value, and consume the token
            var node = new Nodes.Node(CurrentToken);
            ConsumeToken();
            return node;
        }

        /// <summary>
        ///     Errors the specified <paramref name="message" />.
        /// </summary>
        /// <param name="message">The <paramref name="message" />.</param>
        /// <returns><see cref="Nodes" />.<see cref="Nodes.Node" />.</returns>
        public static Nodes.Node Error(string message)
        {
            var node = new Nodes.Error(message);
            AbstractSyntaxTree.Nodes.Add(node);
            return node;
        }

        /// <summary>
        ///     Adds the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="Nodes" />.<see cref="Nodes.Node" />.</returns>
        public static Nodes.Node AddNode(Nodes.Node node)
        {
            AbstractSyntaxTree.Nodes.Add(node);
            return node;
        }

        #endregion ProductionFunctions
    }
}