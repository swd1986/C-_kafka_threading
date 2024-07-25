using dotenv.net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dotenv.net;

namespace Consumer
{
    public static class Program
    {
        private const int MaxDegreeOfParallelism = 200;

        public static async Task Main(string[] args)
        {
            await RunConsumers();
        }

        public static async Task RunConsumers()
        {
            DotEnv.Load();

            CancellationTokenSource cts = new CancellationTokenSource();
            SemaphoreSlim semaphore = new SemaphoreSlim(MaxDegreeOfParallelism);

            var services = InitializeServices();
            var tasks = CreateTasks(services, cts.Token, semaphore);

            await Task.WhenAny(tasks);
            //await Task.WhenAll(tasks);
        }

        private static IEnumerable<Action<CancellationToken>> InitializeServices()
        {
            var cl_ServiceA = new ServiceA();
            var cl_ServiceB = new ServiceB();
            var cl_ServiceC = new ServiceC();

            return new List<Action<CancellationToken>>
            {
                cl_ServiceA.DoWork,
                cl_ServiceB.DoWork,
                cl_ServiceC.DoWork
            };
        }

        private static List<Task> CreateTasks(IEnumerable<Action<CancellationToken>> services, CancellationToken token, SemaphoreSlim semaphore)
        {
            var tasks = new List<Task>();
            foreach (var service in services)
            {
                tasks.Add(StartTask(service, token, semaphore));
            }
            return tasks;
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
}
