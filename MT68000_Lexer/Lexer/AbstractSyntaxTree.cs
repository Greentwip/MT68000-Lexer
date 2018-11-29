using System;
using System.Collections.Generic;
using System.Globalization;

namespace MT68000_Lexer.Lexer
{
    /// <summary>
    ///     Class <see cref="AbstractSyntaxTree"/>.
    /// </summary>
    public class AbstractSyntaxTree
    {
        /// <summary>
        ///     The nodes
        /// </summary>
        public List<Nodes.Node> Nodes = new List<Nodes.Node>();

        /// <summary>
        ///     The root node
        /// </summary>
        public Nodes.Node RootNode => Nodes[0];

        /// <summary>
        ///     Parses the expression and evaluates the root node.This will recursively evaluate all child nodes
        /// </summary>
        /// <returns>System.<see cref="string" />.</returns>
        public string Evaluate()
        {
            Console.WriteLine("Expression Evaluation started...");

            try
            {
                var result = Nodes[0].Evaluate();
                Console.WriteLine("Expression Evaluation complete.");
                if (result is double || result is long || result is int)
                {
                    return Convert.ToDouble(result).ToString(CultureInfo.InvariantCulture);
                }

                return result is bool ? Convert.ToBoolean(result).ToString() : result.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}