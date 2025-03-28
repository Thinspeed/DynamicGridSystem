using Generator.Attributes;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids.Columns;

[EfConstructor]
public partial class StringColumn : Column
{
    public StringColumn(string name, int position, int gridId) : base(name, position, gridId)
    {
    }

    public override ColumnType Type => ColumnType.String;

    public override bool TryCreateColumnRecord(string value, out ColumnRecord? record)
    {
        record = new StringColumnRecord(value);
        return true;
    }

    public void Update(string name, int position)
    {
        Name = name;
        Position = position;
    }
}