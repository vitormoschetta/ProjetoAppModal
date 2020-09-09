using System;
using System.ComponentModel.DataAnnotations;
using Projeto.Models;
using Projeto.Validators.DataAnnotations;

namespace Projeto.ViewModels
{
    public class ClienteViewModel : Entity
    {
        [Required(ErrorMessage = "Preenchimento obrigatório")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Deve Conter entre 5 a 100 caracteres")]
        public string Nome { get; set; }

        [ValidaMaiorDeIdade]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório")]
        [MinLength(11, ErrorMessage = "Deve Conter no mínimo 11 caracteres")]
        public string Cpf { get; set; }
    }
}