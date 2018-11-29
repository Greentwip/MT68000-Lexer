using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class Parser.
    /// </summary>
    public class Parser
    {
        /// <summary>
        ///     The lexical analyser
        /// </summary>
        public static LexicalAnalyser LexicalAnalyser = new LexicalAnalyser();

        /// <summary>
        ///     The syntactical analyser
        /// </summary>
        public static SyntacticalAnalyser SyntacticalAnalyser = new SyntacticalAnalyser();

        /// <summary>
        ///     The expression evaluator
        /// </summary>
        public static ExpressionEvaluation ExpressionEvaluation = new ExpressionEvaluation();

        /// <summary>
        ///     Parses the specified ASM string.
        /// </summary>
        /// <param name="asm">The ASM string.</param>
        /// <returns>System.<see cref="string" />.</returns>
        /// <exception cref="Exception">
        /// </exception>
        public static string Parse(string asm)
        {
            try
            {
                var tok = LexicalAnalyser.AnalyseString (asm);

                if (tok == null)
                {
                    throw new Exception ("Lexical Analysis failure: ");
                }

                var ast = SyntacticalAnalyser.ParseExpression (tok);
                if (ast == null)
                {
                    throw new Exception ("Syntax Analysis failure: ");
                }

                return ast.Evaluate ();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        ///     Gets the tree.
        /// </summary>
        /// <returns><see cref="AbstractSyntaxTree" />.</returns>
        public AbstractSyntaxTree GetTree()
        {
            return SyntacticalAnalyser.GetTree();
        }

        /// <summary>
        ///     Checks the syntax.
        /// </summary>
        /// <param name="asm">The ASM string.</param>
        /// <returns>
        ///     <c>
        ///         <see langword="true" />
        ///     </c>
        ///     if the syntax check is successful,
        ///     <c>
        ///         <see langword="false" />
        ///     </c>
        ///     otherwise.
        /// </returns>
        public bool CheckSyntax(string asm)
        {
            try
            {
                var tok = new List<Token>(); //LexicalAnalyser.Analyse(asm);
                if (tok == null)
                {
                    return false;
                }

                var ast = SyntacticalAnalyser.ParseExpression(tok);

                foreach (var astNode in ast.Nodes)
                {
                    if (astNode.GetType() == typeof(Nodes.Error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}