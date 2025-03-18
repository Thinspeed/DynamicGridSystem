using Generator.Attributes;
using GridSystem.Domain.Abstractions;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids.Columns;

//вынужден не делать его абстактнм, потому что иначе ef core не возможно сконфигурировать(кажется)
public partial class Column : SoftDeletableEntity
{
    protected Column() {}
    
    [RelationId(RelationType = typeof(Grid))]
    private int _gridId;

    public Column(string name, int position, int gridId)
    {
        Name = name;
        Position = position;
        GridId = gridId;
    }
    
    public string Name { get; set; }
    
    public int Position { get; set; }

    public virtual ColumnType Type { get; } = null!;

    public virtual bool TryCreateColumnRecord(string value, out ColumnRecord? record)
    {
        record = null;
        return false;
    }
}