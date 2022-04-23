# Assimalign .NET Extensions

**NOTE**: The .NET extensions within this library break-away from the Microsoft Platform and should be used with caution.

The .NET extensions within this repository are made-up of custom implementations and/or customized version of other Third-Party libraries. It should be important to note that while some of these libraries are a derivative of Microsoft Extensions and other popular third-party libraries they may not have the same implementation and could cause breaking changes if trying to swap out. 

The intent behind this repository is to create a standard ecosystem of commonly used libraries don't necessarly :

1. **Standardize and expose API's which are otherwise not exposed in other libraries,**
2. **Adhere to common designs patterns and principals for a more cohesive implementations across all libraries,**
3. 



that live within [dotnet/runtime](https://github.com/dotnet/runtime) and are being used, modified, and added to for experimental projects under Assimalign.

- [Assimalign .NET Extensions](#assimalign-net-extensions)
- [Coding Guidelines](#coding-guidelines)
  - [Pull-Request](#pull-request)
  - [Documentation](#documentation)
  - [Folder Structure](#folder-structure)
- [Available Extensions](#available-extensions)
  - [Object Validation](#object-validation)
  - [Object-to-Object Mapping](#object-to-object-mapping)
  - [Configuration](#configuration)
  - [LINQ Serialization](#linq-serialization)
  - [JSON Object](#json-object)
  - [HTTP Factory](#http-factory)
  - [CRONS Scheduling](#crons-scheduling)
  - [Dependency Injection](#dependency-injection)


# Coding Guidelines

1. Framework Target: Unified .NET and Above

## Pull-Request 
- All libraries should inlcude a `README`
- All libraries should use standard design patterns.
- 
&#10003; **Allowed**

&#10007; **Disallowed**

## Documentation
## Folder Structure
All projects within the library should mimic the same folder structure
```
Project.csproj

    /Abstractions 
    /Extensions -> 
    /Internal

```





# Available Extensions 

## Object Validation
[Go to Extensions](/src/validation/README.md)

A flexible set of extensions for validating objects either via fluent interfaces and/or configuration interfaces. Write Validation rules in either JSON or XML without having to re-compile your application.

---

## Object-to-Object Mapping
[Go to Extensions](/src/mapping/README.md)

Object-to-Object mapping is the 

---

## Configuration 
[Go to Extensions](/src/configuration/README.md)

The configuration extensions offer a set of standard

---

## LINQ Serialization
[Go to Extensions](/src/linq/README.md)

---

## JSON Object
[Go to Extensions](/src/text/README.md)

Newtonsoft.Json is a very popular library that offers the ability to query JSON via LINQ. Within the 

---

## HTTP Factory
[Go to Extensions](/src/net/README.md)

---

## CRONS Scheduling
[Go to Extensions](/src/scheduling/README.md)

---

## Dependency Injection
[Go to Extensions](/src/di/README.md)

The dependency injection extensions is a mirror to Microsoft Dependency Injection. However it is important to note that some internal enhancements were made and may not operate the same as expected.