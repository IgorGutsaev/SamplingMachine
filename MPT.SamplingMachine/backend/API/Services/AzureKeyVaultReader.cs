using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace API.Services
{
    public class AzureKeyVaultReader
    {
        static readonly string KEY_VAULT_NAME_PROD = "filuetascprd";

        public static TokenCredential _credential;
        private static Dictionary<string, string> _cached = new Dictionary<string, string>();

        static public string GetSecret(string secretName)
        {
            if (_cached.TryGetValue(secretName, out string value))
                return value;

            _credential =
#if DEBUG
                new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = "4dfba626-6445-4bad-b98f-d9049ab7d0d0" })
#else
                new DefaultAzureCredential()
#endif
                ;

            SecretClient client = new SecretClient(new Uri($"https://{KEY_VAULT_NAME_PROD}.vault.azure.net/"),
                _credential,
                new SecretClientOptions {
                    Retry = {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            });
            
            KeyVaultSecret secret = client.GetSecret(secretName);
            _cached.TryAdd(secretName, secret.Value);
            return secret?.Value;
        }
    }
}