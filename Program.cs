public class Program
{
    private bool work;
    private bool LoggedIn;
    private readonly List<AbstractWorker> workerList;

    pr
 
    public static void Main()
    {
        static void WriteLogin()
        {
            Console.WriteLine("---Menu---");
            Console.WriteLine("1 - Create New worker");
            Console.WriteLine("2 - Log in as worker");
        }

        WriteLogin();

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        bool logged = false;
        while (!logged)
        {
            WriteLogin();
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Button1();
                    logged = true;
                    break;
                case ConsoleKey.D2:
                    Button2();
                    logged = true;
                    break;
                default:
                    break;
            }
        }
    }

    void Button1()
    {
        static void WriteWorkerTypes()
        {
            Console.WriteLine("---Menu---");
            Console.WriteLine("1 - Create InnerManager");
            Console.WriteLine("2 - Create ArrivalManager");
            Console.WriteLine("3 - Create DepartureManager");
        }

        WriteWorkerTypes();

        Console.WriteLine("Input Id");
        int Id = int.Parse(Console.ReadLine());
        Console.WriteLine("Input Name");
        string Name = Console.ReadLine();
        Console.WriteLine("Input Salary");
        int Salary = Console.ReadLine();
        Console.WriteLine("Input Login");
        string Login = Console.ReadLine();
        Console.WriteLine("Input Password");
        string Password = Console.ReadLine();

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        bool Work = true;
        while (Work)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    SButton1();
                    Work = false;
                    break;
                case ConsoleKey.D2:
                    SButton2();
                    Work = false;
                    break;
                case ConsoleKey.D3:
                    SButton3();
                    Work = false;
                    break;
                default:
                    break;
            }

            void SButton1()
            {
                workerList.Add(new InnerManager(Id, Name, Salary, Login, Password));
            }
            void SButton2()
            {
                workerList.Add(new ArrivalManager(Id, Name, Salary, Login, Password));
            }
            void SButton3()
            {
                workerList.Add(new DepartureManager(Id, Name, Salary, Login, Password));
            }
        }
    }

    static void Button2()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        bool Work = true;
        while (Work)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    SButton1();
                    break;
                case ConsoleKey.D2:
                    SButton2();
                    break;
                case ConsoleKey.D3:
                    SButton3();
                    break;
                default:
                    break;
            }
        }
    }

    void Login()
    {
        while (!LoggedIn)
        {
            
        }
    }

    private static T Poll<T>(string message, Func<string, T> converter, Func<T, bool> validator)
    {
        bool finished = false;
        string errorMessage = string.Empty;
        while (!finished)
        {
            if (errorMessage != string.Empty) Console.WriteLine(errorMessage);

        }
    }
}
