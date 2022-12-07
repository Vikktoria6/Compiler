using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler.Tokens
{
    class IdentToken : Token
    {
        public string ident;
        public IdentToken(string ident, Position position) : base(position, type)
        {
            this.ident = ident;
            type = Token_type.IDENT;
        }
        public override string ToString()
        {
            return Convert.ToString(type);
        }
    }
}
