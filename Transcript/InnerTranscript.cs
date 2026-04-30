public class InnerTranscript : AbstractTranscript
{
    public Cargo Cargo { get; set; }
    public Address From { get; set; }
    public Address To { get; set; }

    public InnerTranscript(DateTime dateTime, Cargo cargo, Address from, Address to)
        : base(dateTime)
    {
        Cargo = cargo;
        From = from;
        To = to;
    }

    public override string ToString()
    {
        return CreateBoilerPlate() + $"Type: Inner Transcript\n Cargo: {Cargo.ToString()} - From: {From.ToString()} - To: {To.ToString()}";
    }

    public bool Move()
    {
        try
        {
            controller.MoveCargo(From, To);
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