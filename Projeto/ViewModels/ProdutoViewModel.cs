using System.ComponentModel.DataAnnotations;
using Projeto.Models;

namespace Projeto.ViewModels
{
    public class ProdutoViewModel : Entity
    {
        [Required(ErrorMessage = "Preenchimento obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório")]
        public decimal Preco { get; set; } = 0.00m;
    }
}