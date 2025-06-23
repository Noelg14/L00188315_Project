using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Data;
using L00188315_Project.Infrastructure.Data.Identity;
using L00188315_Project.Server.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
if (Debugger.IsAttached)
{
    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerDocumentation();
}
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAppServices(builder.Configuration); //custom extenstion method.
builder.Services.AddIdentityServices(builder.Configuration);

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration).WriteTo.Console(); // write to console
    }
);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(options =>
    {
        options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSpa(options =>
{
    options.Options.SourcePath = "wwwroot"; // path to the client app
    options.Options.DefaultPage = "/index.html"; // default page to serve
});
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

//  Migrate in code
using var scope = app.Services.CreateScope(); // create a scope for this
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>(); // get the db context from the scope service
var Idcontext = services.GetRequiredService<AppIdentityDbContext>(); // get the db context from the scope service
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync(); //apply migration if pending
    await Idcontext.Database.MigrateAsync(); //apply migration if pending
}
catch (Exception ex)
{
    logger.LogError(ex, ex.Message);
}

await app.RunAsync();
