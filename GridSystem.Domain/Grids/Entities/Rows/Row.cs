using System.Text.Json;
using Generator.Attributes;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.Rows;

[EfConstructor]
public partial class Row : SoftDeletableEntity
{
    [RelationId(RelationType = typeof(Grids.Grid))]
    private int _gridId;
    
    public Row(int columnId, JsonDocument data)
    {
        GridId = columnId;
        Data = data;
    }
    
    public JsonDocument Data { get; set; }
}