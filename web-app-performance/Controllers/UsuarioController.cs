using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using web_app_domain;
using web_app_repository.Interfaces;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            /*string key = "getusuario";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10));
            string user = await db.StringGetAsync(key);

            if (!string.IsNullOrEmpty(user))
            {
                return Ok(user);
            }*/

            var usuarios = await _repository.ListarUsuarios();

            if (usuarios == null)
            {
                return NotFound();
            }

            string usuariosJson = JsonConvert.SerializeObject(usuarios);
           // await db.StringSetAsync(key, usuariosJson);

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            await _repository.SalvarUsuario(usuario);

            //apaga o cache
            /*string key = "getusuario";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);*/

            return Ok(new {mensagem = "Criado com sucesso!"});

        }

        [HttpPut]
        public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
        {
            await _repository.AtualizarUsuario(usuario);

            //apaga o cache
            string key = "getusuario";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoverUsuario(id);

            //apaga o cache
            string key = "getusuario";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();

        }

    }
}
