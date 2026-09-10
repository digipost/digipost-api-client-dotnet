---
title: Logging
identification: logging
layout: default
---

### Debugging
#### Enabling Logging
The client library has the ability to log useful information that can be used for debug purposes. 
To enable logging, supply the `DigipostClient` with an implementation of `Microsoft.Extensions.Logging.ILoggerFactory`. 
This is Microsoft’s own logging API, and allows the user to chose their own logging framework.

Enabling logging on level `DEBUG` will output positive results of requests and worse, `WARN` only failed requests or worse, while `ERROR` will only occur on failed requests.
These loggers will be under the `Digipost.Api.Client namespace`.

#### Implementing using NLog
There are numerous ways to implement a logger, but the following examples will be based on [NLog documentation](https://github.com/NLog/NLog.Extensions.Logging/wiki/Getting-started-with-.NET-Core-2---Console-application).

1. Install the Nuget-packages `NLog`, `NLog.Extensions.Logging` and `Microsoft.Extensions.DependencyInjection`.
1. Create an `nlog.config` file. The following is an example that logs to both file and console:
{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>

<!-- XSD manual extracted from package NLog.Schema: https://www.nuget.org/packages/NLog.Schema-->
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xsi:schemaLocation="NLog NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogFile="c:\temp\console-example-internal.log"
      internalLogLevel="Info">
    <!-- the targets to write to -->
    <targets>
        <!-- write logs to file -->
        <target xsi:type="File"
                name="fileTarget"
                fileName="${specialfolder:folder=UserProfile}/logs/digipost-api-client-dotnet/digipost-api-client-dotnet.log"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}"
                archiveEvery="Day"
                archiveNumbering="Date"
                archiveDateFormat="yyyy-MM-dd"/>
        <target xsi:type="Console"
                name="consoleTarget"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}" />
    </targets>

    <!-- rules to map from logger name to target -->
    <rules>
        <logger name="*" minlevel="Trace" writeTo="fileTarget,consoleTarget"/>
    </rules>
</nlog>
{% endhighlight %}

In your application, do the following to create a logger and supply it to `DigipostClient`:

```csharp
private static IServiceProvider CreateServiceProviderAndSetUpLogging()
{
    var services = new ServiceCollection();

    services.AddSingleton<ILoggerFactory, LoggerFactory>();
    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

    var serviceProvider = services.BuildServiceProvider();
    SetUpLoggingForTesting(serviceProvider);

    return serviceProvider;
}

private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
    NLog.LogManager.LoadConfiguration("./nlog.config");
}

static void Main(string[] args)
{
    ClientConfig clientConfig = null;
    
    var serviceProvider = CreateServiceProviderAndSetUpLogging();
    var client = new DigipostClient(clientConfig, CertificateReader.ReadCertificate(), serviceProvider.GetService<ILoggerFactory>());
}
```


#### Request and Response Logging
For initial integration and debugging purposes, it can be useful to log the actual request and response going over the wire. This can be enabled by doing the following:

Set the property `ClientConfig.LogRequestAndResponse = true`.

> Warning: Enabling request logging should never be used in a production system. It will severely impact the performance of the client.
