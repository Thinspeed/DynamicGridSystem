using Generator.Attributes;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids;

[EfConstructor]
public partial class StringColumn : Column
{
    public StringColumn(string name, int position, int gridId) : base(name, position, gridId)
    {
    }

    public override ColumnType Type => ColumnType.String;
}