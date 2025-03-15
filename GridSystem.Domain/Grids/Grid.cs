using Generator.Attributes;
using GridSystem.Domain.Abstractions;
using GridSystem.Domain.Extensions;
using GridSystem.Domain.Grids.Columns;
using GridSystem.Domain.Grids.Rows;

namespace GridSystem.Domain.Grids;

[EfConstructor]
public partial class Grid : SoftDeletableEntity, IAggregateRoot
{
    public string Name { get; set; }

    public Grid(string name)
    {
        Name = name;
    }

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