using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Exceptions;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.DTOs;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Project_Tests;

public class RevolutServiceTests
{
    private readonly Mock<ICacheService> _cacheService = new();
    private readonly Mock<IConfiguration> _configuration = SetupMockConfiguration();
    private readonly Mock<IKeyVaultService> _keyVaultService = new();
    private readonly Mock<IConsentRepository> _consentRepository = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IBalanceRepository> _balanceRepository = new();
    private readonly Mock<ITransactionRepository> _transactionRepository = new();
    private readonly Mock<OpenBankingMapper> _mapper = new();
    private readonly Mock<ILogger<RevolutService>> _logger = new();

    [Fact]
    public async Task GetBalancesAsync_ReturnsBalance_WhenBalanceExists()
    {
        // Arrange
        var userId = "user1";
        var accountId = "acc1";
        var balance = new Balance
        {
            RootAccountId = accountId,
            Currency = "USD",
            Amount = "1000",

        };
        _balanceRepository.Setup(r => r.GetBalanceAsync(userId,accountId)).ReturnsAsync(balance);
        var service = CreateService();

        // Act
        var result = await service.GetAccountBalanceAsync(accountId,userId);

        // Assert
        Assert.Equal(balance, result);
        _balanceRepository.Verify(r => r.GetBalanceAsync(userId, accountId), Times.Once);
    }
    
