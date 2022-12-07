using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    class Error
    {

        //enum errcode + errcodemassage

        public Position errorPosition = new Position();
        public string errorMsg;

        public Error(Position errorPosition, string errorMsg)
        {
            this.errorPosition = errorPosition;
            this.errorMsg = errorMsg;
        }

        public override string ToString()
        {
            return errorMsg + " " + errorPosition;           
        }
    }
}
