using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Exceptions
{
    /// <summary>
    /// Custom TokenNullException
    /// </summary>
    public class TokenNullException : Exception
    {
        public TokenNullException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom ConsentException
    /// </summary>
    public class ConsentException : Exception
    {
        public ConsentException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom BalanceException
    /// </summary>
    public class BalanceException : Exception
    {
        public BalanceException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom TransactionException
    /// </summary>
    public class TransactionException : Exception
    {
        public TransactionException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom KeyVaultException
    /// </summary>
    public class KeyVaultException : Exception
    {
        public KeyVaultException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom TokenException
    /// </summary>
    public class TokenException : Exception
    {
        public TokenException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Custom AccountException
    /// </summary>
    public class AccountException : Exception
    {
        public AccountException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Failure Reason Messages
    /// </summary>
    public static class FailureReason
    {
        public static readonly string USER_ID_NULL = "UserId is null";
        public static readonly string ACCOUNT_ID_NULL = "AccountId is null";
        public static readonly string CONSENT_ID_NULL = "ConsentId is null";
        public static readonly string TOKEN_NULL_RELINK = "Cannot Get Data from Revolut. Please relink account to update";
    }
}
