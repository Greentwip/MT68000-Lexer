namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class <see cref="ExpressionEvaluation"/>.
    /// </summary>
    public class ExpressionEvaluation
    {
        /// <summary>
        ///     Carries the out operation.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object"/>.</returns>
        internal static object CarryOutOperation(Nodes.Node node)
        {
            switch (node.Token.TokenType)
            {
                case Token.TokenTypes.Addition:
                {
                    switch (node.Token.TokenValue)
                    {
                        case '+':
                            return Operations.Add((Nodes.Diadic) node);
                        case '-':
                            return Operations.Subtract((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.Multiplication:
                {
                    switch (node.Token.TokenValue)
                    {
                        case '*':
                            return Operations.Multiply((Nodes.Diadic) node);
                        case '/':
                            return Operations.Divide((Nodes.Diadic) node);
                        case '%':
                            return Operations.Modulus((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.BitwiseAnd:
                {
                    if (node.Token.TokenValue == '&')
                    {
                        return Operations.BitwiseAnd((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.BitwiseOr:
                {
                    if (node.Token.TokenValue == '|')
                    {
                        return Operations.BitwiseOr((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.BitwiseXor:
                {
                    if (node.Token.TokenValue == '^')
                    {
                        return Operations.BitwiseXor((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.BitShift:
                {
                    switch (node.Token.TokenValue)
                    {
                        case "<<":
                            return Operations.ShiftLeft((Nodes.Diadic) node);
                        case ">>":
                            return Operations.ShiftRight((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.LogicalAnd:
                {
                    if (node.Token.TokenValue == "&&")
                    {
                        return Operations.LogicalAnd((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.LogicalOr:
                {
                    if (node.Token.TokenValue == "||")
                    {
                        return Operations.LogicalOr((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.Assignment:
                {
                    if (node.Token.TokenValue.ToString() == "=")
                    {
                        return Operations.Assign((Nodes.Diadic) node);
                    }

                    switch (node.Token.TokenValue)
                    {
                        case "+=":
                            return Operations.AddAssign((Nodes.Diadic) node);
                        case "-=":
                            return Operations.SubtractAssign((Nodes.Diadic) node);
                        case "*=":
                            return Operations.MultiplyAssign((Nodes.Diadic) node);
                        case "/=":
                            return Operations.DivideAssign((Nodes.Diadic) node);
                        case "%=":
                            return Operations.ModulusAssign((Nodes.Diadic) node);
                        case "<<=":
                            return Operations.LeftShiftAssign((Nodes.Diadic) node);
                        case ">>=":
                            return Operations.RightShiftAssign((Nodes.Diadic) node);
                        case "&=":
                            return Operations.BitwiseAndAssign((Nodes.Diadic) node);
                        case "^=":
                            return Operations.BitwiseXorAssign((Nodes.Diadic) node);
                        case "|=":
                            return Operations.BitwiseOrAssign((Nodes.Diadic) node);
                        case "&&=":
                            return Operations.LogicalAndAssign((Nodes.Diadic) node);
                        case "||=":
                            return Operations.LogicalOrAssign((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.Comparison:
                {
                    if (node.Token.TokenValue.ToString().Length == 1)
                    {
                        switch ((char) node.Token.TokenValue)
                        {
                            case '<':
                                return Operations.LessThan((Nodes.Diadic) node);
                            case '>':
                                return Operations.MoreThan((Nodes.Diadic) node);
                        }
                    }
                    else
                    {
                        switch (node.Token.TokenValue)
                        {
                            case "<=":
                                return Operations.LessThan((Nodes.Diadic) node) ||
                                       Operations.CheckEquality((Nodes.Diadic) node);
                            case ">=":
                                return Operations.MoreThan((Nodes.Diadic) node) ||
                                       Operations.CheckEquality((Nodes.Diadic) node);
                        }
                    }

                    break;
                }
                case Token.TokenTypes.Equality:
                {
                    switch (node.Token.TokenValue)
                    {
                        case "==":
                            return Operations.CheckEquality((Nodes.Diadic) node);
                        case "!=":
                            return !Operations.CheckEquality((Nodes.Diadic) node);
                    }

                    break;
                }
                case Token.TokenTypes.Unary:
                {
                    switch (node.Token.TokenValue)
                    {
                        case '!':
                            return Operations.Not((Nodes.Monadic) node);
                        case '-':
                            return Operations.Negative((Nodes.Monadic) node);
                        case '~':
                            return Operations.BitwiseNot((Nodes.Monadic) node);
                    }

                    break;
                }
                default:
                    return null;
            }

            return null;
        }
    }
}