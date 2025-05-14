using Microsoft.Extensions.Configuration;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using L00188315_Project.Core.Entities;

namespace Project_Tests.Services
{
    public class RevolutServiceTests
    {
        private readonly IRevolutService _service;
        private readonly HttpClient _mtlsClient;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly IKeyVaultService _keyVaultService;
        private readonly IConsentRepository _consentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly OpenBankingMapper _mapper;
        private readonly ILogger<RevolutService> _logger;

        public RevolutServiceTests()
        {
            _mtlsClient = Substitute.For<HttpClient>();
            _httpClient = Substitute.For<HttpClient>();
            _cacheService = Substitute.For<ICacheService>();
            _configuration = Substitute.For<IConfiguration>();
            _keyVaultService = Substitute.For<IKeyVaultService>();
            _consentRepository = Substitute.For<IConsentRepository>();
            _accountRepository = Substitute.For<IAccountRepository>();
            _balanceRepository = Substitute.For<IBalanceRepository>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _mapper = new OpenBankingMapper();
            _logger = Substitute.For<ILogger<RevolutService>>();

            _service = new RevolutService(
                _cacheService,
                _configuration,
                _keyVaultService,
                _consentRepository,
                _logger,
                _accountRepository,
                _balanceRepository,
                _transactionRepository,
                _mapper);

        }

    }
}
