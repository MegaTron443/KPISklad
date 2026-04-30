public class ArrivalTranscript : AbstractTranscript
{
    public Cargo Cargo { get; set; }
    public Address Address { get; set; }

    public ArrivalTranscript(DateTime dateTime, string workerName, int workerID, Cargo cargo, Address address) 
        : base(dateTime, workerName, workerID)
    {
        Cargo = cargo;
        Address = address;
    }

    public override string ToString()
    {
        return $"{CreateBoilerPlate()} Type: Arrival Transcript\n Cargo: {Cargo.ToString()} - Address: {Address.ToString()}";
    }

    public bool Add()
    {
        try
        {
            controller.AddCargo(Address, Cargo);
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