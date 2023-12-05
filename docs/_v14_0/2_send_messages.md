---
title: Send messages
identification: usecases
layout: default
---

### Client configuration

`ClientConfig` is a container for all the connection specific parameters that you can set.

```csharp
// The actual sender of the message. The broker is the owner of the organization certificate
// used in the library. The broker id can be retrieved from your Digipost organization account.
var broker = new Broker(12345);

// The sender is what the receiver of the message sees as the sender of the message.
// Sender and broker id will both be your organization's id if you are sending on behalf of yourself.
var sender = new Sender(67890);

var clientConfig = new ClientConfig(broker, Environment.Production);

var client = new DigipostClient(clientConfig, CertificateReader.ReadCertificate());
```

### Send one letter to recipient via personal identification number

```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

### Send message with request for registration

It is possible to send a message to a person who does not have a Digipost account, where the message triggers
an SMS notification with a request for registration. The SMS notification says that if they register for a
Digipost account the document will be delivered digitally.

The actual content of the SMS is not part of the request, it is stored as part of the Digipost sender account and must be agreed upon with Digipost, as well as the SMS sender ID or phone number.

If the user does not register for a Digipost account within the defined deadline, the document will be either delivered as physical mail or not at all.
Be aware that using `PersonalIdentificationNumber` is required as Recipient to be able to deliver the document to the correct person.

The phone number provided SHOULD include the country code (i.e. +47). If the phone number does not start with either `"+"`, `"00"` or `"011"`, we will prepend `"+47"` if and only if the phone number string is 8 characters long. If this is not the case, the request is rejected.

In the following the document will be delivered as physical mail by Digipost if the recipient has not registered for a Digipost account by the defined deadline:

```csharp
var recipient = new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx");
var documentGuid = Guid.NewGuid();

