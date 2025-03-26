using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Core.Models;
using L00188315_Project.Server.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L00188315_Project.Server.Controllers
{
    [ApiController]
    [Route("api/revolut")]
    [Authorize]
    public class RevolutController : ControllerBase
    {
        //"openbanking_intent_id": "CONSENTID",

        private readonly IRevolutService _revolutService;

        public RevolutController(IRevolutService revolutService)
        {
            _revolutService = revolutService;
        }

        [HttpGet("jwks")]
        [AllowAnonymous]
        public ActionResult<JWKS> JWKS()
        {
            var jwks = new JWKS();
            jwks.keys = new List<Key>()
            {
                new Key
                {
                    kty = "RSA",
                    use = "sig",
                    kid = "68d032ce-b2c3-43dd-b6a5-fb6f095f7b3b",
                    e = "AQAB",
                    n =
                        "zb4SS9xkep7pR8XBziBjgRvl2G7FXCzviiGesiA-Lnylram7fsIv5WBxitXKESpC_mro6WRZa3Dl\r\nuhAE8B1yBd8gSp6h7IKjB4kvRxuvy-gnji1Iz7l-6vC3U-siB9XEe74HY2193BnBRjl4nyHuL-WT\r\nq98aebc5Yc_7W3nCPMUSLTFTxvt2ehNlrqyLFUWFYfcNX7nl7ElDCpElGsLL15nG_3ncz1I66Yv-\r\nM2d6fX1HXZr4Xghf1xpVZuhQjNcZ18Mn_oi-U7Y0EJEBNCIke4BCphniEpvi_aHwVSDNarhHk1cr\r\nfPofBXQAcGlYUg8f-cDvbjNbAgBn9yoB_AUpdQ",
                    x5c = new List<string>
                    {
                        "MIIEezCCAmOgAwIBAgIFANgQUp0wDQYJKoZIhvcNAQELBQAwYDELMAkGA1UEBhMCVUsxDzANBgNVBAgMBkxvbmRvbjEQMA4GA1UECgwHUmV2b2x1dDEQMA4GA1UECwwHU2FuZGJveDEcMBoGA1UEAwwTc2FuZGJveC5yZXZvbHV0LmNvbTAeFw0yNTAyMjIwMDM5NThaFw0yNjAyMjIwMDM5NThaMIGdMQswCQYDVQQGEwJHQjEVMBMGA1UECgwMTm9lbCBHcmlmZmluMRswGQYDVQQLDBIwMDE1ODAwMDAxMDNVQXZBQU0xHzAdBgNVBAMMFjJraVhReW8wdGVkalcyc29talNnSDcxOTA3BgNVBGEMMFBTRFVLLVJFVkNBLTk5MzVjNzMxLThmMTAtNGVmNi1iMjQ1LTEwNzQxOWQ2ZTQ0ZjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAM2+EkvcZHqe6UfFwc4gY4Eb5dhuxVws74ohnrIgPi58pa2pu37CL+VgcYrVyhEqQv5q6OlkWWtw5boQBPAdcgXfIEqeoeyCoweJL0cbr8voJ44tSM+5furwt1PrIgfVxHu+B2NtfdwZwUY5eJ8h7i/lk6vfGnm3OWHP+1t5wjzFEi0xU8b7dnoTZa6sixVFhWH3DV+55exJQwqRJRrCy9eZxv953M9SOumL/jNnen19R12a+F4IX9caVWboUIzXGdfDJ/6IvlO2NBCRATQiJHuAQqYZ4hKb4v2h8FUgzWq4R5NXK3z6HwV0AHBpWFIPH/nA724zWwIAZ/cqAfwFKXUCAwEAATANBgkqhkiG9w0BAQsFAAOCAgEAYvOrmql2pA7Kt+uTxxBgiHqQi/+fgq76VR6+QtjdvbCD+e7YB2AcFZDUKW6/jRvmrW3W+DL6divG0YhKs67xpxPdPgE0CPVPhJHLFzXoriObJgfigdqWlgd7EOun6L0iMhr0dnURGai+J+MAnpIjgKeX2qKS4dysmKpRF9SChtuKV6PNuINuQTzZENqKP7lK9Q6bDs9KEroWiMeEVRx6RtjoaIniNOwGJDYcSx0zHoFd15JHLgX/5wP/vmgL2X4qGvho5d014va8Flj32L2uUdZ2LlbhObzGTYtfY0nAjdmbd1nJ0F1XBqzg6mS8WaNtGFL+lcV28MTv0OtaFVuG4tB5CkN83SbSMniedqy0CBA23UQzdr+xpZhv6P3piMvohdDowZpdo0O8/uYI+Q/2Xz8IIU3aglTSm98mmpjI7x9xqniyAH+gyhS2pebUVavTzna0spX0bugTezIrCEGg5B10ztmzkhPSJEBq512ums0b7oN40e3S0FLdhIZXpH6I6JCtaLvqPEiuL5M5FPlWbXd/LVPYRHO3FmN7y2MToXKnH82kUFtEr/7PjVV59K206pef6EgqhOrbLfOnuNjv1nc6461e0NWNCMn2tKqyqKTCrl6a489BMGRn9alvmcPvjP4MRSRqOj1Shh0EON2M4Yr3lvJS3wqQtlkAjz63Tfg=",
                    },
                },
            };
            return Ok(jwks);
        }

        [HttpGet("consent")]
        public async Task<ActionResult<string>> GetConsent()
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

            return Ok(await _revolutService.GetConsentAsync(userId!));
        }

        [HttpGet("callback")]
        public async Task<ActionResult<string>> Callback(
            [FromQuery] string code,
            [FromQuery] string id_token
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(id_token);
            var consentId = jwtSecurityToken
                .Claims.First(x => x.Type == "openbanking_intent_id")
                .Value;

            await _revolutService.UpdateConsent(consentId, Core.Entities.ConsentStatus.Complete); // update consent first
            var token = await _revolutService.GetUserAccessToken(userId, code);

            return Ok(new { Token = token });
        }

        [HttpGet("accounts")] //Call 1st
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            return Ok(await _revolutService.GetAccountsAsync(userId));
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<List<Transaction>>> GetTransactions(
            [FromQuery] string? accountId
        )
        {
            if (string.IsNullOrEmpty(accountId))
                return BadRequest("Account Id is required");

            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var accounts = await _revolutService.GetAccountsAsync(userId);
            var transactions = await _revolutService.GetTransactionsAsync(accountId, userId);

            return Ok(transactions);
        }

        [HttpGet("balances")]
        public async Task<ActionResult<List<Balance>>> GetBalances([FromQuery] string? accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                return BadRequest("Account Id is required");

            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var accounts = await _revolutService.GetAccountsAsync(userId);
            var balances = await _revolutService.GetAccountBalanceAsync(accountId, userId);

            return Ok(balances);
        }
    }
}
