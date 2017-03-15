---
title: Logging
identification: logging
layout: default
---

<h3 id="loggingrequestflow">Logging request flow</h3>
The client is using Common Logging API for .NET as an abstraction for logging. It is up to the user to implement the API with a logging framework.

<blockquote>Common Logging API is a lightweight infrastructure logging platform that allows developers to focus on the logging requirements instead of the logging tools and required configuration. The Common Logging API abstracts the logging requirements of any project making it easy to swap logging providers.</blockquote>

Enabling logging on level `DEBUG` will output results of requests, `WARN` only failed requests or worse. These loggers will be under the `Digipost.Api.Client` namespace. 

<h3 id="log4net">Example with Log4Net</h3>

1. Let's assume that [log4net [1.2.13] 2.0.3](https://www.nuget.org/packages/log4net/2.0.3) is installed in project.
1. Since _log4net_ has version _[1.2.13]_, `Common.Logging.Log4Net1213` is the correct logging adapter. Install this package.
1. In some cases the adapter may install or update Log4Net to a incorrect version. If installing the adapter for _log4net 2.0.3_, _Common.Logging.Log4Net1213_ must be the installed version.
1. Following is a complete App.config with the Log4Net adapter containing a `RollingFileAppender`. Note the version of the adapter specified in the `<factoryAdapter/>` node:

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
