---
title: Client configuration
identification: clientconfig
layout: default
---

ClientConfig is a container for all the connection specific paramters that you can set.

``` csharp

// The actual sender of the message. The broker is the owner of the organization certificate 
// used in the library. The broker id can be retrieved from your Digipost organization account.
var broker = new Broker(12345);

// The sender is what the receiver of the message sees as the sender of the message. 
// Sender and broker id will both be your organization's id if you are sending on behalf of yourself.
var sender = new Sender(67890);

var clientConfig = new ClientConfig(broker, Environment.Production);
var client = new DigipostClient(clientConfig, thumbprint: "84e492a972b7e...");

```