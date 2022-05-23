using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using Assimalign.Extensions.Linq.Serialization.Internal;

namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "T")]
public class TypeNode : Node
{
    public TypeNode() { }

    public TypeNode(ILinqExpressionNodeFactory factory, Type type)
        : base(factory)
    {
        Initialize(type);
    }

    private void Initialize(Type type)
    {
        if (type == null)
            return;

        var isAnonymousType = type.IsCustomAttributeDefined(typeof(CompilerGeneratedAttribute), false)
            && type.IsGenericType() && type.Name.Contains("AnonymousType")
            && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
            && (type.GetTypeAttributes() & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;

        if (type.IsGenericType())
        {
            GenericArguments = type.GetGenericArguments().Select(t => new TypeNode(Factory, t)).ToArray();

            var typeDefinition = type.GetGenericTypeDefinition();
            if (isAnonymousType || !Factory.Settings.UseRelaxedTypeNames)
                Name = typeDefinition.AssemblyQualifiedName;
            else
                Name = typeDefinition.FullName;
        }
        else
        {
            if (isAnonymousType || !Factory.Settings.UseRelaxedTypeNames)
                Name = type.AssemblyQualifiedName;
            else
                Name = type.FullName;
        }
    }


    [DataMember(EmitDefaultValue = false, Name = "N")]
    public string Name { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "G")]
    public TypeNode[] GenericArguments { get; set; }

    public Type ToType(ILinqExpressionContext context)
    {
        var type = context.ResolveType(this);
        if (type == null)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return null;
            throw new SerializationException($"Failed to serialize '{Name}' to a type object.");
        }

        if (GenericArguments != null)
            type = type.MakeGenericType(GenericArguments.Select(t => t.ToType(context)).ToArray());

        return type;
    }
}
