using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace Project_Tests;

public class KeyVaultServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly ICacheService _mockCache;
    public KeyVaultServiceTests()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockCache = new CacheService(GetTestCache());
        _mockConfig.Setup(x => x["kvSettings:ClientId"]).Returns("testClientId"); 
        _mockConfig.Setup(x => x["kvSettings:ClientSecret"]).Returns("testSecret"); 
        _mockConfig.Setup(x => x["kvSettings:TokenUrl"]).Returns("http://localhost/token"); 
        _mockConfig.Setup(x => x["kvSettings:Scope"]).Returns("1234"); 
        _mockConfig.Setup(x => x["kvSettings:KvBaseUrl"]).Returns("http://localhost"); 
        _mockConfig.Setup(x => x["kvSettings:KvApiVersion"]).Returns("1");
    }
    [Fact]
    public async Task GetSecretAsync_ShouldReturnSecret_WhenSecretExists()
    {
        //Arrange

        var handlerMock = GetMockHandler();
        var client = new HttpClient(handlerMock.Object);

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("KeyVaultClient")).Returns(client);

        var service = new KeyVaultService(factoryMock.Object,_mockConfig.Object,_mockCache);

        // Act
        var result = await service.GetSecretAsync("my-secret");

        // Assert
        Assert.Equal("TEST", result);
    }

    public IMemoryCache GetTestCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache!;
    }
    private Mock<HttpMessageHandler> GetMockHandler()
    {
        var token = """
                        {
                "token_type": "Bearer",
                "expires_in": 3599,
                "ext_expires_in": 3599,
                "access_token": "1"
            }
            """;

        var secret = """
                {
                "value": "TEST",
                "id": "secret",
                "attributes": {
                    "enabled": true,
                    "created": 1741782755,
                    "updated": 1741782755,
                    "recoveryLevel": "Recoverable+Purgeable",
                    "recoverableDays": 90
                },
                "tags": {}
            }
            """;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock // TOKEN call
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsoluteUri.Contains("token")),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(token),
           });

        handlerMock // secrets call
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsoluteUri.Contains("secrets")),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(secret)
            });
            return handlerMock;
    }
}
