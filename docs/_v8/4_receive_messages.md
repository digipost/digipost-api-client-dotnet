---
title: Receive messages
identification: receivemessages
layout: default
---

### Client configuration

`ClientConfig` is a container for all the connection specific paramters that you can set.

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

The inbox call outputs a list of documents ordered on `DeliveryTime`. `Offset` is the start index of the list, and `limit` is the max number of documents to be returned. The `offset` and `limit` is therefore not in any way connected to `InboxDocument.Id`. 

The values `offset` and `limit` is meant for pagination so that one can fetch 100 and then the next 100. 


``` csharp

var inbox = client.GetInbox(sender);

var first100 = inbox.Fetch(); //Default offset is 0 and default limit is 100

var next200 = inbox.Fetch(offset: 100, limit: 100);

```

We have now fetched the 200 newest inbox documents. As long as no new documents are received, the two API-calls shown above will always return the same result. If we now receive a new document, this will change. The first 100 will now contain 1 new document and 99 documents we have seen before. This means that as soon as you stumble upon a document you have seen before you can stop processing, given that all the following older ones have been processed. 

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