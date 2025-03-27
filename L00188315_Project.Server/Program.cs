using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Server.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
#if DEBUG // Only run this code in debug mode, uses a local FQDN to redirect correctly.
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(
        443,
        ListenOptions =>
        {
            ListenOptions.UseHttps(httpsOptions =>
            {
                var localhostCert = CertificateLoader.LoadFromStoreCert(
                    "localhost",
                    "My",
                    StoreLocation.CurrentUser,
                    allowInvalid: true
                );
                var exampleCert = CertificateLoader.LoadFromStoreCert(
                    "NOEL-PC\\Noel@Noel-PC",
                    "My",
                    StoreLocation.CurrentUser,
                    allowInvalid: true
                );
                var certs = new Dictionary<string, X509Certificate2>(
                    StringComparer.OrdinalIgnoreCase
                )
                {
                    ["localhost"] = localhostCert,
                    ["test.noelgriffin.ie"] = exampleCert,
                };
                httpsOptions.ServerCertificateSelector = (connectionContext, name) =>
                {
                    if (name is not null && certs.TryGetValue(name, out var cert))
                    {
                        return cert;
                    }

                    return exampleCert;
                };
            });
        }
    );
});
#endif

// Add services to the container.

builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices(builder.Configuration); //custom extenstion method.
builder.Services.AddIdentityServices(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console() // write to console
    );

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#if DEBUG // debug endponts for KV / Cache
app.MapGet(
    "/secret/{secret}",
    (string secret, IKeyVaultService _service) =>
    {
        return _service.GetSecretAsync(secret);
    }
);
app.MapGet(
    "/cert/{secret}",
    (string secret, IKeyVaultService _service) =>
    {
        return _service.GetCertAsync(secret);
    }
);
app.MapGet(
    "/cache/{secret}",
    (string secret, ICacheService _service) =>
    {
        return _service.Get(secret);
    }
);
#endif

app.MapFallbackToFile("/index.html");

app.Run();
