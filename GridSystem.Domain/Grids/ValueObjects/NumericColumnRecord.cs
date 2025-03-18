namespace GridSystem.Domain.Grids.ValueObjects;

public class NumericColumnRecord : ColumnRecord
{
    public decimal Value { get; set; }

    public NumericColumnRecord(decimal value)
    {
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return [Value];
    }
}