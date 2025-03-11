using Generator.Attributes;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.Columns;

[EfConstructor]
public partial class SingleSelectValue : Entity
{
    [RelationId(RelationType = typeof(Columns.SingleSelectColumn))]
    private int _singleSelectColumnId;
    
    public SingleSelectValue(string value, int singleSelectColumnId)
    {
        Value = value;
        SingleSelectColumnId = singleSelectColumnId;
    }
    
    public string Value { get; set; }
}