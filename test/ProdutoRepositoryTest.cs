using Moq;
using web_app_domain;
using web_app_repository.Interfaces;

namespace test
{
    public class ProdutoRepositoryTest
    {
        [Fact]
        public async Task ListarProduto()
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
                },
                new Produto()
                {
                    Id = 2,
                    Nome = "Mochila",
                    Preco = 200.0,
                    Quantidade_estoque = 40,
                    Data_criacao = "10/11/2024"
                }
            };
            var produtoRepositoryMock = new Mock<IProdutoRepository>();
            produtoRepositoryMock.Setup(p => p.ListarProdutos()).ReturnsAsync(produtos);
            var usuarioRepository = produtoRepositoryMock.Object;

            //act
            var result = await usuarioRepository.ListarProdutos();

            //assert
            Assert.Equal(produtos, result);
        }
    }
}
