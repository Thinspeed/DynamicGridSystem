using Generator.Attributes;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids.Columns;

[EfConstructor]
public partial class SingleSelectColumn: Column
{
    public SingleSelectColumn(string name, int position, int gridId) : base(name, position, gridId)
    {
    }

    public ICollection<SingleSelectValue> Values { get; } = new List<SingleSelectValue>();
    
    public override ColumnType Type => ColumnType.SingleSelect;

    public void Update(string name, int position)
    {
        Name = name;
        Position = position;
    }
}