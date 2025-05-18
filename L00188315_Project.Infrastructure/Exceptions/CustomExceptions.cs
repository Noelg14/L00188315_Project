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

    public class BalanceException : Exception
    {
        public BalanceException(string message)
            : base(message) { }
    }

    public class TransactionException : Exception
    {
        public TransactionException(string message)
            : base(message) { }
    }

    public class KeyVaultException : Exception
    {
        public KeyVaultException(string message)
            : base(message) { }
    }
    public class TokenException : Exception
    {
        public TokenException(string message)
            : base(message) { }
    }
    public class AccountException : Exception
    {
        public AccountException(string message)
            : base(message) { }
    }
}
