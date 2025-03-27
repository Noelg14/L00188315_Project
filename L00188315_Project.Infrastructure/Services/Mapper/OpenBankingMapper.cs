using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Services.Mapper
{
    public class OpenBankingMapper
    {
        public Core.Entities.Account MapToAccountEntity(Core.Models.Account modelAccount,string userId)
        {
            return new Core.Entities.Account
            {
                AccountId = modelAccount.AccountId,
                AccountSubType = modelAccount.AccountSubType,
                AccountType = modelAccount.AccountType,
                Currency = modelAccount.Currency,
                Iban = modelAccount._Account?.Find(x => x.SchemeName == "UK.OBIE.IBAN")?.Identification ?? string.Empty,
                Name = modelAccount._Account?.FirstOrDefault()?.Name ?? "Account",
                SortCode = modelAccount._Account?.Find(x => x.SchemeName == "UK.OBIE.SortCodeAccountNumber")?.Identification ?? string.Empty,
                UserId = userId,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };
        }
    }
}
