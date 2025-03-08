using Generator.Attributes;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids;

[EfConstructor]
public partial class SingleSelectColumn: Column
{
    public SingleSelectColumn(string name, int position, int gridId, List<string> values) : base(name, position, gridId)
    {
        Values = values;
    }
    
    public List<string> Values { get; set; }
    
    public override ColumnType Type => ColumnType.SingleSelect;
}