using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MT68000_Lexer.Lexer;
using MT68000_Lexer.Properties;

namespace MT68000_Lexer
{
    /// <inheritdoc />
    /// <summary>
    /// Class LexerForm.
    /// Implements the <see cref="T:System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="T:System.Windows.Forms.Form" />
    public partial class LexerForm : Form
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MT68000_Lexer.LexerForm" /> class.
        /// </summary>
        public LexerForm() { InitializeComponent(); }

        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            //await Task.Run(ParseDisassembly);
        }

        private Task<bool> ParseDisassembly()
        {
            var lexicalAnalyser = new LexicalAnalyser();
            var tokens = lexicalAnalyser.AnalyseResourceFile(Resources.s2.Split('\n'));

            Invoke(new MethodInvoker(delegate
            {
                dataGridView1.Rows.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear ();
                CleanUpLabels (ref tokens);
                Display (tokens);
            }));
            return Task.FromResult(true);
        }

        private async void button1_Click(object sender, EventArgs e) { await Task.Run(ParseStrings); }

        private Task<bool> ParseStrings()
        {
            Invoke(new MethodInvoker(delegate
            {
                var lexicalAnalyser = new LexicalAnalyser();
                var tokens = lexicalAnalyser.AnalyseResourceFile(richTextBox1.Lines);
                dataGridView1.Rows.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                CleanUpLabels(ref tokens);
                Display(tokens);
            }));
            return Task.FromResult(true);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Task.Run(ParseDisassembly);
            MessageBox.Show("Disassembly loaded");
        }

        private void CleanUpLabels(ref List<Token> tokens)
        {
            foreach(var token in tokens)
            {
                //if(token.TokenType == Token.TokenTypes.Label)
                foreach(var key in Relabelling.LabelDictionary.Keys)
                {
                    if(token.TokenValue == null)
                        continue;
                    if(token.TokenValue.ToString().Contains(key))
                        token.TokenValue = token.TokenValue.Replace(key, Relabelling.LabelDictionary[key]);
                }
            }
        }

        private Task<bool> Display(List<Token> tokens)
        {
            Invoke(new MethodInvoker(delegate
            {
                var EOLComment = false;
                var CommentString = string.Empty;

                for (int index = 0; index < tokens.Count; index++)
                {
                    var token = tokens[index];
                    dataGridView1.Rows.Add (token.TokenType, token.TokenValue);
                    //if (token.TokenType == Token.TokenTypes.Instruction)
                    //    richTextBox3.AppendText ($"{token.TokenValue}\r\n");

                    switch (token.TokenType)
                    {
                        //case Token.TokenTypes.Comment:
                        //if (token.TokenValue.Contains ("\n"))
                        //    richTextBox2.AppendText ("\r\n");
                        //break;
                        case Token.TokenTypes.Label:
                        richTextBox2.AppendText (token.TokenValue);
                        break;
                        case Token.TokenTypes.Eol:
                            if (EOLComment)
                            {
                                richTextBox2.AppendText($"\t;{CommentString}");
                                EOLComment = false;
                                CommentString = string.Empty;
                            }
                            richTextBox2.AppendText ("\r\n");
                        break;
                        case Token.TokenTypes.Instruction when ((index + 1) < tokens.Count) &&
                                                         (tokens[index + 1].TokenType == Token.TokenTypes.AddressingMode):
                            richTextBox2.AppendText ($"\t{token.TokenValue}");
                            EOLComment = true;
                            CommentString = InstructionSet.Instructions[token.TokenValue];
                            string addressing = tokens[index + 1].TokenValue.ToString();
                            CommentString += $" ({InstructionSet.Addressing[addressing.Substring(1, 1)]})";
                        break;
                        case Token.TokenTypes.Instruction:
                            richTextBox2.AppendText ($"\t{token.TokenValue} ");
                            EOLComment = true;
                            CommentString = InstructionSet.Instructions[token.TokenValue];
                        break;
                        case Token.TokenTypes.AddressingMode:
                        richTextBox2.AppendText ($"{token.TokenValue} ");
                        break;
                        default:
                        richTextBox2.AppendText ($"{token.TokenValue}");
                        break;
                    }
                }
            }));
            return Task.FromResult(true);
        }
    }
}