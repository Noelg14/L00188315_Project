using System.Transactions;
using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Models;

namespace L00188315_Project.Infrastructure.Services.Mapper
{
    /// <summary>
    /// Helper lass to map Open Banking data models to Core entities.
    /// </summary>
    public class OpenBankingMapper
    {
        /// <summary>
        /// Maps the Open Banking account model to the Core Account entity.
        /// </summary>
        /// <param name="modelAccount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Account MapToAccountEntity(OBAccount modelAccount, string userId)
        {
            return new Account
            {
                AccountId = modelAccount.AccountId!,
                AccountSubType = modelAccount.AccountSubType!,
                AccountType = modelAccount.AccountType!,
                Currency = modelAccount.Currency!,
                Iban =
                    modelAccount._Account?.Find(x => x.SchemeName == "UK.OBIE.IBAN")?.Identification
                    ?? string.Empty,
                Name = modelAccount._Account?.FirstOrDefault()?.Name ?? "Account",
                SortCode =
                    modelAccount
                        ._Account?.Find(x => x.SchemeName == "UK.OBIE.SortCodeAccountNumber")
                        ?.Identification ?? string.Empty,
                UserId = userId,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };
        }

        /// <summary>
        /// Maps the Open Banking balance model to the Core Balance entity.
        /// </summary>
        /// <param name="modelBalance"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Balance MapToBalanceEntity(OBBalance modelBalance, string accountId)
        {
            return new Balance
            {
                //AccountId = accountId,
                Amount = modelBalance.Amount!._Amount,
                BalanceType = modelBalance.Type,
                Currency = modelBalance.Amount.Currency,
                LastUpdated = DateTime.TryParse(modelBalance.DateTime, out _)
                    ? DateTime.Parse(modelBalance.DateTime)
                    : DateTime.Now,
            };
        }

        /// <summary>
        /// Maps the Open Banking transaction model to the Core Transaction entity.
        /// </summary>
        /// <param name="modelTransaction"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Core.Entities.Transaction MapToTransactionEntity(
            OBTransaction modelTransaction,
            string accountId
        )
        {
            return new Core.Entities.Transaction
            {
                Amount = modelTransaction.Amount?._Amount,
                AmountCurrency = modelTransaction.Amount?.Currency,
                TransactionInformation = modelTransaction.TransactionInformation,
                TransactionId = modelTransaction.TransactionId,
                CreditDebitIndicator = modelTransaction.CreditDebitIndicator,
                BookingDateTime = modelTransaction.BookingDateTime,
                ValueDateTime = modelTransaction.ValueDateTime,
                CreditorAccount = modelTransaction.CreditorAccount?.Identification ?? string.Empty,
                DebtorAccount = modelTransaction.DebtorAccount?.Identification ?? string.Empty,
                ProprietaryBankTransactionCode = modelTransaction
                    .ProprietaryBankTransactionCode
                    ?.Code,
                Status = modelTransaction.Status,
                UserComments = modelTransaction.SupplementaryData?.UserComments ?? string.Empty,
            };
        }
    }
}
