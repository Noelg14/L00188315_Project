using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Exceptions
{
    public class TokenNullException : Exception
    {
        public TokenNullException(string message) : base(message)
        {
        }
    }

}
