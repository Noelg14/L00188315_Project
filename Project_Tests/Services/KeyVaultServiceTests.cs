using System.Net;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Exceptions;
using L00188315_Project.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;

namespace Project_Tests;

public class KeyVaultServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly ICacheService _mockCache;

    public KeyVaultServiceTests()
    {
        _mockConfig = new();
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

        var service = new KeyVaultService(factoryMock.Object, _mockConfig.Object, _mockCache);

        // Act
        var result = await service.GetSecretAsync("my-secret");

        // Assert
        Assert.Equal("TEST", result);
    }

    [Fact]
    public async Task GetCertAsync_ShouldReturnCert_WhenCertExists()
    {
        // Arrange
        var handlerMock = GetMockHandler();
        var client = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("KeyVaultClient")).Returns(client);
        var service = new KeyVaultService(factoryMock.Object, _mockConfig.Object, _mockCache);
        // Act
        var result = await service.GetCertAsync("my-cert");
        // Assert
        Assert.True(handlerMock.Invocations.Count == 2);
        Assert.Equal("TEST", result);
    }

    [Fact]
    public async Task GetCertAsync_ShouldThrowError_WhenCertDoesNotExist()
    {
        var handlerMock = GetMockHandler();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x =>
                    x.RequestUri!.AbsoluteUri.Contains("certificates")
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("message\":\"Cert not found\""),
                }
            );
        var client = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("KeyVaultClient")).Returns(client);
        var service = new KeyVaultService(factoryMock.Object, _mockConfig.Object, _mockCache);
        // Act
        var exception = await Assert.ThrowsAsync<KeyVaultException>(async () =>
            await service.GetCertAsync("non-existent-cert")
        );
        // Assert
        Assert.Contains("Cert not found", exception.Message);
    }

    [Fact]
    public async Task GetSecretAsync_ShouldThrowError_WhenSecretIsNull()
    {
        var handlerMock = GetMockHandler();
        var nullSecret = "{}";
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsoluteUri.Contains("secrets")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(nullSecret),
                }
            );
        var client = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("KeyVaultClient")).Returns(client);
        var service = new KeyVaultService(factoryMock.Object, _mockConfig.Object, _mockCache);
        // Act
        var exception = await Assert.ThrowsAsync<KeyVaultException>(async () =>
            await service.GetSecretAsync("null-secret")
        );
        // Assert
        Assert.Contains("Error getting Secret", exception.Message);
    }

    [Fact]
    public async Task GetSecretAsync_ShouldThrowError_WhenSecretDoesNotExist()
    {
        var handlerMock = GetMockHandler();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsoluteUri.Contains("secrets")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("message\":\"Secret not found\""),
                }
            );
        var client = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("KeyVaultClient")).Returns(client);
        var service = new KeyVaultService(factoryMock.Object, _mockConfig.Object, _mockCache);
        // Act
        var exception = await Assert.ThrowsAsync<KeyVaultException>(async () =>
            await service.GetSecretAsync("non-existent-secret")
        );
        // Assert
        Assert.Contains("Error getting Secret", exception.Message);
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
        var cert = """
                {
                "cer":"TEST"
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
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(token),
                }
            );

        handlerMock // secrets call
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsoluteUri.Contains("secrets")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(secret),
                }
            );

        handlerMock // cert call
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x =>
                    x.RequestUri!.AbsoluteUri.Contains("certificates")
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(cert),
                }
            );
        return handlerMock;
    }
}
