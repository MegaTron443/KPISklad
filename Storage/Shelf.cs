public class Shelf<Cell> : AbstractStorageArray<Cell>
{
    public Shelf(Cell[] items, string name, Address addr) : base(items, name, addr) { }
}