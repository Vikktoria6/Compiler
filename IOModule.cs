using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FG_Compiler
{

    
    class IOModule
    {
        public List<Error> errors = new List<Error>();
        private StreamReader reader;
        public Position currentTextPos = new Position();
        private char currentChar;
        public bool endOfFile = true;
        private string line;
        public string lexeme;
        private char sym;
        private char[] doubleSym = { '+', '-', '*', '/', ':', '=', '>', '<', ';', '(', ')', ',', '{', '}', '.' };

        public IOModule(StreamReader reader)
        {
            this.reader = reader;
            line = reader.ReadLine();
            currentTextPos.pos_line = 1;
            currentTextPos.pos_char = 0;
        }

        public void printError()
        {
            foreach (var err in errors)
            {
                Console.WriteLine(err);
            }
        }
        private bool CheckDoubleSymbol() {
            char nextSym = 'q';
            if (line != null)
            {
                if (currentTextPos.pos_char < line.Length)
                {
                    sym = line[currentTextPos.pos_char];
                    if (currentTextPos.pos_char + 1 < line.Length)
                        nextSym = line[currentTextPos.pos_char + 1];
                    
                    if (sym == '.' && (int.TryParse(nextSym.ToString(), out int a)
                        || int.TryParse(currentChar.ToString(), out int b)))
                        return false;
                    foreach (var symbol in doubleSym)
                        if (sym == symbol) return true;
                }
            }
            return false;
        }

        private bool isDoubleSymbol()
        {
            char nextSym;
            if (line == null) return false;
            if (currentTextPos.pos_char < line.Length)
                nextSym = line[currentTextPos.pos_char];
            else nextSym = 'q';
            foreach (var symbol in doubleSym)
                if (currentChar == symbol) { 
                    lexeme = "" + currentChar; 
                        if (((lexeme == ":" || lexeme == ">" || lexeme == "<") && nextSym == '=')
                            || (lexeme == "<" && nextSym == '>')
                                || (lexeme == "/" && nextSym == '/'))
                        {
                            nextSym = GetNextChar();
                            lexeme = lexeme + nextSym;
                            if (lexeme == "//")
                        {
                            
                            line = reader.ReadLine();
                            currentTextPos.pos_line++;
                            currentTextPos.pos_char = 0;
                            if (line == null)
                            {
                                endOfFile = false;
                            }
                            return false;
                        }
                        }
                    return true; 
                }
            return false;
        }
        private char GetNextChar()           //возвращает по одному символу
        {
            char cur_char;
            
            if (currentTextPos.pos_char >= line.Length)
            {
                line = reader.ReadLine();
                currentTextPos.pos_line++;
                currentTextPos.pos_char = 0;
                if (line == null)
                {
                    endOfFile = false;
                }
                return ' ';
            }
            cur_char = line[currentTextPos.pos_char];
            currentTextPos.pos_char++;
            return cur_char;
        }
        public string GetLexeme()           //возвращает лексемы в Lexer
        {
            currentChar = GetNextChar();
            while (currentChar == ' ' && endOfFile) currentChar = GetNextChar();
            Lexer.pos = new Position(currentTextPos.pos_char - 1, currentTextPos.pos_line);
            if (isDoubleSymbol())
            {
                if (lexeme == "//") currentChar = GetNextChar();
                else return lexeme;
            }
            lexeme = "";
            while (currentChar != ' ' )
            {
                lexeme = lexeme + currentChar;
                
                if (CheckDoubleSymbol()) break;
                
                currentChar = GetNextChar();
                
            }
            return lexeme;
        }
    }
}
