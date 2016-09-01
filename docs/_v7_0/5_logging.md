---
title: Logging
identification: logging
layout: default
---

<h3 id="loggingrequestflow">Logging request flow</h3>
The client is using Common Logging API for .NET as an abstraction for logging. It is up to the user to implement the API with a logging framework.

<blockquote>Common Logging API is a lightweight infrastructure logging platform that allows developers to focus on the logging requirements instead of the logging tools and required configuration. The Common Logging API abstracts the logging requirements of any project making it easy to swap logging providers.</blockquote>

Enabling logging on level `DEBUG` will output results of requests, `WARN` only failed requests or worse. These loggers will be under the `Digipost.Api.Client` namespace. 

<h3 id="log4net">Implementing Log4Net</h3>
1. Install Nuget-package `Common.Logging.Log4Net`. This will install the dependencies `Common.Logging.Core` and `Common.Logging`. Note that the versioning of Log4Net is a bit odd, but Nuget Gallery will reveal that Log4Net 2.0.3 has _Log4net [1.2.13] 2.0.3_ as package name. This means that `Common.Logging.Log4Net1213` is the correct logging adapter.
2. In some cases the adapter may install the wrong version of Log4Net. If installing the adapter for 2.0.3, the version must be upped to this version too.

Complete App.config with the Log4Net adapter installed and a `RollingFileAppender`:
{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>

  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter, Common.Logging.Log4net1213">
        <arg key="configType" value="INLINE" />
      </factoryAdapter>
    </logging>
  </common>

   <log4net>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <file value="${AppData}\Digipost\Client\" />
      <appendToFile value="true" />
      <rollingStyle value="Date" />
      <staticLogFileName value="false" />
      <rollingStyle value="Composite" />
      <param name="maxSizeRollBackups" value="10" />
      <datePattern value="yyyy.MM.dd' digipost-api-client-dotnet.log'" />
      <maximumFileSize value="100MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
   <root>
      <appender-ref ref="RollingFileAppender"/>
    </root>
  </log4net>
</configuration>

{% endhighlight %}
