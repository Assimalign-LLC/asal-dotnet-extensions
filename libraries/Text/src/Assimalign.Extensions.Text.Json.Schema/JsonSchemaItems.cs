using System;
using System.Collections.Generic;
using System.Linq;

namespace Assimalign.Extensions.Text.Json.Schema;

/// <summary>
/// Represents the value of the JSON schema <code>items</code> keyword.
/// </summary>
/// <remarks>
/// The value of the <code>items</code> keyword can be either a single schema, in
/// which case all items in the array must conform to that schema, or an array of
/// schemas, in which case the array element at index <code>i</code> must conform to
/// the schema at index <code>i</code>.
/// </remarks>
public class JsonSchemaItems : IEquatable<JsonSchemaItems>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSchemaItems"/> class with the
    /// specified schema.
    /// </summary>
    /// <param name="schema">
    /// The schema to which all array items must conform.
    /// </param>
    public JsonSchemaItems(JsonSchema schema)
    {
        Schemas = new List<JsonSchema> { schema };
        SingleSchema = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSchemaItems"/> class with the
    /// specified schema.
    /// </summary>
    /// <param name="schema">
    /// The sequence of schemas to which corresponding array items must conform.
    /// </param>
    public JsonSchemaItems(IEnumerable<JsonSchema> schemas)
    {
        Schemas = schemas.ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSchemaItems"/> class from an existing
    /// instance.
    /// </summary>
    /// <param name="other">
    /// The existing instance.
    /// </param>
    public JsonSchemaItems(JsonSchemaItems other)
    {
        if (other.Schemas != null)
        {
            Schemas = new List<JsonSchema>(other.Schemas);
        }

        SingleSchema = other.SingleSchema;
    }

    /// <summary>
    /// Gets a value indicating whether all array items conform to a single schema.
    /// </summary>
    public bool SingleSchema { get; set; }

    /// <summary>
    /// Gets the sequence of schemas to which corresponding array items must conform.
    /// </summary>
    public IList<JsonSchema> Schemas { get; set; }

    /// <summary>
    /// Gets the schema to which all array items must conform.
    /// </summary>
    [JsonIgnore]
    public JsonSchema Schema
    {
        get
        {
            if (!SingleSchema)
            {
                throw new InvalidOperationException(Resources.ExceptionNotASingleSchema);
            }

            return Schemas.First();
        }
    }

    #region Object overrides

    public override bool Equals(object obj)
    {
        return Equals(obj as JsonSchemaItems);
    }

    public override int GetHashCode()
    {
        return Hash.Combine(new object[] { Schemas, SingleSchema });
    }

    #endregion

    #region IEquatable<T>

    public bool Equals(JsonSchemaItems other)
    {
        if ((object)other == null)
        {
            return false;
        }

        return SingleSchema == other.SingleSchema
            && (Schemas == null
                    ? other.Schemas == null
                    : Schemas.HasSameElementsAs(other.Schemas));
    }

    #endregion

    public static bool operator ==(JsonSchemaItems left, JsonSchemaItems right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if ((object)left == null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(JsonSchemaItems left, JsonSchemaItems right)
    {
        return !(left == right);
    }
}
