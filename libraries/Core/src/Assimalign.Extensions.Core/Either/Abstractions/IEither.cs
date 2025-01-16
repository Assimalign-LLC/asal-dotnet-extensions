using System;

namespace Assimalign.Extensions;

public interface IEither
{
    int TypeIndex { get; }
    Type Type { get; }
    object Value { get; }
}