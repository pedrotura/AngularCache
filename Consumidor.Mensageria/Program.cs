// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string fila = "fila_teste";
channel.QueueDeclare(queue: fila,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, args) =>
{
    var body = args.Body.ToArray();
    var mensagem = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Estoque atualizado: {mensagem}...");
};

channel.BasicConsume(queue: fila,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("Aguardando mensagem");
Console.ReadLine();