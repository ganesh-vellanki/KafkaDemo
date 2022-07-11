using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaDemo
{
    class Program
    {
        private static IHostBuilder CreateHostBuilder(string [] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<KafkaProducerHostedService>();
                    collection.AddHostedService<KafkaConsumerHostedService>();
                });
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }

    public class KafkaProducerHostedService : IHostedService
    {
        private readonly ILogger<KafkaProducerHostedService> _logger;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 100; i++)
            {
                var message = $"Hello world {i}";
                _logger.LogInformation(message);
                await this._producer.ProduceAsync("demoTopic", new Message<Null, string>()
                {
                    Value = message
                }, cancellationToken);
            }

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }

    public class KafkaConsumerHostedService : IHostedService
    {
        private readonly ILogger<KafkaConsumerHostedService> _logger;
        private readonly IConsumer<Null,string> _consumer;
        private readonly ConsumerConfig _config;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger)
        {
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = "consumerGroup1"
            };
            _consumer = new ConsumerBuilder<Null, string>(_config).Build();
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("demoTopic");
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _consumer.Consume();
                _logger.LogInformation($"Message received: {message.Message.Value}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}