---
title: Client configuration
identification: clientconfig
layout: default
---

ClientConfig is a container for all the connection specific paramters that you can set.

{% highlight csharp%}

// The actual sender of the message. The broker is the owner of the organization certificate 
// used in the library. The broker id can be retrieved from your Digipost organization account.
var broker = new Broker(12345);

// The sender is what the receiver of the message sees as the sender of the message. 
// If you are delivering on behalf of yourself, set this to your organization`s sender id.
var sender = new Sender(67890);

var clientConfig = new ClientConfig(broker, Environment.Production);
var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");

{% endhighlight%}