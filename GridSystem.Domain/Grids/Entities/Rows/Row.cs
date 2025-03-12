using Generator.Attributes;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.Rows;

[EfConstructor]
public partial class Row : SoftDeletableEntity
{
    [RelationId(RelationType = typeof(Grids.Grid))]
    private int _gridId;
    
    public Row(int columnId, Dictionary<string, string> data)
    {
        GridId = columnId;
        Data = data;
    }
    
    public Dictionary<string, string> Data { get; set; }
}