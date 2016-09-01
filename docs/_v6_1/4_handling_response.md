---
title: Handling responses
identification: response
layout: default
---

The client library will return the most important status codes and the raw xml response.

- StatusMessage is the status of the messages.
- DeliveryTime is the time the digital mail was delivered. This will be set with 01/01/0001 00:00:00) if the message had errors.
- DelimeryMethod is the channel through which the message was delivered. This is either DIGIPOST for digital mail or PRINT for physical mail.
- ErrorCode is populated if relevant.
- ErrorType is the type of error.
- ResponseMessage is the the raw XML of the respnse.

Example of a response where the recipient is not a Digipost user:

{% highlight csharp%}

StatusMessage[The recipient does not have a Digipost account.]
Deliverytime[01/01/0001 00:00:00]
DeliveryMethod[]
ErrorCode[UNKNOWN_RECIPIENT]
ErrorType[CLIENT_DATA]
ResponseMessage[<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<error xmlns="http://api.digipost.no/schema/v6">
	<error-code>UNKNOWN_RECIPIENT</error-code>
	<error-message>The recipient does not have a Digipost account.</error-message>
	<error-type>CLIENT_DATA</error-type>
</error>]]

{% endhighlight%}

<h2 id="response">Response objects</h2>

The client library returns appropiate response objects for relevant methods.

<h3 id="identifyResp">Identify response</h3>

[IdentificationResult.cs](https://github.com/digipost/digipost-api-client-dotnet/blob/master/Digipost.Api.Client.Domain/Identification/IdentificationResult.cs) is populated by three properties:

*	__IdentificationResultCode__ , wich describes if the recipient is either identified, unidentified, invalid or an digipost-customer.
*	__IdentificationType__ is in wich type Digipost will return the identification. 
*	__IdentificationValue__ is the value of the IdentificationType. 

An example of response could be: 

	IdentificationResultCode = Digipost
	IdentificationType = Digipostaddress
	IdentificationValue = johnny.test#1234

Example of a response where the letter has been delivered as digital mail:

{% highlight csharp%}

StatusMessage[Delivered]
DeliveryTime[30/04/2015 14:54:07]
DeliveryMethod[DIGIPOST]
ErrorCode[]
ErrorType[]
ResponseMessage[<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<message-delivery xmlns="http://api.digipost.no/schema/v6">
	<delivery-method>DIGIPOST</delivery-method>
	<status>DELIVERED</status>
	<delivery-time>2015-04-30T14:54:07.788+02:00</delivery-time>
	<primary-document>
		<uuid>424882ee-4db8-4585-b701-69e6da10c956</uuid>
		<subject>Primary document</subject>
		<file-type>txt</file-type>
		<authentication-level>PASSWORD</authentication-level>
		<sensitivity-level>NORMAL</sensitivity-level>
		<content-hash hash-algorithm="SHA256">XXX=</content-hash>
	</primary-document>
	<attachment>
		<uuid>a3069d0c-fc1d-4966-b17d-9395d410c126</uuid>
		<subject>Attachment</subject>
		<file-type>txt</file-type>
		<authentication-level>PASSWORD</authentication-level>
		<sensitivity-level>NORMAL</sensitivity-level>
		<content-hash hash-algorithm="SHA256">XXXX</content-hash>
	</attachment>
</message-delivery>]]

{% endhighlight%}

<h3 id="messageResp">Send message response </h3>

[MessageDeliveryResult.cs](https://github.com/digipost/digipost-api-client-dotnet/blob/master/Digipost.Api.Client.Domain/Message/MessageDeliveryResult.cs) is populated by six properties:

*	__Deliverymethod__ is either digital or physical, where Print is physical and Digipost is digital.
*	__Status__ is the status of the message in the delivery prosess. See [MessageStatus.cs](https://github.com/digipost/digipost-api-client-dotnet/blob/master/Digipost.Api.Client.Domain/Enums/MessageStatus.cs) for more information.
*	__Deliverytime__ is the time the message was delivered to the recipient.
*	__Primarydocument__ is a copy of the document that was sent in, except for the actual byte content.
*	__Attachment__ is an list of the attachments that was sent inn, except for the actual byte content.
*	__Link__ is possible links/actions from here. In idempotent methods, this will return blank.

An example of response could be: 
{% highlight csharp %} 	
Deliverymethod = Digipost
Status = Delivered
Deliverytime = 18/05/2015 16:50:06
Primarydocument = 
	Guid = b3928de2-0555-46be-9a75-837b3290c152
	Subject = Primary document
	FileMimeType = txt
	SmsNotification =  
	AuthenticationLevel = Password
	SensitivityLevel = Normal
	TechnicalType = 
Attachment = 
	Guid = 670c162c-e1d0-4ee6-98db-917e53fc0a2a
	Subject = Attachment
	FileMimeType = txt
	SmsNotification =  
	AuthenticationLevel = Password
	SensitivityLevel = Normal
	TechnicalType = 

{% endhighlight%}