using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace Assimalign.Extensions.Linq.Serialization;

public class LinqExpressionJsonSerializer : LinqExpressionTextSerializer, ILinqExpressionJsonSerializer
{
    protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
    {
        return new DataContractJsonSerializer(type, GetKnownTypes());
    }
}
