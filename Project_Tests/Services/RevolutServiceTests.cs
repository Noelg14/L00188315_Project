using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Project_Tests.Services
{
    public class RevolutServiceTests
    {
        private readonly IRevolutService _service;
        private readonly HttpClient _mtlsClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<ICacheService> _cacheService;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IKeyVaultService> _keyVaultService;
        private readonly Mock<IConsentRepository> _consentRepository;
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IBalanceRepository> _balanceRepository;
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly OpenBankingMapper _mapper;
        private readonly ILogger<RevolutService> _logger;

        public RevolutServiceTests()
        {
            _mtlsClient = new HttpClient();
            _httpClient = new HttpClient();
            _cacheService = new Mock<ICacheService>();
            _configuration = new Mock<IConfiguration>();
            _keyVaultService = new Mock<IKeyVaultService>();
            _consentRepository = new Mock<IConsentRepository>();
            _accountRepository = new Mock<IAccountRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();
            _mapper = new OpenBankingMapper();
            _logger = new Mock<ILogger<RevolutService>>().Object;

            _service = new RevolutService(
                _cacheService.Object,
                _configuration.Object,
                _keyVaultService.Object,
                _consentRepository.Object,
                _logger,
                _accountRepository.Object,
                _balanceRepository.Object,
                _transactionRepository.Object,
                _mapper
            );
        }
    }
}
