using System.Text.Json.Serialization;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.ValueObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(NumericColumnRecord), typeDiscriminator: "numeric")]
[JsonDerivedType(typeof(StringColumnRecord), typeDiscriminator: "string")]
[JsonDerivedType(typeof(SingleSelectColumnRecord), typeDiscriminator: "single-select")]
public abstract class ColumnRecord : ValueObject;