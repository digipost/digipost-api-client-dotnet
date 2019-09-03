---
title: Error Handling
identification: errorhandling
layout: default
---

#### Error Handling
If you are communicating synchronously, the errors will be wrapped in an `AggregateException`. 
This is because a set of errors may have occured before you receive You can handle each exception within the aggregate by sending in an anonymous function to `AggregateException.Handle()`:
``` csharp
try
{
    var messageDeliveryResult = api.SendMessage(message);	
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

```

If you are using methods in the client library that has `async` in the name, it means that you are communicating asynchronously.
To correctly identify errors that might happen, for example during sending a message, you can catch errors like you normally would in an application:

``` csharp

try
{
    var messageDeliveryResult = await api.SendMessageAsync(message);	
}
catch(ClientResponseException e)
{
    Console.WriteLine("A client response exception occured!");
}
```

Asynchronous communication with Digipost using the client library involves using methods with `async` in method name, like `SendMessageAsync` or `IdentifyAsync`.
