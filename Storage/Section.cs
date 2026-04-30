public class Section<Rack> : AbstractStorageArray<Rack>
{
    public Section(Rack[] items, string name, Address addr) : base(items, name, addr) { }
}