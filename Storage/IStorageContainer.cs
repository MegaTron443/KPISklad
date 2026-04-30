public interface IStorageContainer
{
    Cell? FindEmptyCell();

    Cell? CheckCell(Address address, int depth);
}
