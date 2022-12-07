using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler.Tokens
{
    public enum type_const 
    {
        integer,
        real,
        boolean,
        @char
    }

    class ConstToken : Token
    {
        public type_const t_const { get; }
        public string value;
        public ConstToken(type_const t_const, Position position, string value) : base(position, type)
        {
            this.t_const = t_const;
            type = Token_type.CONST;
            this.value = value;
        }
        public override string ToString()
        {

            return Convert.ToString(type) + '-' + Convert.ToString(t_const);
        }
    }
}
