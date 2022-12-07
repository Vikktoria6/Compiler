using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    class Position     //расположение
    {
        public int pos_line; // номер строки
        public int pos_char; // номер позиции в строке

        public Position(int line = 1, int ch = 0)
        {
            pos_line = line;
            pos_char = ch;
        }
        public Position(Position position)
        {
            pos_line = position.pos_line;
            pos_char = position.pos_line;
        }
    }
}
