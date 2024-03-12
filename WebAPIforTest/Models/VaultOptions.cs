using Microsoft.AspNetCore.Hosting;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp;

namespace WebAPIforTest.Models
{
    public class VaultOptions
    {
        public string Address { get; set; }
        public string Role { get; set; }

        public string Secret { get; set; }
        public string MountPath { get; set; }
        public string SecretType { get; set; }

        public string roleID { get; set; }
        public string secretID { get; set; }
    }

}
