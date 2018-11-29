using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT68000_Lexer.Lexer
{
    public class Registers
    {
        public enum AddressRegisters
        {
            a0,
            a1,
            a2,
            a3,
            a4,
            a5,
            a6,
            a7
        }

        public enum DataRegisters
        {
            d0,
            d1,
            d2,
            d3,
            d4,
            d5,
            d6,
            d7
        }

        public enum OtherRegisters
        {
            PC,     //Program Counter
            SP,     //Stack Pointer
            SR,     //Status Register
            CCR,    //Code Condition Register
        }
    }
}
