namespace GridSystem.Domain.Grids.ValueObjects;

public class StringColumnRecord : ColumnRecord
{
    public string Value { get; set; }

    public StringColumnRecord(string value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return [Value];
    }
}