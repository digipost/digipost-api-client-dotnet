---
title: Initial setup
identification: setup
layout: default
---

The client library is available as a nuget package. The client library (and associated nuget package) is updated regularly as new functionality is added.

To install the nuget package, follow these steps in Visual Studio:

1. Select _TOOLS -> nuget Package Manager -> Manage nuget Packages Solution..._
2. Search for "_digipost-api-client_"
* If you would like pre-releases og this package, make sure it says _Include Prerelease_ in the drop-down menu above the search results (where it by default says _Stable Only_).
3. Select _digipost-api-client_ and click _Install_.

<h3 id="businesscertificate">Install business certificate in certificate store</h3>

<blockquote>SSL Certificates are small data files that digitally bind a cryptographic key to an organization's details. When installed on a web server, it activates the padlock and the https protocol (over port 443) and allows secure connections from a web server to a browser.</blockquote>

To communicate over HTTPS you need to sign your request with a business certificate. The following steps will install the certificate in the your certificate store. This should be done on the server where your application will run.

1.  Double-click on the actual certificate file (CertificateName.p12)
2.  Save the sertificate in _Current User_ and click _Next_
3.  USe the suggested filename. Click _Next_
4.  Enter password for private key and select _Mark this key as exportable ..._ Click _Next_
5.  Select _Automatically select the certificate store based on the type of certificate_
6.  Click _Next_ and _Finish_
7.  Accept the certificate if prompted.
8.  When prompted that the import was successful, click _Ok_.

<h3 id="find_businesscertificate">Use business certificate thumbprint</h3>

1. Start mmc.exe (Click windowsbutton and type mmc.exe)
2. Choose File -> Add/Remove Snap-in…(Ctrl + M)
3. Mark certificate and click Add >
4. Choose 'My user account' followed by Finish, then 'OK'.
5. Double-click on 'Certificates' 
6. Double-click on the installed certificate
7. Go to the 'Details' tab
8. Scroll down to 'Thumbprint'
9. Copy the thumbprint.
10. IMPORTANT: If you experience that the client is not finding the certificate, then it could be that the thumbprint you copied has an invisible BOM (Byte Order Mark). To remove the invisible character you can paste the thumbprint in a text editor, then go to the start of the string and delete it.
