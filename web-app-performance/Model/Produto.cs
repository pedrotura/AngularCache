using System;

namespace web_app_performance.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade_estoque { get; set; }
        public string Data_criacao { get; set; }
    }
}
