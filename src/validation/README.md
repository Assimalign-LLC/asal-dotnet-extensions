- [Overview](#overview)
- [Nuget Packages](#nuget-packages)
  - [Validation Library](#validation-library)
  - [Configurable Validation Library](#configurable-validation-library)
  - [Configurable JSON Validation Library](#configurable-json-validation-library)
  - [Configurable XML Validation Library](#configurable-xml-validation-library)

# Overview
The validation extensions within this section offers a code-first and configuration-first implementation for validating objects. Within the code first library the APIs are exposed via fluent interfaces which should somewhat mirror the commonly popular [FluentValidation](https://github.com/FluentValidation/FluentValidation) library. However it is important to note that this library was completely rebuilt and may not exactly offer all the functionality in Fluent [FluentValidation](https://github.com/FluentValidation/FluentValidation). Within the configuration-first libraries the supported content types are JSON and XML which should cover main variety of validation needs. 


# Nuget Packages
Below is a list of available packages within [NuGet](https://www.nuget.org/).

|  Package ID											|                                   Latest Version                                      |                                            Downloads                                            | 
| ------------------------------------------------------|---------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------ |
| `Assimalign.Extensions.Validation`					|[![validation.nuget.version]][validation.nuget.url]                                    |[![validation.nuget.downloads]][validation.nuget.url]                                            |
| `Assimalign.Extensions.Validation.Configurable`		|[![validation.configurable.nuget.version]][validation.configurable.nuget.url]          |[![validation.configurable.nuget.downloads]][validation.configurable.nuget.url]                  |
| `Assimalign.Extensions.Validation.Configurable.Json`  |[![validation.configurable.json.nuget.version]][validation.configurable.json.nuget.url]|[![validation.configurable.json.nuget.downloads]][validation.configurable.json.nuget.url]        |
| `Assimalign.Extensions.Validation.Configurable.Xml`   |[![validation.configurable.xml.nuget.version]][validation.configurable.xml.nuget.url]  |[![validation.configurable.xml.nuget.downloads]][validation.configurable.xml.nuget.url]          |

<br/>

---

## Validation Library
>### **Overview**
> A library that allows for fluent validation.
> ### **Build Information**
>|Build Status (By Branch) | Build Status                |
>|-------------------------|-----------------------------|
>|*origin/main*            |![validation.ci.main]        |
>|*origin/development*     |![validation.ci.development] |

<br/>

---

## Configurable Validation Library
> ### **Overview**
> This library represents an abstraction for building a configurable validation provider.
> ### **Build Information**
>|Build Status (By Branch) | Build Status                |
>|-------------------------|-----------------------------|
>|*origin/main*            |![validation.configurable.ci.main]        |
>|*origin/development*     |![validation.configurable.ci.development] |

<br/>

---
## Configurable JSON Validation Library
> ### **Overview**
> The JSON Validation Library offers a configurable validation provider via json.
> ### **Build Information**
>|Build Status (By Branch) | Build Status                |
>|-------------------------|-----------------------------|
>|*origin/main*            |![validation.configurable.json.ci.main]        |
>|*origin/development*     |![validation.configurable.json.ci.development] |


<br/>

---
## Configurable XML Validation Library
> ### Overview
> The XML Validation Library offers a configurable validation provider via xml.
> ### **Build Information**
>|Build Status (By Branch) | Build Status                |
>|-------------------------|-----------------------------|
>|*origin/main*            |![validation.configurable.xml.ci.main]        |
>|*origin/development*     |![validation.configurable.xml.ci.development] |

[validation.nuget.url]:                             https://www.nuget.org/packages/Assimalign.Extensions.Validation
[validation.nuget.version]:                         https://img.shields.io/nuget/v/Assimalign.Extensions.Validation
[validation.nuget.downloads]:                       https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation
[validation.configurable.nuget.url]:                https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable
[validation.configurable.nuget.version]:            https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable
[validation.configurable.nuget.downloads]:          https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable
[validation.configurable.json.nuget.url]:           https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable.Json
[validation.configurable.json.nuget.version]:       https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable.Json
[validation.configurable.json.nuget.downloads]:     https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable.Json
[validation.configurable.xml.nuget.url]:            https://www.nuget.org/packages/Assimalign.Extensions.Validation.Configurable.Xml
[validation.configurable.xml.nuget.version]:        https://img.shields.io/nuget/v/Assimalign.Extensions.Validation.Configurable.Xml
[validation.configurable.xml.nuget.downloads]:      https://img.shields.io/nuget/dt/Assimalign.Extensions.Validation.Configurable.Xml

[validation.ci.main]:                               https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.ci/main
[validation.ci.development]:                        https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.ci/development
[validation.configurable.ci.main]:                  https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.ci/main
[validation.configurable.ci.development]:           https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.ci/development
[validation.configurable.json.ci.main]:             https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.json.ci/main
[validation.configurable.json.ci.development]:      https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.json.ci/development
[validation.configurable.xml.ci.main]:              https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.xml.ci/main
[validation.configurable.xml.ci.development]:       https://img.shields.io/github/workflow/status/assimalign/asal-dotnet-extensions/assimalign.extensions.validation.configurable.xml.ci/development

