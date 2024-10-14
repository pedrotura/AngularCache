using Moq;
using web_app_performance.Controllers;
using web_app_repository.Interfaces;
using web_app_domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace test
{
    public class ProdutoControllerTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly ProdutoController _controller;

        public ProdutoControllerTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _controller = new ProdutoController(_produtoRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ListarProdutosOk()
        {
            //arrange
            var produtos = new List<Produto>()
            {
                new Produto()
                {
                    Id = 1,
                    Nome = "Camiseta",
                    Preco = 60.0,
                    Quantidade_estoque = 100,
                    Data_criacao = "20/12/2024"
                }
            };
            _produtoRepositoryMock.Setup(r => r.ListarProdutos()).ReturnsAsync(produtos);

            //act
            var result = await _controller.GetProduto();

            //assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(produtos), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task Get_ListarProdutosNotFound()
        {
            //arrange
            _produtoRepositoryMock.Setup(p => p.ListarProdutos()).ReturnsAsync((IEnumerable<Produto>)null);

            //act
            var result = await _controller.GetProduto();

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarProduto()
        {
            //arrange
            var produto = new Produto()
            {
                Id = 1,
                Nome = "Camiseta",
                Preco = 60.0,
                Quantidade_estoque = 100,
                Data_criacao = "20/12/2024"
            };
            _produtoRepositoryMock.Setup(p => p.SalvarProduto(It.IsAny<Produto>())).Returns(Task.CompletedTask);

            //act
            var result = await _controller.PostProduto(produto);

            //assert
            Assert.IsType<OkObjectResult>(result);
            _produtoRepositoryMock.Verify(p => p.SalvarProduto(It.IsAny<Produto>()), Times.Once);

        }

    }
}
