using System;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using Microsoft.Extensions.Configuration;
using VaultSharp.V1.SecretsEngines.Database;
using WebAPIforTest.Models;

namespace WebAPIforTest
{
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        public VaultOptions _config;
        private IVaultClient _client;


        public VaultConfigurationProvider(VaultOptions config)
        {
            _config = config;

            var vaultClientSettings = new VaultClientSettings(
                _config.Address, new AppRoleAuthMethodInfo(_config.roleID, _config.secretID)
            );

            _client = new VaultClient(vaultClientSettings);
        }

        public override void Load()
        {
            try
            {
                LoadAsync().Wait();
            }
            catch ( Exception ex ) { }
        }

        public async Task LoadAsync()
        {
            await GetDatabaseCredentials();
        }

        public async Task GetDatabaseCredentials()
        {
            var userID = "";
            var password = "";
            if (_config.SecretType == "database")
            {
                Secret<UsernamePasswordCredentials> staticDatabaseCredentials =
                await _client.V1.Secrets.Database.GetCredentialsAsync(
                  _config.Role,
                  _config.MountPath);

                userID = staticDatabaseCredentials.Data.Username;
                password = staticDatabaseCredentials.Data.Password;
            }

            Data.Add("database:Username", userID);
            Data.Add("database:Password", password);
        }
    }

    public class VaultConfigurationSource : IConfigurationSource
    {
        private VaultOptions _config;

        public VaultConfigurationSource(Action<VaultOptions> config)
        {
            _config = new VaultOptions();
            config.Invoke(_config);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VaultConfigurationProvider(_config);
        }
    }

    public static class VaultExtensions
    {
        public static IConfigurationBuilder AddVault(this IConfigurationBuilder configuration,
        Action<VaultOptions> options)
        {
            var vaultOptions = new VaultConfigurationSource(options);
            configuration.Add(vaultOptions);
            return configuration;
        }
    }
}