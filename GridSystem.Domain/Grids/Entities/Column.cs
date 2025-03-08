using Generator.Attributes;
using GridSystem.Domain.Abstractions;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids;

//вынужден не делать его абстактнм, потому что иначе ef core не возможно сконфигурировать(кажется)
public partial class Column : Entity
{
    protected Column() {}
    
    [RelationId(RelationTypeName = nameof(Grid))]
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
}