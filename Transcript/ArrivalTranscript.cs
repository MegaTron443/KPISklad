public class ArrivalTranscript : AbstractTranscript
{
    public ArrivalTranscript(DateTime dateTime, string workerName, int workerID, Cargo cargo, Address address)
        : base(dateTime, workerName, workerID)
    {
        Cargo = cargo;
        Address = address;
        Add();
    }

    public Cargo Cargo { get; set; }

    public Address Address { get; set; }

    public override string ToString()
    {
        return $"{CreateBoilerPlate()}Type: Arrival Transcript\n Cargo: {Cargo} - Address: {Address}";
    }

    public bool Add()
    {
        try
        {
            StorageController.AddCargo(Cargo, Address);
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
