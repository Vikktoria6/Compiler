using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FG_Compiler
{
    class Syntaxer
    {
        private Lexer LA;
        public Syntaxer(StreamReader reader)
        {
            this.LA = new Lexer(reader);
            while (LA.ioMod.endOfFile)
            {
                LA.GetToken();
            }
        }

    }
}
