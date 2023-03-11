using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography;

namespace ComputerClubSimualtor
{
    internal class Program
    {// руся казах 
        static void Main(string[] args)
        {
            ComputerClub computerClub = new ComputerClub(8); // Виталик Красавик гений лучший в мире
            computerClub.Work();// ХЕР В жопу и пенис
        }
        class ComputerClub
        {
            private int _money = 0;
            private List<Computer> _computers = new List<Computer>();
            private Queue<Client> _clients = new Queue<Client>();

            public ComputerClub(int computersCount)
            {
                Random random = new Random();

                for (int i = 0; i < computersCount; i++)
                {
                    _computers.Add(new Computer(random.Next(10, 20)));
                }

                CreateNewClients(25, random);
            }

            public void CreateNewClients(int count, Random random)
            {
                for (int i = 0; i < count; i++)
                {
                    _clients.Enqueue(new Client(random.Next(300, 700), random));
                }
            }
            public void Work()
            {
                while (_clients.Count > 0)
                {
                    Client newClient = _clients.Dequeue();
                    Console.WriteLine($"Баланс компютерного клуба {_money} руб. Ждем нового клиента.");
                    Console.WriteLine($"У вас новый клиент и он хочет купить {newClient.DesiredMinutes} минут");
                    ShowAllcomputersSate();

                    Console.WriteLine("\nВы предлагаете ему компьютер под номером");
                    string userInput = Console.ReadLine();
                    
                    if(int.TryParse(userInput, out int computerNumber))
                    {
                        computerNumber -= 1;

                        if(computerNumber >= 0 && computerNumber < _computers.Count)
                        {
                            if (_computers[computerNumber].IsTaken)
                            {
                                Console.WriteLine("Компьютер занят, клиент съебался");
                            }
                            else
                            {
                                if (newClient.CheckSolvency(_computers[computerNumber]))
                                {
                                    Console.WriteLine("Клиент оплатил и сел за компьютер "+ (computerNumber + 1));
                                    _money += newClient.Pay();
                                    _computers[computerNumber].BecomeTaken(newClient);
                                }
                                else
                                {
                                    Console.WriteLine("У клиента не хватило денег");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Вы сами не знаете за какой компьютер посадить клиента, он ушел");
                        }
                    }
                    else
                    {
                        CreateNewClients(1, new Random());
                        Console.WriteLine("неверный ввод повторите снова");
                    }

                    Console.WriteLine("Чтобы перейти к следующему клиенту, нажмите любую клавишу");
                    Console.ReadKey();
                    Console.Clear();
                    SpendOneMinutes();
                }
            }
            //grgrgrgr

            public void ShowAllcomputersSate()
            {
                Console.WriteLine("\nСписок всех компьютеров:");
                for (int i = 0; i < _computers.Count; i++)
                {
                    Console.Write(i + 1 + " - ");
                    _computers[i].ShowState();
                }
            }

            private void SpendOneMinutes()
            {
                foreach (var computer in _computers)
                {
                    computer.SpentOneMinutes();
                }
            }
        }
        class Computer
        {
            private Client _client;
            private int _minutesRemaining;
            public bool IsTaken
            {
                get
                {
                    return _minutesRemaining > 0;
                }
            }

            public int PricePerMinutes { get; private set; }

            public Computer(int prisePerMinutes)
            {
                PricePerMinutes = prisePerMinutes;
            }

            public void BecomeTaken(Client client)
            {
                _client = client;
                _minutesRemaining = _client.DesiredMinutes;
            }

            public void BecomeEmpty()
            {
                _client = null;
            }

            public void SpentOneMinutes()
            {
                _minutesRemaining--;
            }

            public void ShowState()
            {
                if (IsTaken)
                    Console.WriteLine($"Компьютер занят, осталось минут: {_minutesRemaining}");
                else
                    Console.WriteLine($"Комп свободен, цена за минуту {PricePerMinutes} руб.");
            }



        }
        class Client
        {
            private int _money;
            private int _moneyToPay;

            public int DesiredMinutes { get; private set; }

            public Client(int money, Random random)
            {
                _money = money;
                DesiredMinutes = random.Next(10, 30);
            }

            public bool CheckSolvency(Computer computer)
            {
                _moneyToPay = DesiredMinutes * computer.PricePerMinutes;
                if (_money >= _moneyToPay)
                {
                    return true;
                }
                else
                {
                    _moneyToPay = 0;
                    return false;
                }
            }
            public int Pay()
            {
                _money -= _moneyToPay;
                return _moneyToPay;
            }
        }

    }
}