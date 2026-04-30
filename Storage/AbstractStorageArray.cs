using System.Collections;
using System.Text;

public abstract class AbstractStorageArray<T> : IStorageContainer, IEnumerable<T>
    where T : IStorageContainer
{
    public IList<T> ItemArray;
    public string FullName;

    public AbstractStorageArray(IList<T> itemArray, string fullName)
    {
        this.ItemArray = itemArray;
        this.FullName = fullName;
    }

    public T this[int i]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, ItemArray.Count);
            return ItemArray[i];
        }

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, ItemArray.Count);
            ItemArray[i] = value;
        }
    }

    public void AddNewPlace(T container)
    {
        ItemArray.Add(container);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T item in ItemArray)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Cell? FindEmptyCell()
    {
        foreach (var item in ItemArray)
        {
            var cell = item.FindEmptyCell();
            if (cell != null) return cell;
        }

        return null;
    }

    public Cell? CheckCell(Address address, int depth)
    {
        int? targetIndex = address[depth];

        if (targetIndex == null || targetIndex < 0 || targetIndex >= ItemArray.Count)
            return null;

        var nextLevel = ItemArray[targetIndex.Value];

        return nextLevel.CheckCell(address, depth + 1);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(FullName);

        if (ItemArray != null)
        {
            foreach (var item in ItemArray)
            {
                string childContent = item?.ToString() ?? "Empty";
                sb.AppendLine("  " + childContent.Replace("\n", "\n  "));
            }
        }

        return sb.ToString();
    }
}
