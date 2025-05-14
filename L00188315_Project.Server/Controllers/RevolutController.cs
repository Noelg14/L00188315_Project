using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Exceptions;
using L00188315_Project.Server.DTOs.Response;
using L00188315_Project.Server.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace L00188315_Project.Server.Controllers
{
    /// <summary>
    /// Controller for integration with Revolut
    /// </summary>
    [ApiController]
    [Route("api/revolut")]
    [Consumes("application/json")]
    [Authorize]
    public class RevolutController : ControllerBase
    {
        //"openbanking_intent_id": "CONSENTID",

        private readonly IRevolutService _revolutService;
        private readonly ILogger<RevolutController> _logger;
        /// <summary>
        /// Constructor for the RevolutController
        /// </summary>
        /// <param name="revolutService"></param>
        /// <param name="logger"></param>
        public RevolutController(IRevolutService revolutService,
            ILogger<RevolutController> logger)
        {
            _revolutService = revolutService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the JWKS used by revolut to verify our consent JWT
        /// </summary>
        [HttpGet("jwks")]
        [AllowAnonymous]
        [SwaggerIgnore] // hide this in the public doc
        public ActionResult<JWKS> JWKS()
        {
            var jwks = new JWKS
            {
                keys = new List<Key>()
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
                }
            };
            return Ok(jwks);
        }

        /// <summary>
        /// Generates the Consent and Login path for Revolut
        /// </summary>
        /// <returns>Login path for revolut</returns>
        [HttpGet("consent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetConsent()
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            _logger.LogInformation("Getting consent for {0}", userId);

            var apiResponse = new ApiResponseDTO<string>();
            apiResponse.Data = await _revolutService.GetConsentRequestAsync(userId!);
            apiResponse.Success = true;
            return Ok(apiResponse);
        }
        /// <summary>
        /// Callback for the consent flow
        /// </summary>
        /// <param name="code">Auth Code to be swapped for access token</param>
        /// <param name="id_token">JWT token containing information about the consent</param>
        /// <returns>The Access token</returns>

        [HttpGet("callback")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Callback(
            [FromQuery] string code,
            [FromQuery] string id_token
        )
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(id_token);
            var consentId = jwtSecurityToken
                .Claims.First(x => x.Type == "openbanking_intent_id")
                .Value;

            var consent = await _revolutService.GetConsentByIdAsync(consentId);
            if (consent is null)
            {
                _logger.LogError("Consent {0} not found", consentId);
                return BadRequest(new ApiResponseDTO<string> { Message = "Consent not found", Success = false });
            }   

            var userId = consent.UserId; // Update consent for the user who created it.

            _logger.LogInformation("Callback Received for User {0} with Consent {1}", userId, consentId);


            await _revolutService.UpdateConsent(consentId, ConsentStatus.Complete); // update consent first
            _logger.LogInformation("Consent {0} updated ", consentId);

            var token = await _revolutService.GetUserAccessToken(userId!, code);

            var usersAccounts = await _revolutService.GetAccountsAsync(userId!); // gets the accounts for the user
            //For each account, get the transactions and balance
            //usersAccounts.ForEach(async x =>
            //{
            //   await Task.WhenAll(
            //         _revolutService.GetTransactionsAsync(x.AccountId, userId!),
            //         _revolutService.GetAccountBalanceAsync(x.AccountId, userId!)
            //    );
            //});

#if DEBUG
            return RedirectPermanent("http://localhost:4200/account"); // if debugging, return to the angular app
            //return Ok(new { Token = token }); // if debugging, return the token
#endif
            return Redirect("/accounts"); // if not in debug mode, return no content - redirect in future
        }
        /// <summary>
        /// Gets the list of revolut accounts for the user
        /// </summary>
        /// <returns>An <see cref="ApiResponseDTO{T}"/> with accounts</returns>
        [HttpGet("accounts")] //Call 1st
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponseDTO<List<Account>>>> GetAccounts()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
                _logger.LogInformation("Getting Accounts for User: {0}", userId);
                var accounts = await _revolutService.GetAccountsAsync(userId!);
                if (accounts is not null)
                {
                    foreach (var account in accounts)
                    {
                        await _revolutService.GetAccountBalanceAsync(account.AccountId, userId!); // get the balance for each account
                    }
                    accounts = await _revolutService.GetAccountsAsync(userId!); // get the accounts again to update the balances
                }


                var apiResponse = new ApiResponseDTO<List<Account>>
                {
                    Data = accounts,
                    Success = accounts is null ? false : true
                };
                return Ok(apiResponse);
            }catch(TokenNullException ex)
            {
                _logger.LogError("Token is null: {0}", ex.Message);
                return BadRequest(new ApiResponseDTO<Balance> { Message = "Token is null, Please Relink Accounts", Success = false });
            }

        }
        /// <summary>
        /// Gets the transaction for a specified account
        /// </summary>
        /// <param name="accountId">Account to get the transactions for</param>
        /// <returns>An <see cref="ApiResponseDTO{T}"/> with transactions</returns>
        [HttpGet("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponseDTO<List<Transaction>>>> GetTransactions(
            [FromQuery] string? accountId
        )
        {
            if (string.IsNullOrEmpty(accountId))
                return BadRequest("Account Id is required");
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
                var accounts = await _revolutService.GetAccountsAsync(userId!);
                var transactions = await _revolutService.GetTransactionsAsync(accountId, userId!);

                _logger.LogInformation("Getting Transactions for User: {0} & Account {1}", userId, accountId);
                var apiResponse = new ApiResponseDTO<List<Transaction>>
                {
                    Data = transactions,
                    Success = transactions is null ? false : true
                };


                return Ok(apiResponse);
            }catch(TokenNullException ex)
            {
                _logger.LogError("Token is null: {0}", ex.Message);
                return BadRequest(new ApiResponseDTO<Balance> { Message = "Token is null, Please Relink Accounts", Success = false });

            }

        }
        /// <summary>
        /// Gets the balance for a specified account
        /// </summary>
        /// <param name="accountId">Account to get the balance</param>
        /// <returns>An <see cref="ApiResponseDTO{T}"/> with transactions</returns>

        [HttpGet("balances")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponseDTO<Balance>>> GetBalances([FromQuery] string? accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                return BadRequest("Account Id is required");
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
                var accounts = await _revolutService.GetAccountsAsync(userId!);
                var balances = await _revolutService.GetAccountBalanceAsync(accountId, userId!);

                _logger.LogInformation("Getting Balances for User: {0} & Account {1}", userId, accountId);
                var apiResponse = new ApiResponseDTO<Balance>
                {
                    Data = balances,
                    Success = balances is null ? false : true
                };

                return Ok(apiResponse);
            }
            catch (TokenNullException ex)
            {
                _logger.LogError("Token is null: {0}", ex.Message);
                return BadRequest(new ApiResponseDTO<Balance> { Message = "Token is null, Please Relink Accounts", Success = false });

            }
        }
    }
}
