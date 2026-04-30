public class Address
{
    public int? Zone { get; set; }
    public int? Section { get; set; }
    public int? Rack { get; set; }
    public int? Shelf { get; set; }
    public int? Cell { get; set; }

    public override string ToString()
    {
        return $"{Zone}-{Section}-{Rack}-{Shelf}-{Cell}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Address other)
        {
            return Zone == other.Zone && Section == other.Section && 
                Rack == other.Rack && Shelf == other.Shelf && Cell == other.Cell;
        }
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Zone, Section, Rack, Shelf, Cell);
}