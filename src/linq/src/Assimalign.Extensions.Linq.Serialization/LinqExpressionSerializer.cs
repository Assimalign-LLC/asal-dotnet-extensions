using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Nodes;
using Assimalign.Extensions.Linq.Serialization.Internal;


public abstract class LinqExpressionSerializer : ILinqExpressionSerializer
{
    private readonly HashSet<Type> customKnownTypes;
    private bool _autoAddKnownTypesAsArrayTypes;
    private bool _autoAddKnownTypesAsListTypes;
    private IEnumerable<Type> _knownTypesExploded;

    public LinqExpressionSerializer()
    {
        customKnownTypes = new HashSet<Type>();
        AutoAddKnownTypesAsArrayTypes = true;
    }

    public virtual void Serialize<T>(Stream stream, T obj) where T : Node
    {
        if (stream == null)
            throw new ArgumentNullException("stream");

        var serializer = CreateXmlObjectSerializer(typeof(T));
        serializer.WriteObject(stream, obj);
    }

    public virtual T Deserialize<T>(Stream stream) where T : Node
    {
        if (stream == null)
            throw new ArgumentNullException("stream");

        var serializer = CreateXmlObjectSerializer(typeof(T));
        return (T)serializer.ReadObject(stream);
    }

    protected abstract XmlObjectSerializer CreateXmlObjectSerializer(Type type);


    public bool AutoAddKnownTypesAsArrayTypes
    {
        get => _autoAddKnownTypesAsArrayTypes;
        set
        {
            _autoAddKnownTypesAsArrayTypes = value;
            if (value)
                _autoAddKnownTypesAsListTypes = false;
            _knownTypesExploded = null;
        }
    }

    public bool AutoAddKnownTypesAsListTypes
    {
        get => _autoAddKnownTypesAsListTypes;
        set
        {
            _autoAddKnownTypesAsListTypes = value;
            if (value)
                _autoAddKnownTypesAsArrayTypes = false;
            _knownTypesExploded = null;
        }
    }

    public void AddKnownType(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        customKnownTypes.Add(type);
        _knownTypesExploded = null;
    }

    public void AddKnownTypes(IEnumerable<Type> types)
    {
        if (types == null)
            throw new ArgumentNullException(nameof(types));

        foreach (var type in types)
            AddKnownType(type);
    }

    protected virtual IEnumerable<Type> GetKnownTypes()
    {
        if (_knownTypesExploded != null)
            return _knownTypesExploded;

        _knownTypesExploded = ExplodeKnownTypes(KnownTypes.All)
            .Concat(ExplodeKnownTypes(customKnownTypes)).ToList();
        return _knownTypesExploded;
    }

    private IEnumerable<Type> ExplodeKnownTypes(IEnumerable<Type> types)
    {
        return KnownTypes.Explode(
            types, AutoAddKnownTypesAsArrayTypes, AutoAddKnownTypesAsListTypes);
    }

}
