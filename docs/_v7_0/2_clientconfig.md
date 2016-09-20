---
title: Client configuration
identification: clientconfig
layout: default
---

ClientConfig is a container for all the connection specific paramters that you can set. All attributes, except _TechnicalSenderID_, have default values.  The technical sender id is a required parameter to contstruct the class.

{% highlight csharp%}

 // The sender id can be retrieved from your Digipost organisation account (https://www.digipost.no/bedrift)
const string senderId = "123456";
var config = new ClientConfig(senderId, Environment.Qa);

{% endhighlight%}