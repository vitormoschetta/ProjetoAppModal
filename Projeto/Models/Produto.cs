using System;

namespace Projeto.Models
{
    public class Produto : Entity
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}