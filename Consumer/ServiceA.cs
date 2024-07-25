using Confluent.Kafka;
using dotenv.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    public class ServiceA
    {
        public void DoWork(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var cfg = DotEnv.Read();
                var config = new ConsumerConfig
                {
                    BootstrapServers = cfg["BOOTSTRAPSERVERS"],
                    GroupId = "test",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnablePartitionEof = true,
                    //Debug = "cgrp"
                    EnableAutoCommit = true,
                    //SASL
                    SaslMechanism = SaslMechanism.Plain,
                    SecurityProtocol = SecurityProtocol.SaslPlaintext,
                    SaslUsername = cfg["SASLUSERNAME"],
                    SaslPassword = cfg["SASLPASSWORD"],
                };

                IConsumer<string, string> Consumer = new ConsumerBuilder<string, string>(config).Build();
                //CancellationTokenSource cts = new CancellationTokenSource();
                //cts.CancelAfter(5000);
                //var tokenCancel = cts.Token;

                Consumer = new ConsumerBuilder<string, string>(config).Build();
                Consumer.Subscribe(new string[] { "test" });

                while (!token.IsCancellationRequested)
                {
                    var cr = Consumer.Consume();
                    //произошла системная ошибка
                    if (cr == null)
                    {
                        continue;
                    }

                    //в кафке есть сообщения
                    if (!cr.IsPartitionEOF)
                    {
                        Console.WriteLine("ServiceA topic test consumed " + cr.Message.Value + "");
                    }

                    if (cr.IsPartitionEOF)
                    {
                        continue;
                    }
                }
            }
        }
    }
}