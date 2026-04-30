public abstract class AbstractTranscript
{
    protected string workerName;
    protected int workerID;
    protected bool status;

    protected AbstractTranscript(DateTime dateTime, string workerName, int workerID)
    {
        this.DateTime = dateTime;
        this.workerName = workerName;
        this.workerID = workerID;
    }

    public DateTime DateTime { get; protected set; }

    public abstract override string ToString();

    protected string CreateBoilerPlate()
    {
        return $"Time: {DateTime} \nWorker: {workerName}, ID: {workerID} \nStatus: {(status ? "Success" : "Failure")}\n";
    }
}
