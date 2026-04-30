public class Cargo : ICloneable
{
    public Cargo(string name, int size, string source, string destination, string description)
    {
        Name = name;
        Size = size;
        Source = source;
        Destination = destination;
        Description = description;
    }

    public string Name { get; set; }

    public int Size { get; set; }

    public string Source { get; set; }

    public string Destination { get; set; }

    public string Description { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"/---/ \nName: {Name} \nSize: {Size} \nSource: {Source} \nDestination: {Destination} \nDescription: {Description} \n/---/";
    }
}
