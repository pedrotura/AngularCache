using Moq;
using web_app_performance.Controllers;
using web_app_repository.Interfaces;
using web_app_domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace test
{
    public class UsuarioControllerTest
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTest()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ListarUsuariosOk()
        {
            //arrange
            var usuarios = new List<Usuario>()
            {
                new Usuario()
                {
                    Id = 1,
                    Nome = "Pedro Tura",
                    Email = "example@email.com"
                }
            };
            _usuarioRepositoryMock.Setup(r => r.ListarUsuarios()).ReturnsAsync(usuarios);

            //act
            var result = await _controller.GetUsuario();

            //assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(usuarios), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task Get_ListarUsuariosNotFound()
        {
            //arrange
            _usuarioRepositoryMock.Setup(u => u.ListarUsuarios()).ReturnsAsync((IEnumerable<Usuario>)null);
            
            //act
            var result = await _controller.GetUsuario();
            
            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarUsuario()
        {
            //arrange
            var usuario = new Usuario()
            {
                Id = 1,
                Nome = "Pedro Tura",
                Email = "example@email.com"
            };
            _usuarioRepositoryMock.Setup(u => u.SalvarUsuario(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            //act
            var result = await _controller.PostUsuario(usuario);

            //assert
            Assert.IsType<OkObjectResult>(result);
            _usuarioRepositoryMock.Verify(u => u.SalvarUsuario(It.IsAny<Usuario>()), Times.Once);

        }

    }
}
