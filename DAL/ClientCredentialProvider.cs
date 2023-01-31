using Microsoft.Identity.Client;

namespace RCMAPI.DAL
{
    internal class ClientCredentialProvider
    {
        private IConfidentialClientApplication confidentialClientApplication;

        public ClientCredentialProvider(IConfidentialClientApplication confidentialClientApplication)
        {
            this.confidentialClientApplication = confidentialClientApplication;
        }
    }
}