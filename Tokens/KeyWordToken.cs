using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    
    class KeyWordToken : Token
    {
        public Keyword kw { get; }
      
        public KeyWordToken(Keyword kw, Position position) : base(position, type)
        {
            this.kw = kw; 
            type = Token_type.KEYWORD;
        }

        public override string ToString()
        {
            return Convert.ToString(type);
        }
    }
}
