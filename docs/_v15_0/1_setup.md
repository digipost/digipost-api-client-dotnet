---
title: Initial setup
identification: setup
layout: default
---

The client library is available as a Nuget package. The client library (and associated Nuget package) is updated regularly as new functionality is added. 


To install the Nuget package, follow these steps in Visual Studio/Rider:

1. Select _TOOLS -> Nuget Package Manager -> Manage Nuget Packages Solution..._

1. Search for _Digipost.Api.Client_. Multiple packages will appear. Install those necessary for you. Make sure you _DON'T_ install the 
`digipost-api-client packages`. Those are .NET Framework libraries with an unfortunately similar name.
If you're looking for the .NET Framework documention, please see version [8.3](http://digipost.github.io/digipost-api-client-dotnet/v8.3/).
If you would like pre-releases of this package, make sure Include Prerelease` is enabled. Please refer to documentation for your version of Visual Studio for detailed instructions.
1. Select which _Digipost.Api.Client.X_ libraries you need and click _Install_ for each.

## Authenticate using JWT/mTLS (OAuth 2.0 client credentials)

This is the only supported way to authenticate with the Digipost API. The client authenticates against Digipost's mIDP using an OAuth 2.0
`client_credentials` grant presented over mTLS, and then calls the Digipost API with the
resulting bearer token. 

### Set up an OAuth2 client and certificate

Register an OAuth2 client and certificate at https://nyva.digipost.no . There you can either generate
a certificate signed by the Digipost CA, or upload an enterprise certificate you already have from Buypass
or Commfides. Install the certificate in your OS certificate store as described below, and note its
thumbprint - this also makes the certificate's issuing chain available locally, which the mTLS handshake to
mIDP needs to complete.

### Trust the certificate

The certificate's issuing chain must be trusted by the OS, not just present - this typically isn't an issue
for CA-issued certificates, certificates issued by the Digipost CA/test certificates may need their intermediate and root
explicitly trusted.

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
1. Choose the `System` keychain, not `login` - this is where the .NET client's certificate lookup by
thumbprint reads from.
1. Press _File_ and then _Import Items..._
1. Choose the certificate and add it. If the certificate's issuing chain (intermediate and root) is
bundled in the same file, this imports those too.
1. If your system does not already trust the root of the certificate chain (for instance if you're using a Digipost-CA generated certificate from Nyva), find the root CA certificate in the list and double-click it to open the
certificate inspector. Expand the _Trust_ section, set _X.509 Basic Policy_ to _Always Trust_.

##### Trust the Certificate on Linux:
Download the root and intermediate certificates from [Difi](https://begrep.difi.no/SikkerDigitalPost/1.2.6/sikkerhet/sertifikathandtering) for your certificate provider.
Note the renaming to have `.crt` ending for `update-ca-certificates`:

```
sudo cp Buypass_Class_3_Test4_Root_CA.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_Root_CA.crt
sudo cp Buypass_Class_3_Test4_CA_3.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_CA_3.crt
sudo update-ca-certificates
```

### Create ClientConfig and DigipostClient

```csharp
var broker = new Broker(brokerId);
var clientConfig = new ClientConfig(broker, Environment.Production);

var jwtAuthConfig = new JwtAuthConfig(clientId: "your-oauth-client-id", thumbprint: "84e492a972b7e...");

var client = new DigipostClient(clientConfig, jwtAuthConfig);
```

The certificate is only presented over mTLS to mIDP - the Digipost API itself is called with just the
resulting bearer token.

The token endpoint is derived automatically from `ClientConfig.Environment`. Only set it explicitly for a
custom/on-prem setup:

```csharp
jwtAuthConfig.TokenEndpoint = new Uri("https://midp.custom.example/oauth2/token");
```

### Load enterprise certificate from file

If there is a reason for not loading the certificate from the OS certificate store, you can use the constructor
taking in a `X509Certificate2` directly instead of a thumbprint:

```csharp
var clientConfig = new ClientConfig(broker, Environment.Production);
var enterpriseCertificate =
    X509CertificateLoader.LoadPkcs12FromFile(
        @"C:\Path\To\Certificate\Cert.p12",
        "secretPasswordProperlyInstalledAndLoaded",
        X509KeyStorageFlags.Exportable
    );

var jwtAuthConfig = new JwtAuthConfig(clientId: "your-oauth-client-id", enterpriseCertificate);

var client = new DigipostClient(clientConfig, jwtAuthConfig);
```

## Migrating from certificate-based authentication

Versions prior to 15.0 authenticated by signing each request with an enterprise certificate
(`new DigipostClient(clientConfig, thumbprint)` / `new DigipostClient(clientConfig, enterpriseCertificate)`).
That request-signing authentication has been removed. JWT/mTLS, described above, is now the only
supported authentication method:

* Register an OAuth2 client for your existing enterprise certificate at https://nyva.digipost.no -
  the same certificate used for signing can be reused for the mTLS handshake.
* Replace `new DigipostClient(clientConfig, thumbprint)` or `new DigipostClient(clientConfig, enterpriseCertificate)`
  with `new DigipostClient(clientConfig, jwtAuthConfig)`, constructing a `JwtAuthConfig` with your new OAuth2
  client id and the same certificate, as shown above.

## Load root element of Digipost rest api

The Digipost API is a HATEOAS-driven REST API. If you query the root you will get a response containing
the Digipost public key and a list of hyperlinks to resources you can use with your sender or as a broker 
for a sender. This information is used behind the scenes by the client library to get the correct urls for different resources.

Fetch the default root. This root is normally what you want to use if you are not a broker for another sender.
```csharp
var root = client.GetRoot(new ApiRootUri());
```

Fetch the root for a sender.
```csharp
var root = client.GetRoot(new ApiRootUri(12345));
```

You can use this resource to safely test your integration or to fetch Digipost public key.

## Proxy support
We have basic proxy support in the client. To use a proxy you need to define it in ClientConfig with a NetworkCredential for authentication.

```csharp
var clientConfig = new ClientConfig(broker, Environment.Production)
{
    WebProxy = new WebProxy("10.0.0.1"),
    Credential = new NetworkCredential("foo", "bar")
};
var jwtAuthConfig = new JwtAuthConfig(clientId: "your-oauth-client-id", thumbprint: "84e492a972b7e...");
var client = new DigipostClient(clientConfig, jwtAuthConfig);
```
