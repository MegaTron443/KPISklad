public class Shelf : AbstractStorageArray<Cell>
{
    public Shelf(IList<Cell> items, string name)
        : base(items, name)
    {
    }
}
