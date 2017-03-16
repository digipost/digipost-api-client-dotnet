---
title: Receive messages
identification: receivemessages
layout: default
---

### Get documents inbox

``` csharp
var config = new ClientConfig("xxxxx", Environment.Qa);
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var inbox = client.Inbox;

var first100 = inbox.Fetch();

var next100 = inbox.Fetch(offset: 100, limit: 100);

```

### Download document content

``` csharp
var config = new ClientConfig("xxxxx", Environment.Qa);
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var inbox = client.Inbox;
var documentMetadata = (await inbox.Fetch()).First();

var documentStream = await inbox.FetchDocument(documentMetadata);

```

### Delete document

``` csharp
var config = new ClientConfig("xxxxx", Environment.Qa);
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var inbox = client.Inbox;
var documentMetadata = (await inbox.Fetch()).First();

await inbox.DeleteDocument(documentMetadata);
```