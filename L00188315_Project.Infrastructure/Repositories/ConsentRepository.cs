using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the IConsentRepository interface for managing consents.
    /// </summary>
    public class ConsentRepository : IConsentRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Constructor for ConsentRepository.
        /// </summary>
        /// <param name="dbContext"></param>
        public ConsentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Create a consent for a user.
        /// </summary>
        /// <param name="consent"></param>
        /// <returns></returns>
        public async Task<Consent> CreateConsentAsync(Consent consent)
        {
            await _dbContext.Consents.AddAsync(consent);
            await _dbContext.SaveChangesAsync();
            return consent;
        }

        /// <summary>
        /// Get all consents for a user by account id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Consent>> GetAllConsentsAsync(string userId)
        {
            return await _dbContext.Consents.Where(c => c.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Gets a consent by its id.
        /// </summary>
        /// <param name="consentId"></param>
        /// <returns></returns>
        public async Task<Consent> GetConsentAsync(string consentId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Consents.FirstOrDefaultAsync(c => c.ConsentId == consentId);
#pragma warning restore CS8603 // Possible null reference return.
            // it's okay to return null here, as we are checking for null in the service.
        }

        /// <summary>
        /// Update a consent.
        /// </summary>
        /// <param name="consent"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<Consent> UpdateConsentAsync(Consent consent, ConsentStatus? status)
        {
            var thisConsent = await GetConsentAsync(consent.ConsentId);
            if (status is not null)
            {
                thisConsent.ConsentStatus = status;
            }
            _dbContext.Consents.Update(thisConsent);
            await _dbContext.SaveChangesAsync();

            return thisConsent;
        }
    }
}
