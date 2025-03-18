using System.Text.Json;
using System.Text.Json.Nodes;
using Generator.Attributes;
using GridSystem.Domain.Abstractions;
using GridSystem.Domain.Extensions;
using GridSystem.Domain.Grids.Columns;
using GridSystem.Domain.Grids.Rows;

namespace GridSystem.Domain.Grids;

[EfConstructor]
public partial class Grid : SoftDeletableEntity, IAggregateRoot
{
    private const string IdPropertyName = "Id";
    public Grid(string name)
    {
        Name = name;
    }
    
    private string GetDefaultRowData => $"{{ \"{IdPropertyName}\":\"{Guid.NewGuid()}\" }}";
    
    public string Name { get; set; }
    
    public ICollection<Column> Columns { get; } = new List<Column>();
    
    public ICollection<Row> Rows { get; } = new List<Row>();

    public void Update(string name)
    {
        Name = name;
    }
    
    public NumericColumn AddNumericColumn(string name, int position, int decimalPlaces)
    {
        var column = new NumericColumn(name, position, Id, decimalPlaces);
        
        Columns.Add(column);

        return column;
    }

    public void UpdateNumericColumn(int columnId, string name, int position, int decimalPlaces)
    {
        NumericColumn column = GetColumn<NumericColumn>(columnId);
        
        column.Update(name, position, decimalPlaces);
    }

    public SingleSelectColumn AddSingleSelectColumn(string name, int position)
    {
        var column = new SingleSelectColumn(name, position, Id);
        
        Columns.Add(column);

        return column;
    }

    public void UpdateSingleSelectColumn(int columnId, string name, int position)
    {
        SingleSelectColumn column = GetColumn<SingleSelectColumn>(columnId);

        column.Update(name, position);
    }

    public SingleSelectValue AddSingleSelectValue(int columnId, string value)
    {
        SingleSelectColumn column = GetColumn<SingleSelectColumn>(columnId);

        return column.AddValue(value);
    }

    public void UpdateSingleSelectValue(int columnId, int singleSelectValueId, string newValue)
    {
        SingleSelectColumn column = GetColumn<SingleSelectColumn>(columnId);

        column.UpdateValue(singleSelectValueId, newValue);
        
        foreach (var row in Rows)
        {
            string? selected = row.GetValue(columnId.ToString());
            if (selected is null) continue;
            
            int selectedId = JsonDocument.Parse(selected).RootElement.GetProperty(nameof(SingleSelectValue.Id)).GetInt32();
            if (selectedId != singleSelectValueId) continue;
            
            row.AddOrUpdate(columnId.ToString(), SingleSelectValue.CreateRowValue(singleSelectValueId, newValue));
        }
    }

    public void RemoveSingleSelectValue(int columnId, int singleSelectValueId)
    {
        SingleSelectColumn column = GetColumn<SingleSelectColumn>(columnId);
        
        column.RemoveValue(singleSelectValueId);

        foreach (var row in Rows)
        {
            string? selected = row.GetValue(columnId.ToString());
            if (selected is null) continue;
            
            int selectedId = JsonDocument.Parse(selected).RootElement.GetProperty(nameof(SingleSelectValue.Id)).GetInt32();
            if (selectedId != singleSelectValueId) continue;
            
            row.Remove(columnId.ToString());
        }
    }

    public StringColumn AddStringColumn(string name, int position)
    {
        var column = new StringColumn(name, position, Id);
        
        Columns.Add(column);
        
        return column;
    }

    public void UpdateStringColumn(int columnId, string name, int position)
    {
        StringColumn column = GetColumn<StringColumn>(columnId);
        
        column.Update(name, position);
    }

    public Row AddRow()
    {
        //string data = GetDefaultRowData;
        Row row = new Row(Id);
        
        Rows.Add(row);

        return row;
    }

    public void UpdateRow(int rowId, int columnId, string value)
    {
        Column column = Columns.FirstOrThrow(
            c => c.Id == columnId, 
            () => new ArgumentException($"Column with id {columnId} was not found"));

        if (!column.ValidateValue(value))
        {
            throw new Exception($"Value is not valid");
        }
        
        Row row = Rows.FirstOrThrow(
            x => x.Id == rowId,
            () => new ArgumentException($"Row with id {columnId} was not found"));
        
        row.AddOrUpdate(columnId.ToString(), value);
        
    }
    
    private T GetColumn<T>(int columnId)
        where T : Column
    {
        Column column = Columns.FirstOrThrow(
            c => c.Id == columnId, 
            () => new ArgumentException($"Column with id {columnId} was not found"));
        
        T typedColumn = column as T ?? throw new Exception($"Column type is not {typeof(T).Name}");
        
        return typedColumn;
    }
}