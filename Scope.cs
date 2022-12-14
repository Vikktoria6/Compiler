using System;
using System.Collections.Generic;
using System.Text;
using FG_Compiler.Tokens;

namespace FG_Compiler
{
    class Scope
    {
        private Dictionary<string, type_const> table = new Dictionary<string, type_const>();

        public bool ExistIden(string id)
        {
            return table.ContainsKey(id);
        }

        public type_const GetTypeIdent(string id)
        {
            return table[id];
        }

        public type_const DefineType(Token t)
        {
            if (t is KeyWordToken kw_tok)
                switch (kw_tok.kw)
                {
                    case Keyword.IntegerNumber:
                        return type_const.integer;
                    case Keyword.RealNumber:
                        return type_const.real;
                    case Keyword.Char:
                        return type_const.@char;
                    case Keyword.Boolean:
                        return type_const.boolean;
                }
            else if (t is ConstToken con_tok) return con_tok.t_const;
            else if (t is IdentToken id_tok) return GetTypeIdent(id_tok.ident);
            return type_const.integer;
        }
        public bool AddIdent(List <string> id, type_const type, Position pos)
        {
            bool a = false;
            foreach (var i in id)
                if (!ExistIden(i))
                {
                    table.Add(i, type);
                    Console.WriteLine(" {0} {1}", i, type);
                    a = true;
                }
            return a;
        }

        public bool AllowedTypes (List <type_const> list_types, type_const type)
        {
            if (type == type_const.real)
            {
                foreach (var i in list_types)
                    if (i != type_const.integer && i != type_const.real)
                        return false;
            }
            else
            {
                foreach (var i in list_types)
                    if (i != type)
                        return false;
            }
            return true;
        }
    }
}
