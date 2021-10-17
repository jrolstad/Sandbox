namespace octokit_sandbox.core.Configuration
{
    public static class ConfigurationNames
    {
        public static KeyVaultNames KeyVault = new KeyVaultNames();
        public static GitHubAppNames GitHubApp = new GitHubAppNames();

        public class KeyVaultNames
        {
            public string ManagedIdentityClient = "KeyVault:ManagedIdentity";
            public string BaseUri = "KeyVault:BaseUri";
        }

        public class GitHubAppNames
        {
            public string Id = "GitHubApp:Id";
        }


    }
}