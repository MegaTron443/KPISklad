public abstract class AbstractTranscript
{
    protected DateTime dateTime;
    protected string workerName;
    protected int workerID;
    protected bool status;
    protected static StorageController controller;

    public DateTime DateTime => dateTime;

    protected AbstractTranscript(DateTime dateTime, string workerName, int workerID)
    {
        this.dateTime = dateTime;
        this.workerName = workerName;
        this.workerID = workerID;
    }

    private string CreateBoilerPlate()
    {
        return $"Time: {dateTime} \n Worker: {workerName}, ID: {workerID} \n Status: {(status ? "Success" : "Failure")}\n";
    }

    public abstract override string ToString();
}