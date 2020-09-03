using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        public string Cpf { get; set; }
    }
}