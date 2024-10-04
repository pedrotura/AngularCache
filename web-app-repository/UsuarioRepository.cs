using Dapper;
using MySqlConnector;
using web_app_domain;
using web_app_repository.Interfaces;

namespace web_app_repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MySqlConnection mySqlConnection;
        public UsuarioRepository()
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123";
            using var connection = new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios()
        {
            await mySqlConnection.OpenAsync();
            string query = "SELECT id, nome, email FROM usuarios;";
            var usuarios = await mySqlConnection.QueryAsync<Usuario>(query);
            await mySqlConnection.CloseAsync();

            return usuarios;
        }

        public async Task SalvarUsuario(Usuario usuario)
        {
            await mySqlConnection.OpenAsync();
            string sql = "INSERT INTO usuarios(nome, email) VALUES(@nome, @email)";
            await mySqlConnection.ExecuteAsync(sql, usuario);
            await mySqlConnection.CloseAsync();
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            await mySqlConnection.OpenAsync();
            string sql = "UPDATE usuarios SET nome = @nome, email = @email WHERE id = @id";
            await mySqlConnection.ExecuteAsync(sql, usuario);
            await mySqlConnection.CloseAsync();
        }

        public async Task RemoverUsuario(int id)
        {
            await mySqlConnection.OpenAsync();
            string sql = "DELETE FROM usuarios WHERE id = @id";
            await mySqlConnection.ExecuteAsync(sql, new { id });
            await mySqlConnection.CloseAsync();
        }
    }
}
