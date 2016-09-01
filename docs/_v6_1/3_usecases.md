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
    new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
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
    new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
    );

var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc03">Send one letter with multiple attachments</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");
var attachment1 = new Document(subject: "Attachment 1", fileType: "txt", path: @"c:\...\attachment_01.txt");
var attachment2 = new Document(subject: "Attachment 2", fileType: "pdf", path: @"c:\...\attachment_02.pdf");

var message = new Message(
    new Recipient(IdentificationChoiceType.PersonalidentificationNumber, id: "241084xxxxx"), primaryDocument
    ) { Attachments = { attachment1, attachment2 } };

var result = client.SendMessage(message);

Logging.Log(TraceEventType.Information, result.Status.ToString());

{% endhighlight %}

<h3 id="uc05">Send letter with SMS notification</h3>

{% highlight csharp %}
var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.SmsNotification = new SmsNotification(afterHours: 0); //SMS reminder after 0 hours
primaryDocument.SmsNotification.NotifyAtTimes.Add(new DateTime(2015, 05, 05, 12, 00, 00)); //new reminder at a specific date

var message = new Message(
    new Recipient(identificationChoiceType: IdentificationChoiceType.PersonalidentificationNumber, id: "311084xxxx"), primaryDocument);

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
     new PrintDetails(printRecipient: new PrintRecipient(
             "Ola Nordmann",
             new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")), 
             printReturnRecipient: new PrintReturnRecipient(
             "Kari Nordmann",
             new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
         );

 //recipient
 var digitalRecipientWithFallbackPrint = new Recipient(recipientByNameAndAddress, printDetails);

 var message = new Message(
     new Recipient(recipientByNameAndAddress, printDetails),
     new Document(subject: "document subject", fileType: "pdf", path: @"c:\...\document.pdf")
    );

 var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc07">Send letter with higher security level</h3>

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var primaryDocument = new Document(subject: "Primary document", fileType: "pdf", path: @"c:\...\document.pdf");

primaryDocument.AuthenticationLevel = AuthenticationLevel.TwoFactor; // Require BankID or BuyPass to open letter
primaryDocument.SensitivityLevel = SensitivityLevel.Sensitive; // Sender information and subject will be hidden until Digipost user is logged in at the appropriate authentication level

var message = new Message(
    new Recipient(identificationChoiceType: IdentificationChoiceType.PersonalidentificationNumber, id: "311084xxxx"), primaryDocument);

var result = client.SendMessage(message);

{% endhighlight %}

<h3 id="uc08">Identify recipient</h3>

Identify recipient attempts to identify the person submitted in the request and return information regarding whether or not the person is a Digipost user. The person can be identified by personal identification number (fødelsnummer), Digipost address, or name and address. If personal identification number is used then true/false will be returned regarding whether or not the person is a Digipost user. If other identification choices are used the actual Digipost adresse will be returned.

Following is a example that uses personal identification number as identification choice.

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var identification = new Identification(IdentificationChoiceType.PersonalidentificationNumber, "211084xxxxx");
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
    new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
  );

var result = client.SendMessage(message); 

{% endhighlight %}

<h3 id="uc10">Send invoice</h3>

It is possible to send invoice-metadata with a document. Documents with invoice-metadata enables the _Send to Online Bank_ feature, which allows people to pay the invoice directly in Digipost.

{% highlight csharp %}

var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");
            
var message = new Message(
    new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "211084xxxx"),
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

{% endhighlight %}

<h3 id="uc11">Search for receivers</h3>
A central part of a user interface in the application that is integrating with Digipost is the possiblity to search for receivers. This is available via the search endpoint. A person can be found by simply searching by first name and last name, e.g. <code>Ola Nordmann</code>, or specified further by street address, postal code, city and organization name.

It is important to note that the search results returned do not necessarily include the receiver to which you actually wish to send. The search results returned are strictly based on the search query you have sent in. This equally applies when only one search result is returned. This means that the actual person to which you wish to send must be confirmed by a human being before the actual document i sent (typically in the senders application). If the goal is to create a 100% automated workflow then the identify recipient endpoint should be used (see Identify recipient use case).

{% highlight csharp %}
 var config = new ClientConfig(senderId: "xxxxx");
 var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

 var response = client.Search("Ola Nordmann Bellsund Longyearbyen");

 foreach (var person in response.PersonDetails)
 {
     var digipostAddress = person.DigipostAddress;
     var phoneNumber = person.MobileNumber;
 }
{% endhighlight %}

<h3 id="uc12">Send on behalf of organization</h3>
In the following use case, sender is defined as the party who is responsible for the actual content of the letter. Broker is defined as the party who is responsible for the technical transaction, which in this context means creating the request and being the party that is authenticated.

![example]({{ site.baseurl}}/assets/images/sender_broker_digipost.png)

Sending on behalf of an organization is accomplished by setting `Message.SenderId` to the id of the sender when constructing a message. The actual letter will appear in the receivers Digipost mailbox with the senders details (logo, name, etc.). 

<blockquote> Remember to use the business certificate of the broker to sign the message, not the one belonging to the sender. Also, the proper permissions need to be set by Digipost to send on behalf of an organization.</blockquote>

Let us illustrate this with an example. Let _BrokerCompany_ be an organization with id _112233_, and thumbprint of their certificate _84e492a972b7e..._. They want to send on behalf of _SenderCompany_ with organization id _5555_.  

{% highlight csharp %}
var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");
var digitalRecipient = new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "311084xxxx");
var primaryDocument = new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt");

var message = new Message(recipient: digitalRecipient, primaryDocument: primaryDocument, senderId: "5555");
{% endhighlight %}


<h3 id="uc13">Send message with delivery time</h3>
A message can be sent with a delivery time. This means that a message can be sent at 11 AM, and be delivered to the recipient's Digipost inbox at 3 PM the next day. `Message.DeliveryTime` is used for this purpose and is of type `DateTime`. This gives you a lot of flexibility on how to set the delivery time.

{% highlight csharp%}
var config = new ClientConfig(senderId: "xxxxx");
var client = new DigipostClient(config, thumbprint: "84e492a972b7e...");

var message = new Message(
    new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "311084xxxx"),
    new Document(subject: "Attachment", fileType: "txt", path: @"c:\...\document.txt")
    ) {DeliveryTime = DateTime.Now.AddDays(1).AddHours(4)};

var result = client.SendMessage(message);
{% endhighlight %}