var requestForRegistration = new RequestForRegistration(
    DateTime.Now.AddDays(3),
    "+4711223344",
    null,
    new PrintDetails(
        printRecipient: new PrintRecipient(
            "Ola Nordmann",
            new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
        printReturnRecipient: new PrintReturnRecipient(
            "Kari Nordmann",
            new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
    )
);

var primaryDocument = new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
{
    Guid = documentGuid.ToString()
};

var messageWithRequestForRegistration = new Message(sender, recipient, primaryDocument)
{
    RequestForRegistration = requestForRegistration
};

var result = client.SendMessage(messageWithRequestForRegistration);
```

If the sender wishes to send the document as physical mail through its own
service, print details _must not be included_ but be `null` instead.

```csharp
var recipient = new RecipientById(identificationType: IdentificationType.PersonalIdentificationNumber, id: "311084xxxx");
var documentGuid = Guid.NewGuid();

var requestForRegistration = new RequestForRegistration(
    DateTime.Now.AddDays(3),
    "+4711223344",
    null,
    null
);
```

In this case the status of the delivery can be checked with the following:

```csharp
var documentStatus = _digipostClient.GetDocumentStatus(sender).GetDocumentStatus(documentGuid).Result;
```

If documentDeliveryStatus is still "NOT_DELIVERED" after the expiry date, you know that the user did not register a Digipost account and the document is not delivered.

The documentGuid is the same as the one used when the originating message was sent.

### Send letter with fallback to print if the user does not read the message within a certain deadline

```csharp
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

### Send letter to print without any Digipost addressing information (directly to print)

```csharp

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

var messageToPrint = new PrintMessage(sender, printDetails, primaryDocument)

var result = client.SendMessage(messageToPrint);
// MessageStatus.DeliveredToPrint
```

### Send letter with higher security level

```csharp
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

```csharp
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

```csharp
ResultType: DigipostAddress
Data: "Ola.Nordmann#3244B"
Error: Null
```

User is identified but does not have a Digipost account:

```csharp
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

```csharp
ResultType: InvalidReason
Data: Null
Error: InvalidPersonalIdentificationNumber
```

The user is not identified because we did not have a match from the identify criteria:

```csharp
ResultType: UnidentifiedReason
Data: Null
Error: NotFound
```

Following is a example that uses personal identification number as identification choice.

```csharp
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

```csharp
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

### Search for receivers

A central part of a user interface in the application that is integrating with Digipost is the possiblity to search for receivers. This is available via the search endpoint. A person can be found by simply searching by first name and last name, e.g. <code>Ola Nordmann</code>, or specified further by street address, postal code, city and organization name.

It is important to note that the search results returned do not necessarily include the receiver to which you actually wish to send. The search results returned are strictly based on the search query you have sent in. This equally applies when only one search result is returned. This means that the actual person to which you wish to send must be confirmed by a human being before the actual document i sent (typically in the senders application). If the goal is to create a 100% automated workflow then the identify recipient endpoint should be used (see Identify recipient use case).

```csharp
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

```csharp
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

```csharp
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

### Get status of a sent document

You can for any given document check its status to see if it and when has been delivered, status of
read approval, delivery method etc.

To do this you need to have the Guid given to the document.

Send the message:

```csharp
var documentGuid = Guid.NewGuid();
var message = new Message(
    sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
    {
        Guid = documentGuid.ToString()
    }
);

client.SendMessage(message);
```

To fetch fhe DocumentStatus later:

```csharp
var documentStatus = _digipostClient.GetDocumentStatus(sender).GetDocumentStatus(documentGuid).Result;
```

This can be useful if you use fallback to print, print-if-unread, request for registration etc.

### Get Sender by organisation number and a part id and send a message

In very specific usecases where a broker organisation has multiple sub-organisations in Digipost
it is possible to send with organisation number and a partid. The partid can be used to distinguish
between different divisions or however the organisation sees fit. This makes it possible
to not have to store the Digipost account id, but in stead fetch this information from the api.

Fetch sender information based on orgnumber and partid.
```csharp
var senderInformation = client.GetSenderInformation(new SenderOrganisation("9876543210", "thePartId"));

var message = new Message(
    senderInformation.Sender,
    new RecipientById(IdentificationType.PersonalIdentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
);

var result = client.SendMessage(message);
```

### Get document events

Document events is special events created by Digipost when certain events happens to a document
that the broker/sender could use to perform other operations with regards to the processing of
the document. 

When a sender send a RequestForRegistration one might be interested in knowing IF the user
has registered with in the timeout specified to for instance print the message or do som other
processing. When the timeout is reached, Digipost will create a DocumentEvent 
with type `RequestForRegistrationExpired`. This can then be fetched by the sender via DocumentEvent-api.

This is done by polling in certain intervals defined by the sender. 

The following code fetches at max 100 events last 5 minutes.
```csharp
var from = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
var to = DateTime.Now;

DocumentEvents events = await Client.DocumentsApi(Sender)
                .GetDocumentEvents(from, to, offset: 0, maxResults: 100);

IEnumerable<DocumentEventType> documentEventTypes = events.Select(aEvent => aEvent.EventType);
```

Please note that this does not mean that you return all events in the time interval, but the maxResult you ask for. This
means that _if_ you get, say, 100 events in the list back, there is a possibility that there are more events in the 
specified time interval. You then need to fetch the same interval again, but specify an offset according to you maxResult.

Note that we here specify offset as 100 and use the same DateTime instance as used above. 
```csharp
DocumentEvents events = await Client.DocumentsApi(Sender)
                .GetDocumentEvents(from: from, to: to, offset: 100, maxResults: 100);

IEnumerable<DocumentEventType> documentEventTypes = events.Select(aEvent => aEvent.EventType);
```

Usually we want the you to pull as few events at a time that seems reasonable since you need to process the events 
somehow while pulling more.

When you receive 0 og less that the specified maxResults, you have received all events in the current time interval.

We propose that you somehow store the state of the polling time interval. We expire/delete events after 90 days, so there
are enough time to be sure you have processed them all. There is no way to delete an event through the api.
