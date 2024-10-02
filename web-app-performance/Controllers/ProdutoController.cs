using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;
using System;
using web_app_performance.Model;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private static ConnectionMultiplexer redis;

        [HttpGet]
        public async Task<IActionResult> GetProduto()
        {

            string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyExpireAsync(key, TimeSpan.FromMinutes(10));
            string user = await db.StringGetAsync(key);

            if (!string.IsNullOrEmpty(user))
            {
                return Ok(user);
            }

            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            string query = "SELECT id, nome, preco, quantidade_estoque, data_criacao FROM produtos;";
            var produtos = await connection.QueryAsync<Produto>(query);
            string produtosJson = JsonConvert.SerializeObject(produtos);
            await db.StringSetAsync(key, produtosJson);

            return Ok(produtos);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduto([FromBody] Produto produto)
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            string sql = "INSERT INTO produtos(nome, preco, quantidade_estoque, data_criacao) VALUES(@nome, @preco, @quantidade_estoque, @data_criacao)";
            await connection.ExecuteAsync(sql, produto);

            //apaga o cache
            string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

        [HttpPut]
        public async Task<IActionResult> PutProduto([FromBody] Produto produto)
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            string sql = "UPDATE produtos SET nome = @nome, preco = @preco, quantidade_estoque = @quantidade_estoque, data_criacao = @data_criacao WHERE id = @id";
            await connection.ExecuteAsync(sql, produto);

            //apaga o cache
            string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            string sql = "DELETE FROM produtos WHERE id = @id";
            await connection.ExecuteAsync(sql, new { id });

            //apaga o cache
            string key = "getproduto";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

    }
}
