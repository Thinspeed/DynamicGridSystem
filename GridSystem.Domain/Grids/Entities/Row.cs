using System.Text.Json;
using System.Text.Json.Serialization;
using Generator.Attributes;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids;

[EfConstructor]
public partial class Row : Entity
{
    [RelationId(RelationTypeName = nameof(Grid))]
    private int _gridId;
    
    public Row(int columnId, Dictionary<string, string> data)
    {
        GridId = columnId;
        Data = data;
    }
    
    public Dictionary<string, string> Data { get; set; }
}