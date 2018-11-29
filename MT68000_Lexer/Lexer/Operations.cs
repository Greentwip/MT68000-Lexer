using System;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class Operations.
    /// </summary>
    internal class Operations
    {
        /// <summary>
        ///     Applies a Addition operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object Add(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return string.Concat((string) node.LHS.EvaluatedValue,
                                             (string) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) +
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Addition operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a Subtraction operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>System.<see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object Subtract(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return ((string) node.LHS.EvaluatedValue).Replace(
                            (string) node.RHS.EvaluatedValue, "");
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) -
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Subtraction operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a Multiplication operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object Multiply(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(int)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return Helper.DuplicateString((string) node.RHS.EvaluatedValue,
                                                      (int) node.LHS.EvaluatedValue);
                    }

                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(int)))
                    {
                        Helper.DuplicateString((string) node.LHS.EvaluatedValue,
                                               (int) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) *
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Multiplication operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a Division operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="DivideByZeroException"></exception>
        internal static object Divide(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return string.Concat((string) node.LHS.EvaluatedValue,
                                             (string) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Convert.ToDouble(node.RHS.EvaluatedValue) == 0)
                    {
                        throw new DivideByZeroException("Divide by zero error");
                    }

                    return Convert.ToDouble(node.LHS.EvaluatedValue) /
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Division operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="Modulus" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="DivideByZeroException"></exception>
        internal static object Modulus(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return string.Concat((string) node.LHS.EvaluatedValue,
                                             (string) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Convert.ToDouble(node.RHS.EvaluatedValue) == 0)
                    {
                        throw new DivideByZeroException("Divide by zero error");
                    }

                    return Convert.ToDouble(node.LHS.EvaluatedValue) %
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Modulus operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="BitwiseAnd" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception"></exception>
        internal static object BitwiseAnd(Nodes.Diadic node)
        {
            try
            {
                return Convert.ToInt64(node.LHS.EvaluatedValue) &
                       Convert.ToInt64(node.RHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Bitwise AND operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="BitwiseOr" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception"></exception>
        internal static object BitwiseOr(Nodes.Diadic node)
        {
            try
            {
                return Convert.ToInt64(node.LHS.EvaluatedValue) |
                       Convert.ToInt64(node.RHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Bitwise OR operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="BitwiseXor" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception"></exception>
        internal static object BitwiseXor(Nodes.Diadic node)
        {
            try
            {
                return Convert.ToInt64(node.LHS.EvaluatedValue) ^
                       Convert.ToInt64(node.RHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Bitwise XOR operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="ShiftLeft" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object ShiftLeft(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(int)))
                    {
                        return Helper.ShiftStringLeft((string) node.LHS.EvaluatedValue,
                                                      (int) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToInt64(node.LHS.EvaluatedValue) <<
                           Convert.ToInt32(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Left Shift operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="ShiftRight" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object ShiftRight(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(int)))
                    {
                        return Helper.ShiftStringRight((string) node.LHS.EvaluatedValue,
                                                       (int) node.RHS.EvaluatedValue);
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToInt64(node.LHS.EvaluatedValue) >>
                           Convert.ToInt32(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Right Shift operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="ShiftLeftRemainder" /> operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        internal static void ShiftLeftRemainder(Nodes.Diadic node) { }

        /// <summary>
        ///     Applies a <see cref="ShiftRightRemainder" /> operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        internal static void ShiftRightRemainder(Nodes.Diadic node) { }

        /// <summary>
        ///     Applies a <see cref="LogicalAnd" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool LogicalAnd(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.EvaluatedValue == null)
                {
                    throw new Exception("LHS null value");
                }

                if (node.RHS.EvaluatedValue == null)
                {
                    throw new Exception("RHS null value");
                }

                return Convert.ToBoolean(node.LHS.EvaluatedValue) &&
                       Convert.ToBoolean(node.RHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Logical And operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="LogicalOr" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool LogicalOr(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.EvaluatedValue == null)
                {
                    throw new Exception("LHS null value");
                }

                if (node.RHS.EvaluatedValue == null)
                {
                    throw new Exception("RHS null value");
                }

                return Convert.ToBoolean(node.LHS.EvaluatedValue) ||
                       Convert.ToBoolean(node.RHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Logical And operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies an Assignment operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object Assign(Nodes.Diadic node)
        {
            try
            {
                if (node.RHS.EvaluatedValue == null)
                {
                    throw new Exception("RHS null value");
                }

                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(), node.RHS.EvaluatedValue);
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] = node.RHS.EvaluatedValue;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a AdditionAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object AddAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToDouble(node.LHS.EvaluatedValue) +
                                               Convert.ToDouble(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToDouble(node.LHS.EvaluatedValue) +
                        Convert.ToDouble(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a SubtractionAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object SubtractAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToDouble(node.LHS.EvaluatedValue) -
                                               Convert.ToDouble(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToDouble(node.LHS.EvaluatedValue) -
                        Convert.ToDouble(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a MultiplicationAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object MultiplyAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToDouble(node.LHS.EvaluatedValue) *
                                               Convert.ToDouble(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToDouble(node.LHS.EvaluatedValue) *
                        Convert.ToDouble(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a DivisionAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object DivideAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToDouble(node.LHS.EvaluatedValue) /
                                               Convert.ToDouble(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToDouble(node.LHS.EvaluatedValue) /
                        Convert.ToDouble(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a ModulusAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object ModulusAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToDouble(node.LHS.EvaluatedValue) %
                                               Convert.ToDouble(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToDouble(node.LHS.EvaluatedValue) %
                        Convert.ToDouble(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a LeftShiftAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object LeftShiftAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToInt64(node.LHS.EvaluatedValue) <<
                                               Convert.ToInt32(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToInt64(node.LHS.EvaluatedValue) <<
                        Convert.ToInt32(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a RightShiftAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object RightShiftAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToInt64(node.LHS.EvaluatedValue) >>
                                               Convert.ToInt32(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToInt64(node.LHS.EvaluatedValue) >>
                        Convert.ToInt32(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a BitwiseAndAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object BitwiseAndAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToInt64(node.LHS.EvaluatedValue) &
                                               Convert.ToInt64(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToInt64(node.LHS.EvaluatedValue) &
                        Convert.ToInt64(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a BitwiseOrAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object BitwiseOrAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToInt64(node.LHS.EvaluatedValue) |
                                               Convert.ToInt64(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToInt64(node.LHS.EvaluatedValue) |
                        Convert.ToInt64(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a BitwiseXORAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object BitwiseXorAssign(Nodes.Diadic node)
        {
            try
            {
                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               Convert.ToInt64(node.LHS.EvaluatedValue) ^
                                               Convert.ToInt64(node.RHS.EvaluatedValue));
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        Convert.ToInt64(node.LHS.EvaluatedValue) ^
                        Convert.ToInt64(node.RHS.EvaluatedValue);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a LogicalAndAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object LogicalAndAssign(Nodes.Diadic node)
        {
            try
            {
                if ((node.LHS.EvaluatedType == typeof(bool)) && (node.RHS.EvaluatedType == typeof(bool)))
                {
                    return null;
                }

                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               (bool) node.LHS.EvaluatedValue && (bool) node.RHS.EvaluatedValue);
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        (bool) node.LHS.EvaluatedValue && (bool) node.RHS.EvaluatedValue;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a LogicalOrAssignment operation to the
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static object LogicalOrAssign(Nodes.Diadic node)
        {
            try
            {
                if ((node.LHS.EvaluatedType == typeof(bool)) && (node.RHS.EvaluatedType == typeof(bool)))
                {
                    return null;
                }

                if (node.LHS.Token.TokenType != Token.TokenTypes.Variable)
                {
                    throw new Exception("Variable dictionary error");
                }

                if (!Variables.VariableList.ContainsKey(node.LHS.Value.ToString()))
                {
                    Variables.VariableList.Add(node.LHS.Value.ToString(),
                                               (bool) node.LHS.EvaluatedValue || (bool) node.RHS.EvaluatedValue);
                }
                else
                {
                    Variables.VariableList[node.LHS.Value.ToString()] =
                        (bool) node.LHS.EvaluatedValue || (bool) node.RHS.EvaluatedValue;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Assignment operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="MoreThan" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool MoreThan(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return ((string) node.LHS.EvaluatedValue).Length > ((string) node.RHS.EvaluatedValue).Length;
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) >
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Comparison operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="LessThan" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool LessThan(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return ((string) node.LHS.EvaluatedValue).Length < ((string) node.RHS.EvaluatedValue).Length;
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) <
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Comparison operation exception: " + e);
            }
        }

        /// <summary>
        ///     Checks the equality of the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool CheckEquality(Nodes.Diadic node)
        {
            try
            {
                try
                {
                    if ((node.LHS.EvaluatedType == typeof(string)) && (node.RHS.EvaluatedType == typeof(string)))
                    {
                        return (string) node.LHS.EvaluatedValue == (string) node.RHS.EvaluatedValue;
                    }

                    throw new Exception();
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) ==
                           Convert.ToDouble(node.RHS.EvaluatedValue);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Equality operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="Not" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the operation is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        internal static bool Not(Nodes.Monadic node)
        {
            try
            {
                if (node.LHS.EvaluatedType == typeof(bool))
                {
                    return !(bool) node.LHS.EvaluatedValue;
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception("NOT operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="Negative" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception"></exception>
        internal static object Negative(Nodes.Monadic node)
        {
            try
            {
                try
                {
                    if (node.LHS.EvaluatedType == typeof(string))
                    {
                        var chars = ((string) node.LHS.EvaluatedValue).ToCharArray();
                        Array.Reverse(chars);
                        return chars.ToString();
                    }

                    return Convert.ToDouble(node.LHS.EvaluatedValue) * -1;
                }
                catch
                {
                    return Convert.ToDouble(node.LHS.EvaluatedValue) * -1;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Negative operation exception: " + e);
            }
        }

        /// <summary>
        ///     Applies a <see cref="BitwiseNot" /> operation to the <paramref name="node" />.
        /// </summary>
        /// <param name="node">The <paramref name="node" />.</param>
        /// <returns><see cref="object" />.</returns>
        /// <exception cref="Exception"></exception>
        internal static object BitwiseNot(Nodes.Monadic node)
        {
            try
            {
                return ~Convert.ToInt64(node.LHS.EvaluatedValue);
            }
            catch (Exception e)
            {
                throw new Exception("Bitwise NOT operation exception: " + e);
            }
        }
    }
}