using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FG_Compiler.Tokens;

namespace FG_Compiler
{
    class Syntaxer
    {
        private Lexer LA;
        private Token curToken;
        private List <string> curIdent = new List<string>();
        private List<type_const> ListCurTypes = new List<type_const>();
        private Scope scp = new Scope();
        private type_const cur_type = type_const.integer;
        
        public Syntaxer(StreamReader reader)
        {
            this.LA = new Lexer(reader);
        }

        private void CheckIdent()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (!scp.ExistIden(cur_tok.ident)) curIdent.Add(cur_tok.ident);
                else throw new Error(cur_tok.Position, "Сeмантическая ошибка", cur_tok.ident);
            }
            
        }

        private void SearchType()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (scp.ExistIden(cur_tok.ident)) ListCurTypes.Add(scp.DefineType(curToken));
                else throw new Error(cur_tok.Position, "Сeмантическая ошибка", cur_tok.ident);
            }
            else ListCurTypes.Add(scp.DefineType(curToken));
        }


        private void AddIdentIntoScope()
        {
            if (curToken is ConstToken cur_tok)
            {
                scp.AddIdent(curIdent, cur_tok.t_const, cur_tok.Position);
            }
            else if (curToken is KeyWordToken cur_tok2)
            {
                type_const tc = scp.DefineType(curToken);
                scp.AddIdent(curIdent, tc, cur_tok2.Position);
            }
            curIdent.Clear();
        }

        public void SetCurType()
        {
            if (curToken is IdentToken cur_tok)
            {
                if (scp.ExistIden(cur_tok.ident)) cur_type = scp.GetTypeIdent(cur_tok.ident);
                else throw new Error(cur_tok.Position, "Сeмантическая ошибка", cur_tok.ident);
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
        private void Accept(Keyword kw)
        {
            if (!Waiting(kw))
                throw new Error(curToken.Position, "Syntaxer error");
        }

        private bool Waiting(Token_type type)
        {
            return (curToken.Type() == type && curToken != null);
        }

        private void Accept(Token_type type)
        {
            if (!Waiting(type))
                throw new Error(curToken.Position, "Syntaxer error");
        }

        private void AcceptType(Token kwtoken)
        {
            if (!Waiting(Keyword.Boolean) && !Waiting(Keyword.IntegerNumber) && !Waiting(Keyword.RealNumber) && !Waiting(Keyword.Char))
                throw new Error(curToken.Position, "Syntaxer error");
            
           
        }
        private void Constants()
        {
            NextToken();
            while (Waiting(Token_type.IDENT))
            {
                CheckIdent();      
                NextToken();
                Accept(Keyword.OneEqual);
                NextToken();
                if (Waiting(Keyword.Minus)) NextToken();
                Accept(Token_type.CONST);
                AddIdentIntoScope();
                NextToken();
                Accept(Keyword.Semicolon);
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
                    Accept(Token_type.IDENT);
                    CheckIdent();
                    NextToken();
                }
                Accept(Keyword.Colon);
                NextToken();
                AcceptType(curToken);
                AddIdentIntoScope();
                NextToken();
                Accept(Keyword.Semicolon);
                NextToken();
            };
        }

        private void Multiplier()
        {
            if (Waiting(Keyword.OpenBracket))
            {
                NextToken();
                Expression();
                Accept(Keyword.CloseBracket);
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
            else throw new Error(curToken.Position, "Syntaxer error");
            
        }
        private void Term()
        {
            Multiplier();
            while (Waiting(Keyword.Multiply) || Waiting(Keyword.Divide))
            {
                NextToken();
                Multiplier();
            }
        }
        private void Expression()
        {
            if (Waiting(Keyword.Minus)) NextToken();
            Term();
            while (Waiting(Keyword.Plus) || Waiting(Keyword.Minus))
            {
                NextToken();
                Term();
            }

        }
        private void Operator()
        {
            NextToken();
            if (Waiting(Keyword.Begin)) SostOperator();
            else
            {
                
                while (Waiting(Token_type.IDENT))
                {
                    ListCurTypes.Clear();
                    SetCurType();
                    NextToken();
                    Accept(Keyword.Assign);
                    NextToken();
                    Expression();
                    scp.AllowedTypes(ListCurTypes, cur_type);
                    
                }
            }
        }

        private void SostOperator()
        {
            Accept(Keyword.Begin);
            Operator();
            while (Waiting(Keyword.Semicolon)) Operator();
            Accept(Keyword.End);
            NextToken();
        }
        private void Block()
        {
            NextToken();
            if (Waiting(Keyword.Constant)) Constants();
            if (Waiting(Keyword.Var)) Variables();
            SostOperator();
            Accept(Keyword.Dot);
        }
        private void Prog()
        {
            Accept(Keyword.Program);
            NextToken();
            Accept(Token_type.IDENT);
            NextToken();
            Accept(Keyword.Semicolon);
            Block();
        }

        public void StartBNF()
        {
            NextToken();
            Prog();
        }
      
    }
}
