using Dapper;
using MySqlConnector;
using web_app_domain;
using web_app_repository.Interfaces;

namespace web_app_repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MySqlConnection mySqlConnection;
        public ProdutoRepository()
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<Produto>> ListarProdutos()
        {
            await mySqlConnection.OpenAsync();
            string query = "SELECT id, nome, preco, quantidade_estoque, data_criacao FROM produtos";
            var produtos = await mySqlConnection.QueryAsync<Produto>(query);
            await mySqlConnection.CloseAsync();

            return produtos;
        }

        public async Task SalvarProduto(Produto produto)
        {
            await mySqlConnection.OpenAsync();
            string sql = "INSERT INTO produtos(nome, preco, quantidade_estoque, data_criacao) VALUES(@nome, @preco, @quantidade_estoque, @data_criacao)";
            await mySqlConnection.ExecuteAsync(sql, produto);
            await mySqlConnection.CloseAsync();
        }

        public async Task AtualizarProduto(Produto produto)
        {
            await mySqlConnection.OpenAsync();
            string sql = "UPDATE produtos SET nome = @nome, preco = @preco, quantidade_estoque = @quantidade_estoque, data_criacao = @data_criacao WHERE id = @id";
            await mySqlConnection.ExecuteAsync(sql, produto);
            await mySqlConnection.CloseAsync();
        }

        public async Task RemoverProduto(int id)
        {
            await mySqlConnection.OpenAsync();
            string sql = "DELETE FROM produtos WHERE id = @id";
            await mySqlConnection.ExecuteAsync(sql, new { id });
            await mySqlConnection.CloseAsync();
        }
    }
}
