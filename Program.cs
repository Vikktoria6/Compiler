using System;
using System.IO;
using System.Collections.Generic;

namespace FG_Compiler
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string path = "C:/Users/user/OneDrive/Рабочий стол/Pascal.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                Syntaxer syntax = new Syntaxer(reader);
                syntax.StartBNF();
            }
            
        }
    }
}
