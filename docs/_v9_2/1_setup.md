---
title: Initial setup
identification: setup
layout: default
---

The client library is available as a nuget package. The client library (and associated nuget package) is updated regularly as new functionality is added. 


To install the nuget package, follow these steps in Visual Studio/Rider:

1. Select _TOOLS -> nuget Package Manager -> Manage nuget Packages Solution..._

1. Search for _Digipost.Api.Client_. Multiple packages will appear. Install those necessary for you. Make sure you _DON'T_ install the `digipost-api-client packages. Those are .NET Framework libraries with an unfortunately similar name.
If you're looking for the .NET Framework documention, please see version [8.3](http://digipost.github.io/digipost-api-client-dotnet/v8.3/).
If you would like pre-releases of this package, make sure Include Prerelease` is enabled. Please refer to documentation for your version of Visual Studio for detailed instructions.
1. Select which _Digipost.Api.Client.X_ libraries you need and click _Install_ for each.

### Install and use enterprise certificate

<blockquote>SSL Certificates are small data files that digitally bind a cryptographic key to an organization's details. When installed on a web server, it activates the padlock and the https protocol (over port 443) and allows secure connections from a web server to a browser.</blockquote>

To communicate over HTTPS you need to sign your request with a enterprise certificate. The enterprise certificate can be loaded directly from file or from the Windows certificate store.

The following steps will install the certificate in the your certificate store. This should be done on the server where your application will run.
For more information, please see the [Microsoft Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows#how-the-secret-manager-tool-works).

The path and password to the certificate must be put somewhere safe.

The path on Windows is:
```
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
```

On MacOS/Linux the path is:
```
~/.microsoft/usersecrets/<user_secrets_id>/secrets.json
```

Add the following `UserSecretsId` element to your `.csproj` file:
{% highlight xml %}
<PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
     <UserSecretsId>enterprise-certificate</UserSecretsId>
</PropertyGroup>
{% endhighlight %}

This means that the element `<user_secrets_id>` in the path will be `enterprise-certificate`.

From the command line, navigate to the directory where the current `.csproj` file is located and run the following commands with your own certificate values:
```
dotnet user-secrets set "Certificate:Path:Absolute" "<your-certificate.p12>"
dotnet user-secrets set "Certificate:Password" "<your-certificate-password>"
```

##### Trust the Certificate on Windows:
1.  Double-click on the actual certificate file (CertificateName.p12)
1.  Save the sertificate in `Current User` or `Local Machine` and click _Next_. 
If you are running the client library from a system account, but debugging from a different user, please install it on `Local Machine`, as this enables loading it from any user.
1.  Use the suggested filename. Click _Next_
1.  Enter password for private key and select _Mark this key as exportable ..._ Click _Next_
1.  Choose _Automatically select the certificate store based on the type of certificate_
1.  Click _Next_ and _Finish_
1.  Accept the certificate if prompted.
1.  When prompted that the import was successful, click _Ok_


##### Trust the Certificate on MacOS:
1. Open `Keychain Access`
1. Choose `login` keychain
1. Press _File_ and then _Import_
1. Choose the enterprise certificate and add it 

##### Trust the Certificate on Linux:
Download the root and intermediate certificates from [Difi](https://begrep.difi.no/SikkerDigitalPost/1.2.6/sikkerhet/sertifikathandtering) for your enterprise certificate provider. 
Note the renaming to have `.crt` ending for `update-ca-certificates`:
 
```
sudo cp Buypass_Class_3_Test4_Root_CA.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_Root_CA.crt
sudo cp Buypass_Class_3_Test4_CA_3.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_CA_3.crt
sudo update-ca-certificates
```

### Create ClientConfig and DigipostClient:
```csharp

// The actual sender of the message. The broker is the owner of the organization certificate 
// used in the library. The broker id can be retrieved from your Digipost organization account.
const int brokerId = 12345;
            
var broker = new Broker(brokerId);
var clientConfig = new ClientConfig(broker, Environment.Production);

var client = new DigipostClient(clientConfig, CertificateReader.ReadCertificate());
```

Where `CertificateReader.ReadCertificate()` is:
```csharp

var pathToSecrets = $"{System.Environment.GetEnvironmentVariable("HOME")}/.microsoft/usersecrets/smoke-certificate/secrets.json";
_logger.LogDebug($"Reading certificate details from secrets file: {pathToSecrets}");
var fileExists = File.Exists(pathToSecrets);

if (!fileExists)
{
    _logger.LogDebug($"Did not find file at {pathToSecrets}");
}
            
var certificateConfig = File.ReadAllText(pathToSecrets);
var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(certificateConfig);

deserializeObject.TryGetValue("Certificate:Path:Absolute", out var certificatePath);
deserializeObject.TryGetValue("Certificate:Password", out var certificatePassword);

_logger.LogDebug("Reading certificate from path found in secrets file: " + certificatePath);

return new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.Exportable);
```


### Load enterprise certificate from file
If there is a reason for not loading the certificate from the Windows certificate store, you can use the constructor taking in a `X509Certificate2`:

```csharp

var clientConfig = new ClientConfig(broker, Environment.Production);
var enterpriseCertificate =
    new X509Certificate2(
        @"C:\Path\To\Certificate\Cert.p12",
        "secretPasswordProperlyInstalledAndLoaded",
        X509KeyStorageFlags.Exportable
    );

var client = new DigipostClient(clientConfig, enterpriseCertificate);

```

#### <span style="color:red">\[Warning: This is technically supported on other operating systems than Windows, but we've not tested that thoroughly. Use at your own risk.\]</span> Load enterprise certificate from thumbprint in code

1. Start mmc.exe (Click windowsbutton and type mmc.exe)
1. _Choose File_ -> _Add/Remove Snap-in…_ (Ctrl + M)
1. Mark certificate and click _Add >_
1. If the certificate was installed in `Current User` choose `My User Account` and if installed on `Local Machine` choose `Computer Account`. Then click _Finish_ and then _OK_
1. Expand _Certificates_ node, select _Personal_ and open _Certificates_
1. Double-click on the installed certificate
1. Go to the _Details_ tab
1. Scroll down to _Thumbprint_
1. Copy the thumbprint and load as shown below

```csharp

 var clientConfig = new ClientConfig(broker, Environment.Production);
 var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");

```