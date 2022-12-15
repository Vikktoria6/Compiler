using System;
using System.Collections.Generic;
using System.Text;

namespace FG_Compiler
{
    class Error : Exception
    {

        //enum errcode + errcodemassage

        private Position errorPosition = new Position();
        private string errorMsg;
        private string bit;

        public Error(Position errorPosition, string errorMsg)
        {
            this.errorPosition = errorPosition;
            this.errorMsg = errorMsg;
        }
        public Error(Position errorPosition, string errorMsg, string bit) 
        {
            this.errorPosition = errorPosition;
            this.errorMsg = errorMsg;
            this.bit = bit;
        }

        public override string ToString()
        {
            return errorMsg + " " + errorPosition + " " + bit;           
        }
    }
}
