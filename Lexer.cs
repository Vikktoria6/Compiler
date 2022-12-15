using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FG_Compiler.Tokens;

namespace FG_Compiler
{
    class Lexer
    {
        public IOModule ioMod;
        private string lexem;
        private Token token;
        static public Position pos;
        public Lexer(StreamReader reader)
        {
            this.ioMod = new IOModule(reader);

        }
        private bool isKeyWord(string lexem)
        {
            return KeyWords.IsKW(lexem);
        }

        private bool isConst(string lexem, ref type_const type)
        {
            if (lexem == "True" || lexem == "False"
                || lexem == "true" || lexem == "false")
            {
                type = type_const.boolean;
                return true;
            }
            else if (lexem.Length == 3 && lexem[0] == '\'' && lexem[2] == '\'')
                {
                    type = type_const.@char;
                    return true;
                }
            else
            if (int.TryParse(lexem, out int a))
            {
                if (a <= 32767)
                {
                    type = type_const.integer;
                    return true;
                }
                else return false;
            }
            else
            {
                bool dot = true;
                if (Convert.ToInt32(lexem[0]) - 48 >= 0 && Convert.ToInt32(lexem[0]) - 48 <= 9)
                {
                    for (int i = 1; i < lexem.Length; i++)
                    {
                        if (lexem[i] == '.')
                        {
                            if ((Convert.ToInt32(lexem[i + 1] - 48) < 0 || Convert.ToInt32(lexem[i + 1]) - 48 > 9) || !dot)
                                return false;
                            else i++;
                            dot = false;
                        }
                        else if (Convert.ToInt32(lexem[i] - 48) < 0 || Convert.ToInt32(lexem[i]) - 48 > 9) return false;
                    }
                    type = type_const.real;
                    return true;
                }
            }
            return false;
        }

        private bool true_symbol(char ch, int i)
        {
            int t = Convert.ToInt32(ch);
            if ((t >= 48 && t <= 57 && i != 0) || (t >= 65 && t <= 90) || (t >= 97 && t <= 122) || (t == 95))
            {
                return true;
            }
            else return false;
        }
        private bool isIdent(string lexem)
        {
            for (int i = 0; i< lexem.Length; i++)
            {
                if (!true_symbol(lexem[i], i)) return false;
            }
            return true;
        }


        public Token GetToken()
        {

            if (ioMod.endOfFile)
            {
                type_const type = type_const.real;
                lexem = ioMod.GetLexeme();
                
                token = null;
                
                if (isKeyWord(lexem))
                {
                    token = new KeyWordToken(KeyWords.FindKW(lexem), pos);
                }
                else if (isConst(lexem, ref type))
                {
                    token = new ConstToken(type, pos, lexem);
                }
                else if (isIdent(lexem))
                {
                    token = new IdentToken(lexem, pos);
                }
                else
                {
                    token = new UndefinedToken(lexem, pos);
                    Error err = new Error(pos, "Неизвестные символы", lexem);
                    ioMod.errors.Add(err);
                }

                if (token != null)
                    Console.WriteLine("lex= {0} -> {1}", lexem, token.ToString());
                else Console.WriteLine("lex= {0} -> ", lexem);
            }

            return token;
        }
    }
}
