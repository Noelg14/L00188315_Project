using System.Net;
using System.Net.Http;
using System.Text.Json;
using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace Project_Tests.Services
{
    public class RevolutServiceTests
    {
        private IRevolutService _service;
        private HttpClient _mtlsClient;
        private HttpClient _httpClient;
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
            _cacheService = new Mock<ICacheService>();
            _configuration = new Mock<IConfiguration>();
            _keyVaultService = new Mock<IKeyVaultService>();
            _consentRepository = new Mock<IConsentRepository>();
            _accountRepository = new Mock<IAccountRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();
            _mapper = new OpenBankingMapper();
            _logger = new Mock<ILogger<RevolutService>>().Object;
        }

        //Helper method to set up the mock httpClient
        private HttpClient ConfigureMockClient<T>(T responseData)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData)),
            };
            // Set up the SendAsync method behavior.
            httpMessageHandlerMock
                .Protected() // <= this is most important part that it need to setup.
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);
            // create the HttpClient
            return new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new System.Uri("http://localhost"), // It should be in valid uri format.
            };
        }
    }
}
