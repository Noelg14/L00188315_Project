using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Exceptions
{
    public class TokenNullException : Exception
    {
        public TokenNullException(string message)
            : base(message) { }
    }

    public class ConsentException : Exception
    {
        public ConsentException(string message)
            : base(message) { }
    }
}
