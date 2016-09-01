---
title: Use cases
identification: usecases
layout: default
---

<h3 id="uc01">Send one letter to recipient via personal identification number</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var message = new Message(
    new Recipient(IdentificationChoice.PersonalidentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", mimeType: "txt", path: @"c:\...\document.txt")
  );

var result = client.SendMessage(message); 

{% endhighlight %}

<h3 id="uc02">Send one letter to recipient via name and address</h3>

{% highlight csharp %}

//Init Client
var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

//Compose Recipient by name and address
var recipientByNameAndAddress = new RecipientByNameAndAddress(
    fullName: "Ola Nordmann",
    addressLine: "Prinsensveien 123",
    postalCode: "0460",
    city: "Oslo"
   );

//Compose message
var message = new Message(
    new Recipient(recipientByNameAndAddress),
    new Document(subject: "document subject", mimeType: "pdf", path: @"c:\...\document.pdf")
    );

var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc03">Send one letter with multiple attachments</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", mimeType: "pdf", path: @"c:\...\document.pdf");
var attachment1 = new Document(subject: "Attachment 1", mimeType: "txt", path: @"c:\...\attachment_01.txt");
var attachment2 = new Document(subject: "Attachment 2", mimeType: "pdf", path: @"c:\...\attachment_02.pdf");

var message = new Message(
    new Recipient(IdentificationChoice.PersonalidentificationNumber, id: "241084xxxxx"), primaryDocument
    ) { Attachments = { attachment1, attachment2 } };

var result = client.SendMessage(message);

Logging.Log(TraceEventType.Information, result.Status.ToString());

{% endhighlight %}

<h3 id="uc05">Send letter with SMS notification</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", mimeType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.SmsNotification = new SmsNotification(afterHours: 0); //SMS reminder after 0 hours
primaryDocument.SmsNotification.AddAtTime.Add(new Listedtime(new DateTime(2015, 05, 05, 12, 00, 00))); //new reminder at a specific date

var message = new Message(
    new Recipient(identificationChoice: IdentificationChoice.PersonalidentificationNumber, id: "311084xxxx"), primaryDocument);

var result = client.SendMessage(message);

Logging.Log(TraceEventType.Information, result.Status.ToString());

{% endhighlight %}

<h3 id="uc06">Send letter with fallback to print</h3>

In cases where the recipient is not a Digipost user, it is also possible to use the recipient's name and address for physical mail delivery.

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

//recipientIdentifier for digital mail
var recipientByNameAndAddress = new RecipientByNameAndAddress(
    fullName: "Ola Nordmann",
    postalCode: "0460",
    city: "Oslo",
    addressLine: "Prinsensveien 123");

//printdetails for fallback to print (physical mail)
var printDetails =
    new PrintDetails(
        recipient: new PrintRecipient(
            "Ola Nordmann",
            new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
        printReturnAddress: new PrintReturnAddress(
            "Kari Nordmann",
            new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
        );

//recipient
var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

var message = new Message(
    new Recipient(recipientByNameAndAddress, printDetails),
    new Document(subject: "document subject", mimeType: "pdf", path: @"c:\...\document.pdf")
   );

var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc07">Send letter with higher security level</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", mimeType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.AuthenticationLevel = AuthenticationLevel.TwoFactor; // Require BankID or BuyPass to open letter
primaryDocument.SensitivityLevel = SensitivityLevel.Sensitive; // Sender information and subject will be hidden until Digipost user is logged in at the appropriate authentication level

var message = new Message(
    new Recipient(identificationChoice: IdentificationChoice.PersonalidentificationNumber, id: "311084xxxx"), primaryDocument);

var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc08">Identify user by personal identification number</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "211084xxxx");
var identificationResponse = client.Identify(identification); 

{% endhighlight %}

<h3 id="uc09">Send letter through Norsk Helsenett</h3>

The Digipost API is accessible from both internet and Norsk Helsenett (NHN). Both entry points use the same API, the only difference is the base URL.

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

// API URL is different when request is sent from NHN
config.ApiUrl = new Uri("https://api.nhn.digipost.no");

var message = new Message(
    new Recipient(IdentificationChoice.PersonalidentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", mimeType: "txt", path: @"c:\...\document.txt")
  );

var result = client.SendMessage(message); 

{% endhighlight %}