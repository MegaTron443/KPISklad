public class InnerTranscript : AbstractTranscript
{
    public InnerTranscript(DateTime dateTime, string workerName, int workerID, Address from, Address to)
        : base(dateTime, workerName, workerID)
    {
        From = from;
        To = to;
        Move();
    }

    public Cargo? Cargo1 { get; set; }

    public Cargo? Cargo2 { get; set; }

    public Address From { get; set; }

    public Address To { get; set; }

    public override string ToString()
    {
        return $"{CreateBoilerPlate()}Type: Inner Transcript\nFrom: {From} New: {Cargo1} - To: {To} New{Cargo2}";
    }

    public bool Move()
    {
        try
        {
            var cellInfo = StorageController.MoveCargo(From, To);
            Cargo1 = cellInfo.CargoFrom;
            Cargo2 = cellInfo.CargoTo;
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
