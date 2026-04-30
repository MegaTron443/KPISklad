public class Rack : AbstractStorageArray<Shelf>
{
    public Rack(IList<Shelf> items, string name)
        : base(items, name)
    {
    }
}
