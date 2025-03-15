using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices(builder.Configuration); //custom extenstion method.
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#if DEBUG
    app.MapGet("/secret/{secret}", (string secret, IKeyVaultService _service) =>
    {
        return _service.GetSecretAsync(secret);
    });    
    app.MapGet("/cert/{secret}", (string secret, IKeyVaultService _service) =>
    {
        return _service.GetCertAsync(secret);
    });    
    app.MapGet("/cache/{secret}", (string secret, ICacheService _service) =>
    {
        return _service.Get(secret);
    });
#endif

app.MapFallbackToFile("/index.html");

app.Run();
