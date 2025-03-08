using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.ValueObjects;

public enum ColumnTypeEnum
{
    String = 1,
    Numeric = 2,
    Email = 3,
    RegexpValidated = 4,
    ExternalCollection = 5,
    SingleSelect = 6,
    MultiSelect = 7,
}

public class ColumnType : ValueObject
{
    public string Name { get; }
    
    public ColumnTypeEnum Type { get; }

    public ColumnType(string name, ColumnTypeEnum type)
    {
        Name = name;
        Type = type;
    }
    
    public static ColumnType String => new ColumnType("string", ColumnTypeEnum.String);
    public static ColumnType Numeric => new ColumnType("numeric", ColumnTypeEnum.Numeric);
    public static ColumnType Email => new ColumnType("email", ColumnTypeEnum.Email);
    public static ColumnType RegexpValidated => new ColumnType("regexp validated", ColumnTypeEnum.RegexpValidated);
    public static ColumnType ExternalCollection => new ColumnType("external collection", ColumnTypeEnum.ExternalCollection);
    public static ColumnType SingleSelect => new ColumnType("single-select", ColumnTypeEnum.SingleSelect);
    public static ColumnType MultiSelect => new ColumnType("multi-select", ColumnTypeEnum.MultiSelect);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return [Name, Type];
    }
}