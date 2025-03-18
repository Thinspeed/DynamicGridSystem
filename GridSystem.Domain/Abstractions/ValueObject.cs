namespace GridSystem.Domain.Abstractions;

public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (this.GetType() != obj?.GetType())
        {
            return false;
        }
        
        object[] l = this.GetEqualityComponents().ToArray();
        object[] r = this.GetEqualityComponents().ToArray();

        if (l.Length != r.Length)
        {
            return false;
        }

        for (int i = 0; i < l.Length; i++)
        {
            if (!l[i].Equals(r[i]))
            {
                return false;
            }
        }

        return true;
    }
    
    protected abstract IEnumerable<object> GetEqualityComponents();
}