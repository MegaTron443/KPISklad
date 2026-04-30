public static class StorageController
{
    public static readonly Storage Core;

    static StorageController()
    {
        Core = new Storage([], "Main Warehouse");
    }

    public static Cell? CheckCell(Address address)
    {
        return Core.CheckCell(address, 0);
    }

    public static (Cargo? CargoFrom, Cargo? CargoTo) MoveCargo(Address from, Address to)
    {
        Cell? fromCell = CheckCell(from);
        Cell? toCell = CheckCell(to);

        ArgumentNullException.ThrowIfNull(fromCell, "Source cell not found.");
        ArgumentNullException.ThrowIfNull(toCell, "Destination cell not found.");

        Cargo? cargoFrom = fromCell.Item;
        Cargo? cargoTo = toCell.Item;

        if (cargoFrom != null && cargoFrom.Size > toCell.Capacity)
            throw new InvalidOperationException("Source cargo exceeds destination cell capacity.");

        if (cargoTo != null && cargoTo.Size > fromCell.Capacity)
            throw new InvalidOperationException("Destination cargo exceeds source cell capacity.");

        fromCell.Item = cargoTo;
        toCell.Item = cargoFrom;

        return (cargoFrom, cargoTo);
    }

    public static void AddCargo(Cargo cargo, Address address)
    {
        Cell? cell = CheckCell(address);
        ArgumentNullException.ThrowIfNull(cell);

        cell.AddCargo(cargo);
    }

    public static (Cargo Cargo, Cell Cell) RemoveCargo(Address address)
    {
        Cell? cell = CheckCell(address);
        ArgumentNullException.ThrowIfNull(cell);

        Cargo? cargo = cell.Item;
        ArgumentNullException.ThrowIfNull(cargo, "Cell is already empty.");

        cell.RemoveCargo();

        return (cargo, cell);
    }

    public static Address? FindEmptyCell()
    {
        return Core.FindEmptyCell()?.Address;
    }

    public static Address? FindEmptyCellInRange(Address startAt)
    {
        if (startAt.Zone.HasValue && startAt.Zone >= 0 && startAt.Zone < Core.ItemArray.Count)
        {
            var zone = Core.ItemArray[startAt.Zone.Value];
            return zone.FindEmptyCell()?.Address;
        }

        return FindEmptyCell();
    }
}
