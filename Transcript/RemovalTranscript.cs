public class RemovalTranscript : AbstractTranscript
{
    private Cargo cargo;
    private Cell cell;
    public RemovalTranscript(DateTime dateTime, string workerName, int workerID, bool status)
    {
        this.dateTime = dateTime;
        this.workerName = workerName;
        this.workerID = workerID;
        this.status = status;
    }

    public override string ToString()
    {
        return CreateBoilerPlate() + $"Type: Removal Transcript\n Cargo: {cargo.ToString()} - Cell: {cell.Address.ToString()}";
    }

    public bool Remove(Address address)
    {
        try
        {
            cell = controller.GetCell(address);
            cargo = cell.Item;
            controller.RemoveCargo(address);
            status = true;
            Log.AddTranscript(this);
            return true;
        }
        catch (Exception)
        {
            status = false;
            Log.AddTranscript(this);
            return false;
        }
    }
}