using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;
using CloudStorage.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddAzureClients(config =>
{
    config.AddBlobServiceClient(configuration["Microsoft:BlobStorage:ConnectionString"]);
});

services.AddControllers();

services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("AuthContext"));
});

services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddGoogle(options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddTransient<IStorageService, StorageService>();
services.AddTransient<IManageService, ManageService>();
services.AddTransient<DirectoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
