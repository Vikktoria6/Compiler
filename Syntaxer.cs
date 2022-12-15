using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FG_Compiler.Tokens;

namespace FG_Compiler
{
    class Syntaxer
    {
        public Lexer LA;
        private Token curToken;
        private List <string> curIdent = new List<string>();
        private List<type_const> ListCurTypes = new List<type_const>();
        private Scope scp = new Scope();
        private type_const cur_type = type_const.integer;
        private List<Keyword> starters = new List<Keyword>();
        private List<Keyword> followers = new List<Keyword>();
        
        public Syntaxer(StreamReader reader)
        {
            this.LA = new Lexer(reader);
        }

        private bool IsBelong(List <Keyword> set)
        {
            return (curToken is KeyWordToken cur_tok && set.Contains(cur_tok.kw));
        }

        private void Skip()

        {
            while (curToken != null && !IsBelong(starters) && !IsBelong(followers))
            {
                NextToken();
            }
        }

        private void CheckIdent()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (!scp.ExistIden(cur_tok.ident)) curIdent.Add(cur_tok.ident);
                else LA.ioMod.errors.Add(new Error(cur_tok.Position, "Семантическая ошибка", cur_tok.ident));
            }
            
        }

        private void SearchType()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (scp.ExistIden(cur_tok.ident)) ListCurTypes.Add(scp.DefineType(curToken));
                else LA.ioMod.errors.Add(new Error(cur_tok.Position, "Семантическая ошибка", cur_tok.ident));
            }
            else ListCurTypes.Add(scp.DefineType(curToken));
        }


        private void AddIdentIntoScope()
        {
            if (curToken is ConstToken cur_tok)
            {
                if (!scp.AddIdent(curIdent, cur_tok.t_const, cur_tok.Position))
                    LA.ioMod.errors.Add(new Error(cur_tok.Position, "Существующий идентификатор"));
                        
            }
            else if (curToken is KeyWordToken cur_tok2)
            {
                type_const tc = scp.DefineType(curToken);
                if (!scp.AddIdent(curIdent, tc, cur_tok2.Position))
                    LA.ioMod.errors.Add(new Error(cur_tok2.Position, "Существующий идентификатор"));
            }
            curIdent.Clear();
        }

        public void SetCurType()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (scp.ExistIden(cur_tok.ident)) cur_type = scp.GetTypeIdent(cur_tok.ident);
                else LA.ioMod.errors.Add(new Error(cur_tok.Position, "Несуществующий идентификатор", cur_tok.ident));
            }

        }
        private void NextToken()
        {
            if (LA.ioMod.endOfFile) curToken = LA.GetToken();
            else curToken = null;
        }

        private bool Waiting(Keyword kw)
        {
            return (curToken is KeyWordToken cur_tok && cur_tok.kw == kw);
        }
        private bool Accept(Keyword kw)
        {
            if (!Waiting(kw))
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Отсутствует символ", Convert.ToString(kw)));
                return false;
            }
            else return true;
        }

        private bool Waiting(Token_type type)
        {
            return (curToken.Type() == type && curToken != null);
        }

        private bool Accept(Token_type type)
        {
            if (!Waiting(type))
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Отсутствует символ", Convert.ToString(type)));
                return false;
            }
            else return true;
        }

        private void AcceptType(Token kwtoken)
        {
            if (!Waiting(Keyword.Boolean) && !Waiting(Keyword.IntegerNumber) && !Waiting(Keyword.RealNumber) && !Waiting(Keyword.Char))
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка"));
                
            }


        }
        private void Constants()
        {
            NextToken();
            while (Waiting(Token_type.IDENT))
            {
                CheckIdent();      
                NextToken();
                if (Accept(Keyword.OneEqual))
                    NextToken();
                if (Waiting(Keyword.Minus)) NextToken();
                if (Accept(Token_type.CONST))
                {
                    AddIdentIntoScope();
                    NextToken();
                }
                if (Accept(Keyword.Semicolon))
                    NextToken();
            };
        }
        private void Variables()
        {
            NextToken();
            while (Waiting(Token_type.IDENT))
            {
                CheckIdent();
                NextToken();
                while (Waiting(Keyword.Comma))
                {
                    NextToken();
                    CheckIdent();
                    Accept(Token_type.IDENT);
                    NextToken();
                }
                if (Accept(Keyword.Colon))
                    NextToken();
                AcceptType(curToken);
                AddIdentIntoScope();
                NextToken();
                if (Accept(Keyword.Semicolon))
                    NextToken();
                
            };
        }

        private void Multiplier()
        {
            starters = new List<Keyword> { Keyword.OpenBracket };
            if (IsBelong(starters) || curToken is ConstToken || curToken is IdentToken)
            {
                if (Waiting(Keyword.OpenBracket))
                {
                    NextToken();
                    Expression();
                    if (Accept(Keyword.CloseBracket))
                        NextToken();

                }
                else if (Waiting(Token_type.IDENT))
                {
                    SearchType();
                    NextToken();

                }
                else if (Waiting(Token_type.CONST))
                {
                    SearchType();
                    NextToken();
                }
            }
            else
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка"));
                Skip();
            }

        }
        private void Term()
        {
            starters = new List<Keyword> { Keyword.Multiply, Keyword.Divide };
            Multiplier();
            {
                while (Waiting(Keyword.Multiply) || Waiting(Keyword.Divide))
                {
                    NextToken();
                    Multiplier();
                }
            }
        }
        private void Expression()
        {
            if (Waiting(Keyword.Minus)) NextToken();
            Term();
            starters = new List<Keyword> { Keyword.Minus, Keyword.Plus, Keyword.Semicolon, Keyword.CloseBracket };
            followers = new List<Keyword> { Keyword.Semicolon };
            if (IsBelong(starters) || curToken is IdentToken )
            {
                while (Waiting(Keyword.Plus) || Waiting(Keyword.Minus))
                {
                    NextToken();
                    Term();
                }
            }
            else
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка"));
                Skip();
            }

        }
        private void Operator()
        {
            if (Waiting(Keyword.Begin)) SostOperator();
            else
            {
                while (Waiting(Token_type.IDENT))
                {
                    ListCurTypes.Clear();
                    SetCurType();
                    NextToken();
                    if (Accept(Keyword.Assign))
                        NextToken();
                    Expression();
                    if (!scp.AllowedTypes(ListCurTypes, cur_type))
                        LA.ioMod.errors.Add(new Error(curToken.Position, "Несоответствие типов"));
                    if (Accept(Keyword.Semicolon))
                        NextToken();
                }
            }
        }

        private void SostOperator()
        {
            starters = new List<Keyword> { Keyword.Begin };
            if (IsBelong(starters))
            {
                if (Accept(Keyword.Begin))
                    NextToken();
                Operator();
                while (Waiting(Keyword.Semicolon)) Operator();
                if (Accept(Keyword.End))
                    NextToken();
                
            }
            else
            {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка2", curToken.ToString()));
                Skip();
            }
        }
        private void Block()
        {
            starters = new List<Keyword> { Keyword.Constant, Keyword.Var, Keyword.Begin };
            followers.Add(Keyword.Dot);
            if (IsBelong(starters))
            {
                if (Waiting(Keyword.Constant)) Constants();
                if (Waiting(Keyword.Var)) Variables();
                SostOperator();
            }
            else {
                LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка3", curToken.ToString()));
                Skip();
            }
            Accept(Keyword.Dot);
        }
        private void Prog()
        {
            starters = new List<Keyword> { Keyword.Program };
            if (IsBelong(starters))
            {
                if (Accept(Keyword.Program)) NextToken();
                if (Accept(Token_type.IDENT)) NextToken();
                if (Accept(Keyword.Semicolon))
                    NextToken();
                Block();
            }
            else LA.ioMod.errors.Add(new Error(curToken.Position, "Синтаксическая ошибка4", curToken.ToString()));
        }

        public void StartBNF()
        {
            NextToken();
            Prog();

            //while (LA.ioMod.endOfFile && LA.lexem != ".") NextToken();
        }
      
    }
}
