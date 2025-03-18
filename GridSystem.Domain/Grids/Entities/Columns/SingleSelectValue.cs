using Generator.Attributes;
using GridSystem.Domain.Abstractions;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids.Columns;

[EfConstructor]
public partial class SingleSelectValue : SoftDeletableEntity
{
    [RelationId(RelationType = typeof(Columns.SingleSelectColumn))]
    private int _singleSelectColumnId;
    
    public SingleSelectValue(string value, int singleSelectColumnId)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{nameof(value)} can not bet null or whitespace.", nameof(value));
        }
        
        Value = value;
        SingleSelectColumnId = singleSelectColumnId;
    }
    
    public string Value { get; set; }

    public SingleSelectColumnRecord ToSingleSelectColumnRecord()
    {
        return SingleSelectValue.CreateSingleSelectColumnRecord(Id, Value);
    }

    public static SingleSelectColumnRecord CreateSingleSelectColumnRecord(int id, string value)
    {
        return new SingleSelectColumnRecord(id, value);
    }
}