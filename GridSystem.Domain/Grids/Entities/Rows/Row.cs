using System.Text.Json;
using System.Text.Json.Nodes;
using Generator.Attributes;
using GridSystem.Domain.Abstractions;

namespace GridSystem.Domain.Grids.Rows;

[EfConstructor]
public partial class Row : SoftDeletableEntity
{
    [RelationId(RelationType = typeof(Grids.Grid))]
    private int _gridId;

    private readonly Dictionary<string, string> _data;
    
    public Row(int gridId, JsonDocument data)
    {
        GridId = gridId;
        _data = data.Deserialize<Dictionary<string, string>>()
                ?? throw new Exception("Row data deserialization failed");
    }

    public Row(int gridId, string data)
    {
        GridId = gridId;
        _data = JsonSerializer.Deserialize<Dictionary<string, string>>(data) 
                ?? throw new Exception("Row data deserialization failed");
    }

    public Row(int gridId)
    {
        GridId = gridId;
        _data = new Dictionary<string, string>();
    }
    
    public string? GetValue(string key)
    {
        return _data.GetValueOrDefault(key);
    }
    
    public void AddOrUpdate(string key, string value)
    {
        _data[key] = value;
    }

    public void Remove(string key)
    {
        _data.Remove(key);
    }
    
    public JsonDocument Data
    {
        get => JsonSerializer.SerializeToDocument(_data);
        init => _data = value.Deserialize<Dictionary<string, string>>()
                        ?? throw new Exception("Row data deserialization failed");
    }
    
    public string DataAsString => Data.RootElement.ToString();
}