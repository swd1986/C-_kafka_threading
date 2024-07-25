using Confluent.Kafka;
using dotenv.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    public class ServiceC
    {
        public async void DoWork(CancellationToken token)
        {
            var cfg = DotEnv.Read();
            var config = new ProducerConfig
            {
                BootstrapServers = cfg["BOOTSTRAPSERVERS"],
                Acks = Acks.All,
                MessageTimeoutMs = 30000,
                //SASL
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslPlaintext,
                SaslUsername = cfg["SASLUSERNAME"],
                SaslPassword = cfg["SASLPASSWORD"],
            };

            IProducer<string, string> Producer = new ProducerBuilder<string, string>(config).Build();

            while (!token.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                var msg = new Message<string, string> { Key = now.ToString(), Value = now.ToString() };
                Producer.ProduceAsync("test3", msg).GetAwaiter().GetResult();
                Console.WriteLine("ServiceA topic test3 produced " + now.ToString());
            }
        }
    }
}
