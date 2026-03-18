

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StatisticService
{
    public class Author
    {
        public string Name { get; set; } = string.Empty;
        public string SureName { get; set; } = string.Empty;
    }
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, e) =>
            {
                // отримуємо байти повідомлення
                var body = e.Body.ToArray();

                // конвертуємо у string
                var json = Encoding.UTF8.GetString(body);

                // десеріалізуємо JSON у об'єкт
                var message = JsonSerializer.Deserialize<Author>(json);

                Console.WriteLine($"Name: {message.Name}");
                Console.WriteLine($"Surename: {message.SureName}");
            };

            await channel.BasicConsumeAsync(
                queue: "Authors",
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("Waiting messages...");
            Console.ReadLine();
        }
    }
}
