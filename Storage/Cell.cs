public class Cell : IStorageContainer
{
    public Cell(int capacity, Address address)
    {
        Capacity = capacity;
        Address = address;
        Item = null;
    }

    public int Capacity { get; set; }

    public Cargo? Item { get; set; }

    public Address Address { get; set; }

    public bool IsEmpty => Item == null;

    public void AddCargo(Cargo cargo)
    {
        if (Item != null)
            throw new InvalidOperationException("Cell is already occupied.");

        if (cargo.Size > Capacity)
            throw new InvalidOperationException("Cargo exceeds cell capacity.");

        Item = cargo;
    }

    public void RemoveCargo()
    {
        if (Item == null)
            throw new InvalidOperationException("Cell is already empty.");

        Item = null;
    }

    public Cell? FindEmptyCell() => IsEmpty ? this : null;

    public Cell? CheckCell(Address address, int depth) => Address.Equals(address) ? this : null;
}
