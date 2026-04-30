public class Rack<Shelf> : AbstractStorageArray<Shelf>
{
    public Rack(Shelf[] items, string name, Address addr) : base(items, name, addr) { }
}