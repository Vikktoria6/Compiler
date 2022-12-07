using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    enum Token_type
    {
        KEYWORD,
        IDENT,
        CONST,
        UNDEFINED
    }
    abstract class Token
    {
        public Position Position { get; }
        public static Token_type type;
        protected Token(Position position, Token_type type)
        {
            Position = new Position(position);
            type = Token_type.UNDEFINED;
        }

        

    }
}
