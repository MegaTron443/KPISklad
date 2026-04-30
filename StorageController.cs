public static class StorageController
{
    private static Storage core;
    static StorageController()
    {
        core = new Storage();
    }

    public static Cargo? CheckCell(Address address)
    {
        //TODO: Implement method to check the contents of a cell based on the address and return the cargo if it exists, otherwise return null.
        return null;
    }

    public static Address? FindEmptyCell(Address address)
    {
        //TODO: Implement method to find an empty cell based on the address and return its location, otherwise return null.
        return null;
    }
}