---
title: Send messages
identification: usecases
layout: default
---

### Client configuration

`ClientConfig` is a container for all the connection specific paramters that you can set.

``` csharp

// The actual sender of the message. The broker is the owner of the organization certificate 
// used in the library. The broker id can be retrieved from your Digipost organization account.
const int brokerId = 12345;
            
var broker = new Broker(brokerId);
var clientConfig = new ClientConfig(broker, Environment.Production);

var client = new DigipostClient(clientConfig, CertificateReader.ReadCertificate());

```

### Send one letter to recipient via personal identification number

``` csharp

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
);

var result = client.SendMessage(message);

```
### Other recipient types
There are other recipient types available to identify recipients of messages. Note that some recipient types may 
require special permissions to be set up in order to be used.

``` csharp

var recipient = new RecipientByNameAndAddress(
    fullName: "Ola Nordmann",
    addressLine1: "Prinsensveien 123",
    postalCode: "0460",
    city: "Oslo"
);

var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

var message = new Message(sender, recipient, primaryDocument);
var result = client.SendMessage(message);

```

### Send one letter with multiple attachments

``` csharp

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");
var attachment1 = new Document(subject: "Attachment 1", fileType: "txt", path: @"c:\...\attachment_01.txt");
var attachment2 = new Document(subject: "Attachment 2", fileType: "pdf", path: @"c:\...\attachment_02.pdf");

var message = new Message(
        sender,
        new RecipientById(IdentificationType.PersonalIdentificationNumber, id: "241084xxxxx"),
        primaryDocument
    ){Attachments = {attachment1, attachment2}};

var result = client.SendMessage(message);

```

### Send letter with SMS notification

``` csharp

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.SmsNotification = new SmsNotification(afterHours: 0); //SMS reminder after 0 hours
primaryDocument.SmsNotification.NotifyAtTimes.Add(new DateTime(2015, 05, 05, 12, 00, 00)); //new reminder at a specific date

var message = new Message(
    sender,
    new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx"),
    primaryDocument
);

var result = client.SendMessage(message);

```

### Send letter with fallback to print if the user does not exist in Digipost

In cases where the recipient is not a Digipost user, it is also possible to use the recipient's name and address for physical mail delivery.

``` csharp

var recipient = new RecipientByNameAndAddress(
    fullName: "Ola Nordmann",
    addressLine1: "Prinsensveien 123",
    postalCode: "0460",
    city: "Oslo"
);

var printDetails =
    new PrintDetails(
        printRecipient: new PrintRecipient(
            "Ola Nordmann",
            new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
        printReturnRecipient: new PrintReturnRecipient(
            "Kari Nordmann",
            new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
    );

var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

var messageWithFallbackToPrint = new Message(sender, recipient, primaryDocument)
{
    PrintDetails = printDetails
};

var result = client.SendMessage(messageWithFallbackToPrint);

```
### Send letter with fallback to print if the user does not read the message within a certain deadline
``` csharp

var recipient = new RecipientByNameAndAddress(
    fullName: "Ola Nordmann",
    addressLine1: "Prinsensveien 123",
    postalCode: "0460",
    city: "Oslo"
);

var printDetails =
    new PrintDetails(
        printRecipient: new PrintRecipient(
            "Ola Nordmann",
            new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
        printReturnRecipient: new PrintReturnRecipient(
            "Kari Nordmann",
            new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
    );

var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf");

var messageWithPrintIfUnread = new Message(sender, recipient, primaryDocument)
{
    PrintDetails = printDetails,
    DeliveryTime = DateTime.Now.AddDays(3),
    PrintIfUnread = new PrintIfUnread(DateTime.Now.AddDays(6), printDetails)
};

var result = client.SendMessage(messageWithPrintIfUnread);
```

### Send letter with higher security level

``` csharp

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf")
{
    AuthenticationLevel = AuthenticationLevel.TwoFactor, // Require BankID or BuyPass to open letter
    SensitivityLevel = SensitivityLevel.Sensitive // Sender information and subject will be hidden until Digipost user is logged in at the appropriate authentication level
};

var message = new Message(
    sender,
    new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx"),
    primaryDocument
);

var result = client.SendMessage(message);

```

### Send letter with higher security level

``` csharp

var config = new ClientConfig("xxxxx", Environment.Production);
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.AuthenticationLevel = AuthenticationLevel.TwoFactor; // Require BankID or BuyPass to open letter
primaryDocument.SensitivityLevel = SensitivityLevel.Sensitive; // Sender information and subject will be hidden until Digipost user is logged in at the appropriate authentication level

var message = new Message(
    new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx"), primaryDocument);

var result = client.SendMessage(message);

```

