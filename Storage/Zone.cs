public class Zone<Section> : AbstractStorageArray<Section>
{
    public Zone(Section[] items, string name, Address addr) : base(items, name, addr) { }
}