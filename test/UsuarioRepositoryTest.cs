using Moq;
using web_app_domain;
using web_app_repository.Interfaces;

namespace test
{
    public class UsuarioRepositoryTest
    {
        [Fact]
        public async Task ListarUsuarios()
        {
            //arrange
            var usuarios = new List<Usuario>()
            {
                new Usuario()
                {
                    Id = 1,
                    Nome = "Pedro Tura",
                    Email = "example@email.com"
                },
                new Usuario()
                {
                    Id = 2,
                    Nome = "Santiago Menezes",
                    Email = "example@email.com"
                }
            };
            var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            usuarioRepositoryMock.Setup(u => u.ListarUsuarios()).ReturnsAsync(usuarios);
            var usuarioRepository = usuarioRepositoryMock.Object;

            //act
            var result = await usuarioRepository.ListarUsuarios();

            //assert
            Assert.Equal(usuarios, result);
        }
    }
}
