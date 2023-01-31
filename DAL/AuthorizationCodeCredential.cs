using Azure.Identity;

namespace RCMAPI.DAL
{
    internal class AuthorizationCodeCredential
    {
        private string tenantId;
        private string clientId;
        private string clientSecret;
        private string authorizationCode;
        private TokenCredentialOptions options;

        public AuthorizationCodeCredential(string tenantId, string clientId, string clientSecret, string authorizationCode, TokenCredentialOptions options)
        {
            this.tenantId = tenantId;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.authorizationCode = authorizationCode;
            this.options = options;
        }
    }
}