### Identify recipient

Attempts to identify the person submitted in the request and returns whether he or she has a Digipost account. The person can be identified by personal identification number (PIN), Digipost address, or name and address.

If the user is identified, the `ResultType` will be `DigipostAddress` or `PersonAlias`. In cases where we identify the person, the data parameter will contain the given identification value.

User is identified and have a Digipost account:

``` csharp
ResultType: DigipostAddress
Data: "Ola.Nordmann#3244B"
Error: Null
```

User is identified but does not have a Digipost account:

``` csharp
ResultType: PersonAlias
Data: "azdixsdfsdffsdfncixtvpwdp#6QE6"
Error: Null
```

The `PersonAlias` can be used for feature lookups, instead of the given identify criteria.

<blockquote>
Note: If you identify a person by PIN, the Digipost address will not be returned, since it is preferred that you continue to use this as identificator when sending the message.
</blockquote>

If the user is not identified, the `ResultType` will be `InvalidReason` or `UnidentifiedReason`. See the Error parameter for more detailed error message.

The user is not identified because the PIN is not valid:

``` csharp
ResultType: InvalidReason
Data: Null
Error: InvalidPersonalIdentificationNumber
```

The user is not identified because we did not have a match from the identify criteria:

``` csharp
ResultType: UnidentifiedReason
Data: Null
Error: NotFound
```

Following is a example that uses personal identification number as identification choice.

``` csharp
var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxxx"));
var identificationResponse = client.Identify(identification);

if (identificationResponse.ResultType == IdentificationResultType.DigipostAddress)
{
    //Exist as user in Digipost.
    //If you used personal identification number to identify - use this to send a message to this individual.
    //If not, see Data field for DigipostAddress.  
}
else if (identificationResponse.ResultType == IdentificationResultType.Personalias)
{
    //The person is identified but does not have an active Digipost account.
    //You can continue to use this alias to check the status of the user in future calls.
}
else if (identificationResponse.ResultType == IdentificationResultType.InvalidReason ||
         identificationResponse.ResultType == IdentificationResultType.UnidentifiedReason)
{
    //The person is NOT identified. Check Error for more details.
}

```

### Send letter through Norsk Helsenett

The Digipost API is accessible from both internet and Norsk Helsenett (NHN). Both entry points use the same API, the only difference is that `ClientConfig.Environment` must be set to `Environment.NorskHelsenett`.

``` csharp

// API URL is different when request is sent from NHN
var config = new ClientConfig(new Broker(12345), Environment.NorskHelsenett);
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
);

var result = client.SendMessage(message);

```

### Send invoice

It is possible to send invoice-metadata with a document. Documents with invoice-metadata enables the _Send to Online Bank_ feature, which allows people to pay the invoice directly in Digipost.

``` csharp

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "211084xxxx"),
    new Invoice(
        subject: "Invoice 1",
        fileType: "pdf",
        path: @"c:\...\invoice.pdf",
        amount: new decimal(100.21),
        account: "2593143xxxx",
        duedate: DateTime.Parse("01.01.2016"),
        kid: "123123123")
);

var result = client.SendMessage(message);
```

### Search for receivers
A central part of a user interface in the application that is integrating with Digipost is the possiblity to search for receivers. This is available via the search endpoint. A person can be found by simply searching by first name and last name, e.g. <code>Ola Nordmann</code>, or specified further by street address, postal code, city and organization name.

It is important to note that the search results returned do not necessarily include the receiver to which you actually wish to send. The search results returned are strictly based on the search query you have sent in. This equally applies when only one search result is returned. This means that the actual person to which you wish to send must be confirmed by a human being before the actual document i sent (typically in the senders application). If the goal is to create a 100% automated workflow then the identify recipient endpoint should be used (see Identify recipient use case).

``` csharp
var response = client.Search("Ola Nordmann Bellsund Longyearbyen");

foreach (var person in response.PersonDetails)
{
    var digipostAddress = person.DigipostAddress;
    var phoneNumber = person.MobileNumber;
}

```

### Send on behalf of organization
In the following use case, `Sender` is defined as the party who is responsible for the actual content of the letter. `Broker` is defined as the party who is responsible for the technical transaction, which in this context means creating the request and being the party that is authenticated.

![example]({{ site.baseurl}}/assets/images/sender_broker_digipost.png)

