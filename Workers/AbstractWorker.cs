public abstract class AbstractWorker
{
    public readonly int Id;

    private readonly int password;

    public AbstractWorker(int id, string name, int salary, string login, string password)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        Id = id;
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(salary);
        Salary = salary;
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        Login = login;
        this.password = password.GetHashCode();
    }

    public string Name { get; set; }

    public int Salary { get; set; }

    protected string Login { get; private set; }

    public bool LoginAccount(string input)
    {
        if (input.GetHashCode() == password)
        {
            return true;
        }

        return false;
    }
}
