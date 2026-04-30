public class ArrivalManager(int id, string name, int salary, string login, string password) : InnerManager(id, name, salary, login, password)
{
    public void AddItem(Address address, Cargo cargo)
    {
        ArrivalTranscript transcript = new ArrivalTranscript(DateTime.Now, Name, Id, cargo, address);
        Console.WriteLine(transcript.ToString());
    }
}
