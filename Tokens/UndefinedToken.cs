using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler.Tokens
{
    class UndefinedToken : Token
    {
        public string unknow;
        public UndefinedToken(string unknow, Position position) : base(position, type)
        {
            type = Token_type.UNDEFINED;
            this.unknow = unknow;
        }
        public override string ToString()
        {
            return Convert.ToString(type);
        }
    }
}
