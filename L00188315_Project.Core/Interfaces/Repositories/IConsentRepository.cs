using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Core.Interfaces.Repositories;

/// <summary>
/// Repository for managing consents
/// </summary>
public interface IConsentRepository
{
    /// <summary>
    /// Get a consent by its id
    /// </summary>
    /// <param name="consentId"></param>
    /// <returns></returns>
    public Task<Consent> GetConsentAsync(string consentId);

    /// <summary>
    /// Get all consents for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<Consent>> GetAllConsentsAsync(string userId);

    /// <summary>
    /// Get all consents for a user by account id
    /// </summary>
    /// <param name="consent"></param>
    /// <returns></returns>
    public Task<Consent> CreateConsentAsync(Consent consent);

    /// <summary>
    /// Update a consent
    /// </summary>
    /// <param name="consent"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<Consent> UpdateConsentAsync(Consent consent, ConsentStatus? status);
}
