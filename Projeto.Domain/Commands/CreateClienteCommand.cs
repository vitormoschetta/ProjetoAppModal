using System;
using Flunt.Notifications;
using Flunt.Validations;
using Projeto.Shared.Commands;

namespace Projeto.Domain.Commands
{
    public class CreateClienteCommand : Notifiable, ICommand
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }


        // Essa validação nem deixa chegar nas entidades, caso inválido:
        // Tbm está implementada na entidade Cliente. Verificar onde fica melhor deixar a implementação
        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(Nome, 3, "Nome", "Nome tem que ter pelo menos 3 caracteres")
                .HasLen(Cpf, 11, "CPF", "CPF deve conter 11 digitos")
                .IsEmail(Email, "Email", "Digite um email válido")
            );
        }
    }
}