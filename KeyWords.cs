using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    public enum Keyword
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Colon,
        Semicolon,
        OneEqual,
        Assign,
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        Comma,
        OpenBracket,
        CloseBracket,
        Dot,
        Program,
        Var,
        Begin,
        End,
        IntegerNumber,
        RealNumber,
        Char,
        Boolean,
        Constant
    }
    class KeyWords
    {
        private static Dictionary<string, Keyword> key_var = new Dictionary<string, Keyword> {
            {"+", Keyword.Plus},
            {"-", Keyword.Minus},
            {"*", Keyword.Multiply},
            {"/", Keyword.Divide},
            {":", Keyword.Colon},
            {";", Keyword.Semicolon},
            {"=", Keyword.OneEqual},
            {":=", Keyword.Assign},
            {"==", Keyword.Equal},
            {"<>", Keyword.NotEqual},
            {">", Keyword.Greater},
            {"<", Keyword.Less},
            {">=", Keyword.GreaterOrEqual},
            {"<=", Keyword.LessOrEqual},
            {",", Keyword.Comma},
            {"(", Keyword.OpenBracket},
            {")", Keyword.CloseBracket},
            {".", Keyword.Dot},
            {"program", Keyword.Program},
            {"var", Keyword.Var},
            {"begin", Keyword.Begin},
            {"end", Keyword.End},
            {"integer", Keyword.IntegerNumber},
            {"real", Keyword.RealNumber},
            {"char", Keyword.Char},
            {"boolean", Keyword.Boolean},
            {"const", Keyword.Constant}
        };

        public static bool IsKW(string s)
        {
            return key_var.ContainsKey(s);
        }

        public static Keyword FindKW(string s)
        {
            return key_var[s];
        }

    }
}
