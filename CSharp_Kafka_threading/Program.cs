using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class ServiceA
{
    public void DoWork(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            
        }
    }
}

public class ServiceB
{
    public void DoWork(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // Simulate work
            Console.WriteLine("ClassB is working...");
            //Thread.Sleep(1000); // Sleep for 1 second
        }
    }
}

public class ServiceC
{
    public void DoWork(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Console.WriteLine("ClassC is working...");
        }
    }
}

public class Program
{
    //создание ограничении на потоке - экспериментально 200 потоков для куба
    private const int MaxDegreeOfParallelism = 200;

    public static void Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        SemaphoreSlim semaphore = new SemaphoreSlim(MaxDegreeOfParallelism);

        List<Task> tasks = new List<Task>();

        // Инициализация сервисов который читает каждый свой топик кафка
        var ServiceA = new ServiceA();
        var ServiceB = new ServiceB();
        var ServiceC = new ServiceC();

        // Создание задачи с семафором
        tasks.Add(StartTask(ServiceA.DoWork, cts.Token, semaphore));
        tasks.Add(StartTask(ServiceB.DoWork, cts.Token, semaphore));
        tasks.Add(StartTask(ServiceC.DoWork, cts.Token, semaphore));

        //Ожидаем после всего выполнения ALL или хотя бы из любого ANY 
        //Task.WhenAll(tasks).Wait();
        Task.WhenAny(tasks).Wait();
    }

    private static Task StartTask(Action<CancellationToken> action, CancellationToken token, SemaphoreSlim semaphore)
    {
        return Task.Run(async () =>
        {
            await semaphore.WaitAsync();
            try
            {
                action(token);
            }
            finally
            {
                semaphore.Release();
            }
        });
    }
}