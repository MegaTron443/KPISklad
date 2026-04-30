public class RemovalTranscript : AbstractTranscript
{
    private Cargo? cargo;
    private Cell? cell;

    public RemovalTranscript(DateTime dateTime, string workerName, int workerID, Address address)
        : base(dateTime, workerName, workerID)
    {
        Remove(address);
    }

    public override string ToString()
    {
        return $"{CreateBoilerPlate()}Type: Removal Transcript\n{((cargo != null) ? cargo : "Unknown Cargo")}\n - Cell: {((cell != null) ? cell.Address : "Unknown address")}";
    }

    public bool Remove(Address address)
    {
        try
        {
            var cellInfo = StorageController.RemoveCargo(address);
            cargo = cellInfo.Cargo;
            cell = cellInfo.Cell;
            status = true;
            Log.AddTranscript(this);
            return true;
        }
        catch
        {
            status = false;
            Log.AddTranscript(this);
            return false;
        }
    }
}
