namespace GridSystem.Domain.Grids.ValueObjects;

public class SingleSelectColumnRecord : ColumnRecord
{
    public int Id { get; set; }
    
    public string Value { get; set; }

    public SingleSelectColumnRecord(int id, string value)
    {
        Id = id;
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return [Id, Value];
    }
}