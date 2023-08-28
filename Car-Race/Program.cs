using System;
using System.Threading;
using System.Collections.Generic;

class Car
{
    public string Name { get; }
    public double Distance { get; private set; }
    private int speed = 120;
    private Thread thread;
    private Random random = new Random();
    public void WaitForFinish()
    {
        thread.Join();
    }

    public Car(string name)
    {
        Name = name;
        Distance = 0;
        thread = new Thread(Drive);
    }

    public void Start()
    {
        thread.Start();
        Console.WriteLine($"{Name} startar!");
    }

    public void Drive()
    {
        Timer timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        while (Distance < 10)
        {
            Thread.Sleep(1000);
            Distance += speed / 3600.0;
        }

        timer.Dispose();
    }

    private void TimerCallback(object state)
    {
        TriggerEvent();
    }

    public void TriggerEvent()
    {
        if (RandomEvent())
        {
            HandleEvent();
        }
    }

    private bool RandomEvent()
    {
        return random.Next(1, 31) == 1;
    }

    private void HandleEvent()
    {
        List<Tuple<string, double, int>> events = new List<Tuple<string, double, int>>
        {
            Tuple.Create("Slut på bensin", 1.0 / 50, 30),
            Tuple.Create("Punktering", 2.0 / 50, 20),
            Tuple.Create("Fågel på vindrutan", 5.0 / 50, 10),
            Tuple.Create("Motorfel", 10.0 / 50, 1)
        };
        var eventIndex = random.Next(events.Count);
        var chosenEvent = events[eventIndex];

        if (random.NextDouble() <= chosenEvent.Item2)
        {
            Console.WriteLine($"{Name} drabbades av {chosenEvent.Item1} och stannar i {chosenEvent.Item3} sekunder.");
            Thread.Sleep(chosenEvent.Item3 * 1000);
            if (chosenEvent.Item1 == "Motorfel")
            {
                speed -= 1;
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Car> cars = new List<Car>
        {
            new Car("Bil1"),
            new Car("Bil2")
        };

        foreach (var car in cars)
        {
            car.Start();
        }

        //timer for every 30s to trigger an event
        Timer eventTimer = null;
        int currentCarIndex = 0;

        eventTimer = new Timer((state) =>
        {
            cars[currentCarIndex].TriggerEvent();
            currentCarIndex = (currentCarIndex + 1) % cars.Count;
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        //User input key S
        bool racing = true;
        while (racing)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.S)
                {
                    Console.WriteLine("Car distances:");
                    foreach (var car in cars)
                    {
                        Console.WriteLine($"{car.Name}: {car.Distance:F2} km");
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    racing = false;
                }
            }
        }

        foreach (var car in cars)
        {
            car.WaitForFinish();
        }

        eventTimer.Dispose();
        Console.WriteLine("All cars finished the race!");
    }
}
