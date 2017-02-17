---
title: Initial setup
identification: setup
layout: default
---

The client library is available as a nuget package. The client library (and associated nuget package) is updated regularly as new functionality is added. 


To install the nuget package, follow these steps in Visual Studio:

1. Select _TOOLS -> nuget Package Manager -> Manage nuget Packages Solution..._
1. Search for _digipost-signature-api-client-dotnet_.
* If you would like pre-releases of this package, make sure _Include Prerelease_ is enabled. Please refer to documentation for your version of Visual Studio for detailed instructions.
1. Select _digipost-signature-api-client_ and click _Install_.

### Install business certificate in certificate store

<blockquote>SSL Certificates are small data files that digitally bind a cryptographic key to an organization's details. When installed on a web server, it activates the padlock and the https protocol (over port 443) and allows secure connections from a web server to a browser.</blockquote>

To communicate over HTTPS you need to sign your request with a business certificate. The following steps will install the certificate in the your certificate store. This should be done on the server where your application will run.

1.  Double-click on the actual certificate file (CertificateName.p12)
1.  Save the sertificate in _Current User_ or _Local Machine_ and click _Next_ 
1.  USe the suggested filename. Click _Next_
1.  Enter password for private key and select _Mark this key as exportable ..._ Click _Next_
1.  Select _Automatically select the certificate store based on the type of certificate_
1.  Click _Next_ and _Finish_
1.  Accept the certificate if prompted.
1.  When prompted that the import was successful, click _Ok_

### Use business certificate thumbprint

1. Start mmc.exe (Click windowsbutton and type mmc.exe)
1. _Choose File_ -> _Add/Remove Snap-in…_ (Ctrl + M)
1. Mark certificate and click _Add >_
1. If the certificate was installed in _Current User_ choose _My User Account_ and if installed on _Local Machine_ choose _Computer Account_. Then click _Finish_ and then _OK_
1. Expand _Certificates_ node, select _Personal_ and open _Certificates_
1. Double-click on the installed certificate
1. Go to the _Details_ tab
1. Scroll down to _Thumbprint_
1. Copy the thumbprint