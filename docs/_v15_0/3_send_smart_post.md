---
title: Send smart post
identification: smartpost
layout: default
---


### Send a message with extra computer readable data

Starting with version 8 of the Digipost API, messages can have extra bits of computer readable information that
allows the creation of a customized, dynamic user experience for messages in Digipost. These extra bits of
information are referred to as "Datatypes".

All datatypes are sent in the same way. Each document can accommodate one datatype-object. An exhaustive list of
available datatypes and their documentation can be found at
[digipost/digipost-data-types-dotnet](https://github.com/digipost/digipost-data-types-dotnet).

### Send invoice or inkasso (Dept collection)

It is possible to send invoice-metadata with a document. Documents with invoice-metadata enables the _Send to Online Bank_ feature, which allows people to pay the invoice directly in Digipost.

```csharp
var invoice = new Invoice(dueDate: DateTime.Parse("2022-12-03T10:15:30+01:00 Europe/Paris"), sum: new decimal(100.21), creditorAccount: "2593143xxxx")
{
    Kid = "123123123"
};

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
    new Document(
        subject: "Invoice 1",
        fileType: "pdf",
        path: @"c:\...\invoice.pdf",
        dataType: invoice)
);

var result = client.SendMessage(message);
```

It is possible to send inkasso-metadata with a document.

```csharp
var inkasso = new Inkasso(dueDate: DateTime.Parse("2022-12-03T10:15:30+01:00 Europe/Paris"))
{
    Kid = "123123123",
    Sum = new decimal(100.21),
    Account = "2593143xxxx"
};

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
    new Document(
        subject: "Invoice 1",
        fileType: "pdf",
        path: @"c:\...\invoice.pdf",
        dataType: inkasso)
);

var result = client.SendMessage(message);
```

### Send message with appointment datatype

`Appointment` represents a meeting set for a specific place and time. The following example demonstrates how to include such extra data:

```csharp
var appointment = new Appointment(startTime: DateTime.Parse("2017-11-24T13:00:00+0100"))
{
    EndTime = DateTime.Parse("2017-11-24T13:00:00+0100").AddMinutes(30),
    Address = new Address {StreetAddress = "Storgata 1", PostalCode = "0001", City = "Oslo"}
};

var document = new Document(
    subject: "Your appointment",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: appointment
);

// Create Message and send using the client as specified in other examples.
```

### Send message with external link datatype

`ExternalLink` enhances a message in Digipost with a button which sends the user to an external site. The button
can optionally have a deadline, a description and a custom text.

```csharp
var externalLink = new ExternalLink(absoluteUri: new Uri("https://example.org/loan-offer/uniqueCustomerId/"))
{
    Description = "Please read the terms, and use the button above to accept them. The offer expires at 23/10-2018 10:00.",
    ButtonText = "Accept offer",
    Deadline = DateTime.Parse("2018-10-23T10:00:00+0200")
};

var document = new Document(
    subject: "Your appointment",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: externalLink
);

// Create Message and send using the client as specified in other examples.
```

### Datatype ShareDocumentsRequest

This datatype facilitates exchange of documents between an organisation and a Digipost end user. The sender
first sends a message of datatype ShareDocumentsRequest, to which the end user can attach a list of documents. When
new documents are shared, a DocumentEvent is generated. The organisation can retrieve the status of their
ShareDocumentsRequest. If documents are shared and the sharing is not cancelled, the documents can either be downloaded
or viewed on the digipostdata.no domain. Active requests can be cancelled both by the end user and the organisation.

The `purpose` attribute of the ShareDocumentsRequest should briefly explain why the sender organisation want to gain
access to the relevant documents. The primary document should contain a more detailed explanation.

#### Send ShareDocumentsRequest

```csharp
var shareDocReq = new ShareDocumentsRequest(
    maxShareDurationSeconds: 60 * 60 * 24 * 5,  // Five calendar days
    purpose: "The purpose for my use of the document");

var requestGuid = Guid.NewGuid(); // Keep this in your database as reference to this particular user interaction

var document = new Document(
    subject: "Information about document sharing",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: shareDocReq
)
{
    Guid = requestGuid.ToString()
};

var message = new Message(
    senderInformation.Sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "pdf", path: @"c:\...\attachment.pdf")
);

IMessageDeliveryResult result = client.SendMessage(message);
```

#### Be notified of new shared documents
The sender organisation can be notified of new shared documents by polling document events regularly. Use the `uuid` attribute
of the DocumentEvent to match with the `messageUUID` of the origin ShareDocumentsRequest.

This particular api is still not developed for .NET client. Periodically check the below api to check current status for 
sent shared request.

#### Get state of ShareDocumentsRequest

* Use the `requestGuid` that you stored from previous.

```csharp
var shareDocumentsRequestState = await client.GetDocumentSharing(sender)
                .GetShareDocumentsRequestState(requestGuid);
```

If the `shareDocumentsRequestState.SharedDocuments.Count() > 0` is true, then the user has shared a 
document for this share request.

#### Get documents

Each `SharedDocument` has attributes describing the document and its origin. If `SharedDocumentOrigin` is of type
`OrganisationOrigin`, the corresponding document was received by the end user through Digipost from the organisation
with the provided organisation number. If the origin is of type `PrivatePersonOrigin`, the document was received either
from another end user or uploaded by the user itself.

Get a single document as stream:

```csharp
Stream stream = await client.GetDocumentSharing(sender)
    .FetchSharedDocument(
        shareDocumentsRequestState.SharedDocuments.First().GetSharedDocumentContentStreamUri()
    );
```

Get link to view a single document on digipostdata.no:

```csharp
SharedDocumentContent sharedDocumentContent = await client.GetDocumentSharing(sender)
    .GetShareDocumentContent(
        shareDocumentsRequestState.SharedDocuments.First().GetSharedDocumentContentUri()
    );

Uri uriToOpenInBrowser = sharedDocumentContent.Uri;
```

#### Stop sharing

Use the `requestGuid` from before. Top stop the sharing you add more information on the original 
message you sent to start this process. Stop sharing can be done when you definitely don't need the
document to be shared any more. The sharing will in any case stop at the originally 
specified max share duration automatically. 

```csharp
var shareDocumentsRequestState = await client.GetDocumentSharing(sender)
                .GetShareDocumentsRequestState(requestGuid);

var additionalData = new AdditionalData(sender, new ShareDocumentsRequestSharingStopped());

client.AddAdditionalData(additionalData, shareDocumentsRequestState.GetStopSharingUri());
```
