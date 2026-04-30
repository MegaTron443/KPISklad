public class Address
{
    private readonly int?[] levels = new int?[5];

    public int? Zone { get => levels[0]; set => levels[0] = value; }

    public int? Section { get => levels[1]; set => levels[1] = value; }

    public int? Rack { get => levels[2]; set => levels[2] = value; }

    public int? Shelf { get => levels[3]; set => levels[3] = value; }

    public int? Cell { get => levels[4]; set => levels[4] = value; }

    public int LevelCount => levels.Length;

    public int? this[int index]
    {
        get => levels[index];
        set => levels[index] = value;
    }

    public override string ToString() => string.Join("-", levels.Select(l => l?.ToString() ?? "X"));

    public override bool Equals(object? obj)
    {
        if (obj is Address other)
        {
            return levels.SequenceEqual(other.levels);
        }

        return false;
    }

    public override int GetHashCode()
    {
        var hash = default(HashCode);
        foreach (var level in levels) hash.Add(level);
        return hash.ToHashCode();
    }
}
