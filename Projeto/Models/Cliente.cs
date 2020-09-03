using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }        
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }        
        public string Cpf { get; set; }
    }
}