Sending on behalf of an organization is accomplished by setting `Message.Sender` to the id of the sender when constructing a message. The actual letter will appear in the receivers Digipost mailbox with the senders details (logo, name, etc.).

<blockquote> Remember to use the enterprise certificate of the broker to sign the message, not the one belonging to the sender. Also, the proper permissions need to be set by Digipost to send on behalf of an organization.</blockquote>

Let us illustrate this with an example. Let _BrokerCompany_ be an organization with id _12345_, and thumbprint of their certificate _84e492a972b7e..._. They want to send on behalf of _SenderCompany_ with organization id _67890_.  

``` csharp

var broker = new Broker(12345);
var sender = new Sender(67890);

var digitalRecipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx");
var primaryDocument = new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt");


var clientConfig = new ClientConfig(broker, Environment.Production);

var message = new Message(sender, digitalRecipient, primaryDocument);

var result = client.SendMessage(message);

```

### Send message with delivery time

A message can be sent with a delivery time. This means that a message can be sent at 11 AM, and be delivered to the recipient's Digipost inbox at 3 PM the next day. `Message.DeliveryTime` is used for this purpose and is of type `DateTime`. This gives you a lot of flexibility on how to set the delivery time.

``` csharp

var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
)
{
    DeliveryTime = DateTime.Now.AddDays(1).AddHours(4)
};

var result = client.SendMessage(message);

```

### Send a message with extra computer readable data

Starting with version 8 of the Digipost API, messages can have extra bits of computer readable information that
allows the creation of a customized, dynamic user experience for messages in Digipost. These extra bits of
information are referred to as "Datatypes".

All datatypes are sent in the same way. Each document can accommodate one datatype-object. An exhaustive list of
available datatypes and their documentation can be found at
[digipost/digipost-data-types](https://github.com/digipost/digipost-data-types).

### Send message with appointment datatype

One of the complex data types supported is `Appointment`, which represents a meeting set for a specific place and time. The following example demonstrates how to include such extra data:

``` csharp

var startTime = DateTime.Parse("2017-11-24T13:00:00+0100");
var appointment = new Appointment(startTime)
{
    EndTime = startTime.AddMinutes(30),
    AppointmentAddress = new AppointmentAddress("Storgata 1", "0001", "Oslo")
};

var document = new Document(
    subject: "Your appointment",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: appointment
);

// Create Message and send using the client as specified in other examples.

```
### Send message with event datatype

One of the complex data types supported is `Event`, which represents a meeting set for a specific place, but covering multiple time spans. The following example demonstrates how to include such extra data:

``` csharp
var startTime = DateTime.Parse("2017-11-24T13:00:00+0100");
            
var eventTimeSpans = new List<EventTimeSpan>();
var timeSpan = new EventTimeSpan(DateTime.Today, DateTime.Today.AddHours(3));
var timeSpan2 = new EventTimeSpan(DateTime.Today.AddDays(1), DateTime.Today.AddDays(1).AddHours(5));
eventTimeSpans.Add(timeSpan);
eventTimeSpans.Add(timeSpan2);
            
var barcode = new EventBarcode("12345678", "insert type here", "this is a code", true);
var address = new EventAddress("Gateveien 1", "0001", "Oslo");
var info = new Info("Title", "Very important information");
var links = new List<ExternalLink>();

var _event = new Event(eventTimeSpans)

    Description = "Description here",
    Address = address,
    Info = new List<Info>
    {
        info
    },
    Place = "Oslo City Røntgen",
    PlaceLabel = "This is a place",
    SubTitle = "SubTitle",
    Barcode = barcode,
    BarcodeLabel = "Barcode Label",
    Links = links
};

var document = new Document(
    subject: "Your appointment",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: _event
);

// Create Message and send using the client as specified in other examples.

```
### Send message with external link datatype

This Datatype enhances a message in Digipost with a button which sends the user to an external site. The button
can optionally have a deadline, a description and a custom text.

``` csharp

var linkTarget = new Uri("https://example.org/loan-offer/uniqueCustomerId/");
var externalLink = new ExternalLink(
    url: linkTarget,
    description: "Please read the terms, and use the button above to accept them. The offer expires at 23/10-2018 10:00.",
    buttonText: "Accept offer"
)
{
    Deadline = DateTime.Parse("2018-10-23T10:00:00+0200"),
};

var document = new Document(
    subject: "Your appointment",
    fileType: "pdf",
    path: @"c:\...\document.pdf",
    dataType: externalLink
);

// Create Message and send using the client as specified in other examples.

```
