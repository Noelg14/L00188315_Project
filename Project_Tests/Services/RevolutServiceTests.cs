using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Project_Tests;
public class RevolutServiceTests
{
    private readonly Mock<ICacheService> _cacheService = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IKeyVaultService> _keyVaultService = new();
    private readonly Mock<IConsentRepository> _consentRepository = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IBalanceRepository> _balanceRepository = new();
    private readonly Mock<ITransactionRepository> _transactionRepository = new();
    private readonly Mock<OpenBankingMapper> _mapper = new();
    private readonly Mock<ILogger<RevolutService>> _logger = new();

    private RevolutService CreateService() =>
        new RevolutService(
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

    [Fact]
    public async Task GetAccountsAsync_ReturnsAccounts_WhenAccountsExist()
    {
        // Arrange
        var userId = "user1";
        var accounts = new List<Account> { new Account {
            Id = "a1",
            AccountId = "acc1",
            AccountSubType = "Personal",
            AccountType = "Personal",
            Currency = "USD",
            Name = "Test",
            Iban = "123"
           }
        };
        _accountRepository.Setup(r => r.GetAllAccountsAsync(userId)).ReturnsAsync(accounts);

        var service = CreateService();

        // Act
        var result = await service.GetAccountsAsync(userId);

        // Assert
        Assert.Equal(accounts, result);
        _accountRepository.Verify(r => r.GetAllAccountsAsync(userId), Times.Once);
    }
}