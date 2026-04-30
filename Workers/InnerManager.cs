public class InnerManager(int id, string name, int salary, string login, string password) : AbstractWorker(id, name, salary, login, password)
{
    public void MoveItem(Address from, Address to)
    {
        InnerTranscript transcript = new InnerTranscript(DateTime.Now, Name, Id, from, to);
        Console.WriteLine(transcript.ToString());
    }
}
