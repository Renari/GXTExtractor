using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXTExtractor
{
    class InvalidGXTException : Exception
    {
        public InvalidGXTException() : base("The provided file may not have been a valid gtx file"){ }
    }
}
