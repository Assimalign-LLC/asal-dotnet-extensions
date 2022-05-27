using System;
using System.IO;
using System.Runtime.Serialization;

namespace Assimalign.Extensions.Linq.Serialization;

public class LinqExpressionXmlSerializer : LinqExpressionTextSerializer, ILinqExpressionXmlSerializer
{
    protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
    {
        return new DataContractSerializer(type, GetKnownTypes());
    }
}
