using System;

namespace Sieve.Models
{
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        public SortTerm() { }

        private string _sort;
        private string[] _nestedJsonProperties;

        public string Sort
        {
            set
            {
                string[] parts = value.Split("->");
                
                _sort = parts[0];
                _nestedJsonProperties = parts[1..];
            }
        }

        public string Name => (_sort.StartsWith("-")) ? _sort.Substring(1) : _sort;
        public string[] NestedJsonProperties => _nestedJsonProperties;

        public bool Descending => _sort.StartsWith("-");

        public bool Equals(SortTerm other)
        {
            return Name == other.Name
                && Descending == other.Descending;
        }
    }
}
