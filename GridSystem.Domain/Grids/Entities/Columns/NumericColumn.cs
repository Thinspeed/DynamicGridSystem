using Generator.Attributes;
using GridSystem.Domain.Grids.ValueObjects;

namespace GridSystem.Domain.Grids.Columns;

[EfConstructor]
public partial class NumericColumn : Column
{
    private int _decimalPlaces;

    public NumericColumn(string name, int position, int gridId, int decimalPlaces = 0) : base(name, position, gridId)
    {
        DecimalPlaces = decimalPlaces;
    }
    
    public int DecimalPlaces
    {
        get => _decimalPlaces;
        
        set => _decimalPlaces = value < 0 ? throw new ArgumentOutOfRangeException(nameof(DecimalPlaces)) : value;
    }

    public override ColumnType Type => ColumnType.Numeric;

    public override bool TryCreateColumnRecord(string value, out ColumnRecord? record)
    {
        if (!decimal.TryParse(value, out decimal decimalValue))
        {
            record = null!;
            return false;
        }

        record = new NumericColumnRecord(decimalValue);
        return true;
    }

    public void Update(string name, int position, int decimalPlaces = 0)
    {
        Name = name;
        Position = position;
        DecimalPlaces = decimalPlaces;
    }
}