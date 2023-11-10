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
