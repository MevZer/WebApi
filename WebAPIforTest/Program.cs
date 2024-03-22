
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Vault;
using Vault.Models;
using WebAPIforTest.Interfaces;
using WebAPIforTest.Services;
using Microsoft.Extensions.DependencyInjection;


namespace WebAPIforTest
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Configuration.GetSection("Vault")["Role"] != null)
            {
                builder.Configuration.AddVault(options =>
                {
                    var vaultOptions = builder.Configuration.GetSection("Vault");
                    options.Address = vaultOptions["Address"];
                    options.Role = vaultOptions["Role"];
                    options.MountPath = vaultOptions["MountPath"];
                    options.SecretType = vaultOptions["SecretType"];
                    options.roleID = vaultOptions["roleID"];
                    options.secretID = vaultOptions["secretID"];
                    options.Secret = builder.Configuration.GetSection("VAULT_SECRET_ID").Value;
                });
            }


            switch (builder.Configuration.GetSection("SELECT_AUTHENTICATION").Value)
            {
                case "vault":
                    AuthExtensions.AuthDbVault(builder);
                    break;
                case "db":
                    AuthExtensions.AuthDb(builder);
                    break;
                case "default":
                    AuthExtensions.AuthDefault(builder);
                    break;
            }

            builder.Services.AddTransient<IDataProvider, DataProvider>();

            builder.Services.AddMvc();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.AddHostedService<Increment5sService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

    }
}