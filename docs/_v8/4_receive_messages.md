---
title: Receive messages
identification: receivemessages
layout: default
---

### Client configuration

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

### Get documents inbox

``` csharp

var inbox = client.GetInbox(sender);

var first100 = inbox.Fetch();

var next200 = inbox.Fetch(offset: 100, limit: 200);

```

### Download document content

``` csharp

var inbox = client.GetInbox(sender);

var documentMetadata = (await inbox.Fetch()).First();

var documentStream = await inbox.FetchDocument(documentMetadata);

```

### Delete document

``` csharp

var inbox = client.GetInbox(sender);

var documentMetadata = (await inbox.Fetch()).First();

await inbox.DeleteDocument(documentMetadata);

```