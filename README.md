# Assimalign .NET Extensions

**NOTE**: The .NET extensions within this library break-away from the Microsoft Platform and should be used with caution.

The .NET extensions within this repository are made-up of custom implementations and/or customized version of other Third-Party libraries. It should be important to note that while some of these libraries are a derivative of Microsoft Extensions and other popular third-party libraries they may not have the same implementation and could cause breaking changes if trying to swap out. 

The intent behind this repository is to create a standard ecosystem of commonly used libraries don't necessarly :

1. **Standardize and expose API's which are otherwise not exposed in other libraries,**
2. **Adhere to common designs patterns and principals for a more cohesive implementations across all libraries,**
3. 


that live within [dotnet/runtime](https://github.com/dotnet/runtime) and are being used, modified, and added to for experimental projects under Assimalign.

- [Assimalign .NET Extensions](#assimalign-net-extensions)
- [Available Extensions](#available-extensions)
- [Build Status](#build-status)
  - [Mapping](#mapping)


# Available Extensions 

|  Package ID														|  Latest Version  | Downloads |
| ------------------------------------------------------------------|----------------- | --------- |
| `Assimalign.Extensions `                              |[![core.nuget.version]][core.nuget.url]                                                       |[![core.nuget.downloads]][core.nuget.url]                                                        |
| `Assimalign.Extensions.Mapping`									      |[![mapping.nuget.version]][mapping.nuget.url]                                                 |[![mapping.nuget.downloads]][mapping.nuget.url]                                                  |
| `Assimalign.Extensions.Validation`								    |[![validation.nuget.version]][validation.nuget.url]                                           |[![validation.nuget.downloads]][validation.nuget.url]                                            |
| `Assimalign.Extensions.Validation.Configurable`			  |[![validation.configurable.nuget.version]][validation.configurable.nuget.url]                 |[![validation.configurable.nuget.downloads]][validation.configurable.nuget.url]                  |
| `Assimalign.Extensions.Validation.Configurable.Json`  |[![validation.configurable.json.nuget.version]][validation.configurable.json.nuget.url]       |[![validation.configurable.json.nuget.downloads]][validation.configurable.json.nuget.url]        |
| `Assimalign.Extensions.Validation.Configurable.Xml`   |[![validation.configurable.xml.nuget.version]][validation.configurable.xml.nuget.url]         |[![validation.configurable.xml.nuget.downloads]][validation.configurable.xml.nuget.url]          |
| `Assimalign.Extensions.Linq.Serialization`						|[![linq.serialization.nuget.version]][linq.serialization.nuget.url]                           |[![linq.serialization.nuget.downloads]][linq.serialization.nuget.url]                            |
| `Assimalign.Extensions.Linq.Serialization.Binary`	    |[![linq.serialization.binary.nuget.version]][linq.serialization.binary.nuget.url]             |[![linq.serialization.binary.nuget.downloads]][linq.serialization.binary.nuget.url]              |
| `Assimalign.Extensions.Linq.Serialization.Text`	      |[![linq.serialization.text.nuget.version]][linq.serialization.text.nuget.url]                 |[![linq.serialization.text.nuget.downloads]][linq.serialization.text.nuget.url]                  |
| `Assimalign.Extensions.Linq.Serialization.Json`	      |[![NuGet](https://img.shields.io/nuget/v/Assimalign.Extensions.Linq.Serialization.Json)](https://nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Json) | [![Nuget](https://img.shields.io/nuget/dt/Assimalign.Extensions.Linq.Serialization.Json)](https://nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Json) |
| `Assimalign.Extensions.Linq.Serialization.Xml`        |[![NuGet](https://img.shields.io/nuget/v/Assimalign.Extensions.Linq.Serialization.Xml)](https://nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Xml) | [![Nuget](https://img.shields.io/nuget/dt/Assimalign.Extensions.Linq.Serialization.Xml)](https://nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Xml) |
																	
# Build Status

## Mapping
| Package       |Build Status (By Branch) | Build Status                |
|---------------|-------------------------|-----------------------------|
|**Mapping**    |                         |                             |
|               |*origin/main*            |![mapping.ci.main]           |
|               |*origin/development*     |![mapping.ci.development]    |
|**Validation** |                         |                             |
|               |*origin/main*            |![validation.ci.main]        |
|               |*origin/development*     |![validation.ci.development] |



[core.nuget.url]:                                   https://www.nuget.org/packages/Assimalign.Extensions.Configuration
[core.nuget.version]:                               https://img.shields.io/nuget/v/Assimalign.Extensions.Configuration
[core.nuget.downloads]:                             https://img.shields.io/nuget/dt/Assimalign.Extensions.Configuration

[mapping.nuget.url]:                                https://www.nuget.org/packages/Assimalign.Extensions.Mapping
[mapping.nuget.version]:                            https://img.shields.io/nuget/v/Assimalign.Extensions.Mapping
[mapping.nuget.downloads]:                          https://img.shields.io/nuget/dt/Assimalign.Extensions.Mapping
[mapping.ci.main]:                                  https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.mapping.ci/main
[mapping.ci.development]:                           https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.mapping.ci/development

[validation.nuget.url]:                             https://www.nuget.org/packages/Assimalign.Extensions.Validation
[validation.nuget.version]:                         https://img.shields.io/nuget/v/Assimalign.Extensions.Validation
[validation.nuget.downloads]:                       https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation
[validation.ci.main]:                                  https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.ci/main
[validation.ci.development]:                           https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.ci/development


[validation.configurable.nuget.url]:                https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable
[validation.configurable.nuget.version]:            https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable
[validation.configurable.nuget.downloads]:          https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable

[validation.configurable.json.nuget.url]:           https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable.Json
[validation.configurable.json.nuget.version]:       https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable.Json
[validation.configurable.json.nuget.downloads]:     https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable.Json

[validation.configurable.xml.nuget.url]:            https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable.Xml
[validation.configurable.xml.nuget.version]:        https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable.Xml
[validation.configurable.xml.nuget.downloads]:      https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable.Xml

[linq.serialization.nuget.url]:                     https://www.nuget.org/packages/Assimalign.Extensions.Linq.Serialization
[linq.serialization.nuget.version]:                 https://img.shields.io/nuget/v/Assimalign.Extensions.Linq.Serialization
[linq.serialization.nuget.downloads]:               https://img.shields.io/nuget/dt/Assimalign.Extensions.Linq.Serialization

[linq.serialization.binary.nuget.url]:              https://www.nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Binary
[linq.serialization.binary.nuget.version]:          https://img.shields.io/nuget/v/Assimalign.Extensions.Linq.Serialization.Binary
[linq.serialization.binary.nuget.downloads]:        https://img.shields.io/nuget/dt/Assimalign.Extensions.Linq.Serialization.Binary

[linq.serialization.text.nuget.url]:                https://www.nuget.org/packages/Assimalign.Extensions.Linq.Serialization.Text
[linq.serialization.text.nuget.version]:            https://img.shields.io/nuget/v/Assimalign.Extensions.Linq.Serialization.Text
[linq.serialization.text.nuget.downloads]:          https://img.shields.io/nuget/dt/Assimalign.Extensions.Linq.Serialization.Text
