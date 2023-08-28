using System;
using System.Threading;
using System.Collections.Generic;

class Car
{
    public string Name { get; }
    public double Distance { get; private set; }
    private int speed = 120;
    private Thread thread;

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
        while (Distance < 10000)
        {
            Thread.Sleep(1000);
            Distance += speed / 3600.0; // Convert speed to km/s
            if (RandomEvent())
            {
                HandleEvent();
            }
        }
    }

    private bool RandomEvent()
    {
        return new Random().Next(1, 31) == 1;
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
        var eventIndex = new Random().Next(events.Count);
        var chosenEvent = events[eventIndex];

        if (new Random().NextDouble() <= chosenEvent.Item2)
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
            new Car("Bil2") // Add more cars if needed
        };

        foreach (var car in cars)
        {
            car.Start();
        }

        while (cars.Exists(car => car.Distance < 10000))
        {
            Thread.Sleep(1000);
        }

        foreach (var car in cars)
        {
            car.Drive();
            if (car.Distance >= 10000)
            {
                Console.WriteLine($"{car.Name} kom i mål!");
                if (car.Distance == cars.Max(c => c.Distance))
                {
                    Console.WriteLine($"{car.Name} vann!");
                }
            }
        }
    }
}
