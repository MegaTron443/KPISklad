public interface IStorageContainer
{
    Cell? FindEmptyCell();
    Cargo? CheckCell(Address address);
}