public class Cell
{
    public int Capacity { get; set; }
    public Cargo? Item { get; set; }
    public Address Address { get; set; }

    public Cell(int capacity, Address address)
    {
        Capacity = capacity;
        Address = address;
        Item = null;
    }

    public bool IsEmpty => Item == null;

    public void AddCargo(Cargo cargo)
    {
        if (Item != null)
        {
            throw new InvalidOperationException("Cell is already occupied.");
        }

        if (cargo.Size > Capacity)
        {
            throw new InvalidOperationException("Cargo exceeds cell capacity.");
        }

        Item = cargo;
    }
}