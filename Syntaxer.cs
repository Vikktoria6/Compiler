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
            //while (LA.ioMod.endOfFile)
            //{
            //    curToken = LA.GetToken();
            //}
            //LA.ioMod.printError();
        }

        private void NextToken()
        {
            if (LA.ioMod.endOfFile) curToken = LA.GetToken();
            else curToken = null;
        }
        private bool Accept(Keyword kw)
        {
            return (curToken is KeyWordToken cur_tok && cur_tok.kw == kw);
            
        }

        private bool Accept(Token_type type)
        {
            return (curToken.Type() == type && curToken != null);
        }

        private void Constants()
        {

        }
        private void Variables()
        {
            NextToken();
            while (Accept(Token_type.IDENT))
            {
                NextToken();
                if (Accept)
            };
        }

        private void Operators()
        {
            NextToken();
            while (Accept(Token_type.IDENT))
            {
                NextToken();
                Accept(Token_type.CONST);
                NextToken();
            };
        }
        private void Block()
        {
            NextToken();
            if (Accept(Keyword.Constant)) Constants();
            if (Accept(Keyword.Var)) Variables();
            Operators();
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
