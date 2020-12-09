# .NET Core Secret Manager Demo
This is a demo of how to use the .NET Core secret manager to enable safe storage of secrets for a development environment.

# References
* Official Documentation: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows

# Using the Secret Manager
## Setup
To enable the usage of the .NET Core Secret Manager, run the following [command](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows#enable-secret-storage) from the same directory as the top level application you wish to manage secrets for:
```powershell
dotnet user-secrets init
```
This adds the correct backing locations on your machine, and adds the User Secret Id to the project:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UserSecretsId>c36d8d74-e56a-4483-bbf0-6afb3f04a4e9</UserSecretsId>
  </PropertyGroup>

  ...
```

## Managing Secrets
After the user secret manager has been set up, add the required secrets for this project by running the following [command](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows#set-a-secret) for each secret
```powershell
dotnet user-secrets set "SampleSecret" "the super secret value"
```

## Reading Secrets
Once secrets have been set, ensure that the application is able to read them by adding the [Microsoft.Extensions.Configuration.UserSecrets](https://www.nuget.org/packages?q=Microsoft.Extensions.Configuration.UserSecrets) nuget package.  

After the package is added, be sure to add the _AddUserSecrets_ method in your application configuration setup.
Example:
```
 var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(); <= This is the one
```
For real world examples, see the usage of this in the sample projects below.

### Sample Projects
|Project|Type|Purpose|
|---|---|---|
|Sandbox.SecretShowcase.Console|.NET Core Console Application|Simple use case on how to read configuration values from differing sources|
|Sandbox.SecretShowcase.ConsoleWithDI|.NET Core Console Application|A more robust example using dependency injection and the host builder so user secrets are only read during local development|
