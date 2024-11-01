using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using web_app_domain;
using web_app_repository.Interfaces;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IProdutoRepository _repository;

        public ProdutoController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduto()
        {

            /*string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyExpireAsync(key, TimeSpan.FromMinutes(10));
            string user = await db.StringGetAsync(key);

            if (!string.IsNullOrEmpty(user))
            {
                return Ok(user);
            }*/

            var produtos = await _repository.ListarProdutos();

            if (produtos == null)
            {
                return NotFound();
            }

            string produtosJson = JsonConvert.SerializeObject(produtos);
            //await db.StringSetAsync(key, produtosJson);

            return Ok(produtos);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduto([FromBody] Produto produto)
        {
            //await _repository.SalvarProduto(produto);

            //apaga o cache
            /*string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);*/

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

            string mensagem = JsonConvert.SerializeObject(produto);
            var body = Encoding.UTF8.GetBytes(mensagem);

            channel.BasicPublish(exchange: "",
                                 routingKey: fila,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("Estoque atualizado com Sucesso");
            Console.ReadLine();

            return Ok(mensagem);

        }

        [HttpPut]
        public async Task<IActionResult> PutProduto([FromBody] Produto produto)
        {
            await _repository.AtualizarProduto(produto);

            //apaga o cache
            /*string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);*/

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            await _repository.RemoverProduto(id);

            //apaga o cache
            string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

    }
}
