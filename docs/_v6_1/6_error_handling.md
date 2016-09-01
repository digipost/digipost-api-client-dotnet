---
title: Error Handling
identification: errorhandling
layout: default
---

<h3 id="SynchronousError">Synchronous error handling</h3>
If you are communicating synchronously, the errors will be wrapped in an `AggregateException`. This is because a set of errors may have occured before you receive You can handle each exception within the aggregate by sending in an anonymous function to `AggregateException.Handle()`:

{% highlight csharp %}

try
{
	var messageDeliveryResult = api.SendMessage();	
}
catch (AggregateException ae)
{
	ae.Handle((x) =>
	 {
         if (x is ClientResponseException)
         {
             Console.WriteLine("A client response exception occured!");
             return true;
         }
         return false;
     });
}

{% endhighlight %}

<h3 id="AsynchronousError">Asynchronous error handling</h3>
If you are using methods in the client library that has `async` in the name, it means that you are communicating asynchronously.
To correctly identify errors that might happen, for example during sending a message, you can catch errors like you normally would in an application:

{% highlight csharp %}
try
{
	var messageDeliveryResult = api.SendMessage();	
}
catch(ClientResponseException e)
{
	Console.WriteLine("A client response exception occured!");
}

{% endhighlight %} 

Asynchronous communication with Digipost using the client library involves using methods with _async_ in method name, like IdentifyAsync.