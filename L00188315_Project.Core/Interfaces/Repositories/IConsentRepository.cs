using L00188315_Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Interfaces.Repositories;

public interface IConsentRepository
{
    public Task<Consent> GetConsentAsync(string consentId);
    public Task<IReadOnlyList<Consent>> GetAllConsentsAsync(string userId);
    public Task<Consent> CreateConsentAsync(Consent consent);
    public Task<Consent> UpdateConsentAsync(Consent consent, ConsentStatus? status);
}
