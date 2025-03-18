using Generator.Attributes;
using GridSystem.Domain.Abstractions;

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

    public string ToRowValue()
    {
        return SingleSelectValue.CreateRowValue(Id, Value);
    }

    public static string CreateRowValue(int id, string value)
    {
        return $"{{\"{nameof(SingleSelectValue.Id)}\":{id},\"{nameof(SingleSelectValue.Value)}\":\"{value}\"}}";
    }
}