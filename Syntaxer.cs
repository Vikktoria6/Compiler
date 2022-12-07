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
        
        public Syntaxer(StreamReader reader)
        {
            this.LA = new Lexer(reader);
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
                NextToken();
                Accept(Keyword.OneEqual);
                NextToken();
                if (Waiting(Keyword.Minus)) NextToken();
                Accept(Token_type.CONST);
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
                NextToken();
                while (Waiting(Keyword.Comma))
                {
                    NextToken();
                    Accept(Token_type.IDENT);
                    NextToken();
                }
                Accept(Keyword.Colon);
                NextToken();
                AcceptType(curToken);
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
            else if (Waiting(Token_type.IDENT)) NextToken();
            else if (Waiting(Token_type.CONST)) NextToken();
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
            while (Waiting(Token_type.IDENT))
            {
                NextToken();
                Accept(Keyword.Assign);
                NextToken();
                Expression();
            }
        }
        private void Block()
        {
            NextToken();
            if (Waiting(Keyword.Constant)) Constants();
            if (Waiting(Keyword.Var)) Variables();
            Accept(Keyword.Begin);
            Operator();
            while (Waiting(Keyword.Semicolon)) Operator();
            Accept(Keyword.End);
            NextToken();
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
