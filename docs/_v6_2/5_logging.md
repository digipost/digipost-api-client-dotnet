---
title: Logging
identification: logging
layout: default
---

The need for application logging varies from project to project. The client library exposes the ability to set a separate log function on the [ClientConfig]({{site.baseurl}}/v6.1/#clientconfig) object. `ClientConfig.Logger` can be set to a `Action<TraceEventType, Guid?, String, String>`,  where `TraceEventType` is the type of log message is and `Guid` is Id of the message. The penultimate parameter is the method in which was logged, and the final parameter is the actual message.

Following is an example:

{% highlight csharp %}

var clientConfig = new ClientConfig()
{
    Logger = (severity, traceID, metode, message) =>
    {
        System.Diagnostics.Debug.WriteLine("{0} - {1} [{2}]", 
        	DateTime.Now, 
        	message, 
        	traceID.GetValueOrDefault()
        );
    }
};

{% endhighlight %}

The following will log all messages sent an received.

{% highlight csharp %}

clientConfig.DebugToFile = true;
clientConfig.StandardLogPath = @"\LoggPath";

{% endhighlight%}