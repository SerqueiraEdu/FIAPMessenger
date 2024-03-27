using FIAPMessanger.Core;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FIAPMessanger.Produtor.Controllers
{
    [ApiController]
    [Route("/pedido")]
    public class PedidoController : Controller
    {
        [HttpPost]
        public IActionResult Post()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "fila",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var message = JsonSerializer.Serialize(
                        new Pedido(1,
                            new Usuario(
                                2, "Eduardo", "edu@edu.com")));

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "fila",
                        basicProperties: null,
                        body: body);
                }
            }

            return Ok();
        }
    }
}
