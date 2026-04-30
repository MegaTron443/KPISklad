public abstract class AbstractStorageArray <T> : IStorageContainer
{
    public T[] itemArray;
    public string fullName;
    public Address Address { get; set; }
    private bool vacant;

    public AbstractStorageArray(T[] itemArray, string fullName, Address address)
    {
        this.itemArray = itemArray;
        this.fullName = fullName;
        this.Address = address;
        this.vacant = true;
    }

    public Cell? FindEmptyCell()
    {
        if (itemArray == null) return null;

        foreach (var item in itemArray)
        {
            if (item is Cell cell && cell.IsEmpty)
            {
                return cell;
            } 
            else if (item is IStorageContainer container) 
            {
                var emptyCell = container.FindEmptyCell();
                if (emptyCell != null) return emptyCell;
            }
        }
        return null;
    }
    
    public Cargo? CheckCell(Address address)
    {
        if (itemArray == null) return null;

        foreach (var item in itemArray)
        {
            if (item is Cell cell && cell.Address.Equals(address))
                return cell.Item;

            if (item is IStorageContainer container)
            {
                var found = container.CheckCell(address);
                if (found != null) return found;
            }
        }
        return null;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{fullName} (Address: {Address})");

        if (itemArray != null)
        {
            foreach (var item in itemArray)
            {
                string childContent = item?.ToString() ?? "Empty";
                sb.AppendLine("  " + childContent.Replace("\n", "\n  "));
            }
        }
        return sb.ToString();
    }
}