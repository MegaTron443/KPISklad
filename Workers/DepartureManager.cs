public class DepartureManager(int id, string name, int salary, string login, string password) : InnerManager(id, name, salary, login, password)
{
    public void RemoveItem(Address address)
    {
        RemovalTranscript transcript = new RemovalTranscript(DateTime.Now, Name, Id, address);
        Console.WriteLine(transcript.ToString());
    }
}