    [Fact]
    public async Task GetTransactionsAsync_ReturnsTransactions_WhenTransactionsExist()
    {
        // Arrange
        var userId = "user1";
        var accountId = "acc1";
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = "t1",
                RootAccountId = accountId,
                Amount = "1000",
                AmountCurrency = "USD",
                TransactionInformation = "Test transaction",
            },
        };
        _transactionRepository.Setup(r => r.GetAllTransactionsByAccountIdAsync(userId,accountId)).ReturnsAsync(transactions);
        var service = CreateService();

        // Act
        var result = await service.GetTransactionsAsync(accountId,userId);

        // Assert
        Assert.Equal(transactions, result);
        _transactionRepository.Verify(r => r.GetAllTransactionsByAccountIdAsync(userId,accountId), Times.Once);
    }
    
    [Fact]
    public async Task GetAccountsAsync_ReturnsAccounts_WhenAccountsExist()
    {
        // Arrange
        var userId = "user1";
        var accounts = new List<Account>
        {
            new Account
            {
                Id = "a1",
                AccountId = "acc1",
                AccountSubType = "Personal",
                AccountType = "Personal",
                Currency = "USD",
                Name = "Test",
                Iban = "123",
            },
        };
        _accountRepository.Setup(r => r.GetAllAccountsAsync(userId)).ReturnsAsync(accounts);
        var service = CreateService();

        // Act
        var result = await service.GetAccountsAsync(userId);

        // Assert
        Assert.Equal(accounts, result);
        _accountRepository.Verify(r => r.GetAllAccountsAsync(userId), Times.Once);
    }
    
    [Fact]
    public async Task GetAccountsAsync_ReturnsException_WhenNoAccountsAndNoToken()
    {
        // Arrange
        var userId = "user1";
        var service = CreateService();
        // Act & Assert
        await Assert.ThrowsAsync<TokenNullException>(async () => await service.GetAccountsAsync(userId));
        _accountRepository.Verify(r => r.GetAllAccountsAsync(userId), Times.Once);
    }
    
    [Fact]
    public async Task GetBalancesAsync_ReturnsException_WhenNoBalanceAndNoToken()
    {
        // Arrange
        var userId = "user1";
        var accountId = "acc1";
#pragma warning disable CS8603 // Possible null reference return. Allow it for this test - we want a null return.
        _balanceRepository.Setup(r => r.GetBalanceAsync(userId,accountId)).ReturnsAsync(() => null);
#pragma warning restore CS8603 // Possible null reference return.
        var service = CreateService();
        // Act
        // Assert
        await Assert.ThrowsAsync<TokenNullException>(async () => await service.GetAccountBalanceAsync(accountId,userId));
        _balanceRepository.Verify(r => r.GetBalanceAsync(userId,accountId), Times.Once);
    }
    
    [Fact]
    public async Task GetTransactionsAsync_ReturnsException_WhenNoTransactionsAndNoToken()
    {
        // Arrange
        var userId = "user1";
        var accountId = "acc1";
        var service = CreateService();
        // Act
        // Assert
        await Assert.ThrowsAsync<TokenNullException>(async () => await service.GetTransactionsAsync(accountId,userId));
        _transactionRepository.Verify(r => r.GetAllTransactionsByAccountIdAsync(userId,accountId), Times.Once);
    }

    [Fact]
    public async Task UpdateConsent_ReturnsOK_WhenConsentExists()
    {
        // Arrange
        var userId = "user1";
        var consent = new Consent
        {
            UserId = userId,
            ConsentId = "consent1",
            ConsentStatus = ConsentStatus.Created,
            Provider = "Revolut",
            Expires = DateTime.Now.AddDays(1)
        };
        _consentRepository.Setup(r => r.GetConsentAsync(consent.ConsentId)).ReturnsAsync(consent);
        var service = CreateService();
        // Act 
        await service.UpdateConsent(consent.ConsentId, ConsentStatus.Complete);
        consent.ConsentStatus = ConsentStatus.Complete;
        // Assert
        _consentRepository.Verify(r => r.GetConsentAsync(consent.ConsentId), Times.Once);
        _consentRepository.Verify(r => r.UpdateConsentAsync(consent, ConsentStatus.Complete), Times.Once);
    }
    [Fact]
    public async Task UpdateConsent_ReturnsException_WhenConsentDoesNotExists()
    {
        // Arrange
        var consentId = "non-existant-consent";
#pragma warning disable CS8603 // Possible null reference return. Allow it for this test - we want a null return.
        _consentRepository.Setup(r => r.GetConsentAsync(consentId)).ReturnsAsync(() => null);
#pragma warning restore
        var service = CreateService();
        // Act 
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateConsent(consentId, ConsentStatus.Complete));
        _consentRepository.Verify(r => r.GetConsentAsync(consentId), Times.Once);
    }
    
    [Fact]
    public async Task GetConsentRequestAsync_ReturnsOk_WhenConsentExists()
    {
        // Arrange
        var userId = "user1";
        var consent = new Consent
        {
            UserId = userId,
            ConsentId = "consent1",
            ConsentStatus = ConsentStatus.Created,
            Provider = "Revolut",
            Expires = DateTime.Now.AddDays(1)
        };

        _consentRepository.Setup(r => r.GetAllConsentsAsync(userId)).ReturnsAsync(new List<Consent> { consent });
        _keyVaultService.Setup(r => r.GetSecretAsync("revolutClientId")).ReturnsAsync("value");
        var service = CreateService();
        // Act
        var generatedRequest = await service.GetConsentRequestAsync(userId);
        // Assert
        Assert.Contains("request", generatedRequest);
        Assert.Contains("client_id", generatedRequest);
        Assert.Contains("redirect_uri", generatedRequest);
        _consentRepository.Verify(r => r.GetAllConsentsAsync(userId), Times.Once);
    }
    
    [Fact]
    public async Task GetConsentByIdAsync_ReturnsOk_WhenConsentExists()
    {
        // Arrange
        var userId = "user1";
        var consent = new Consent
        {
            UserId = userId,
            ConsentId = "consent1",
            ConsentStatus = ConsentStatus.Created,
            Provider = "Revolut",
            Expires = DateTime.Now.AddDays(1)
        };
        _consentRepository.Setup(r => r.GetConsentAsync(consent.ConsentId)).ReturnsAsync(consent);
        var service = CreateService();
        // Act
        var returnedConsent = await service.GetConsentByIdAsync(consent.ConsentId);
        // Assert
        Assert.Equal(consent, returnedConsent);
        _consentRepository.Verify(r => r.GetConsentAsync(consent.ConsentId), Times.Once);
    }
    
    [Fact]
    public async Task GetConsentByIdAsync_ReturnsException_WhenConsentIdIsNullExists()
    {
        // Arrange
        var service = CreateService();
        // Act
        // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetConsentByIdAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
    
    private RevolutService CreateService()
    {
        return new RevolutService(
            _cacheService.Object,
            _configuration.Object,
            _keyVaultService.Object,
            _consentRepository.Object,
            _logger.Object,
            _accountRepository.Object,
            _balanceRepository.Object,
            _transactionRepository.Object,
            _mapper.Object
        );
    }

    private static Mock<IConfiguration> SetupMockConfiguration()
    {
        var _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(x => x["Revolut:baseUrl"]).Returns("https://localhost");
        _mockConfig.Setup(x => x["Revolut:tokenUrl"]).Returns("http://localhost/token");
        _mockConfig.Setup(x => x["Revolut:consentUrl"]).Returns("http://localhost/token");
        _mockConfig.Setup(x => x["Revolut:loginUrl"]).Returns("http://localhost/token");
        _mockConfig.Setup(x => x["Revolut:certPath"]).Returns("./");
        _mockConfig.Setup(x => x["Revolut:keyPath"]).Returns("E:/ATU/Project/Idea/RevolutCert/private.key");
        _mockConfig.Setup(x => x["Revolut:pfxPath"]).Returns("E:/ATU/Project/Idea/RevolutCert/combined.pfx");
        _mockConfig.Setup(x => x["Revolut:redirectUri"]).Returns("http://localhost");

        return _mockConfig;
    }
}
