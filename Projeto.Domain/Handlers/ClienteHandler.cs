using System.Threading.Tasks;
using Flunt.Notifications;
using Projeto.Domain.Commands;
using Projeto.Domain.Entities;
using Projeto.Domain.Repositories;
using Projeto.Shared.Commands;
using Projeto.Shared.Handlers;

namespace Projeto.Domain.Handlers
{
    public class ClienteHandler : Notifiable, IHandler<CreateClienteCommand>
    {
        private readonly IClienteRepository _repository;
        public ClienteHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> Handle(CreateClienteCommand command)
        {
            // Faz validações de Modelo - Fast Fail Validations:            
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar o cadastro");
            }

            if (await _repository.CpfExists(command.Cpf))
                AddNotification("CPF", "Este CPF já está em uso.");


            // Gerar Value Objecst (caso esteja trabalhando com VOs)

            // Gerar Entidade Cliente   
            var cliente = new Cliente(command.Nome, command.DataNascimento, command.Cpf, command.Email);

            // Agrupar as Validações
            AddNotifications(command, cliente);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar o cadastro");

            // Salvar cliente no banco:     
            await _repository.Cadastrar(cliente);

            // Retornar informações : command result
            return new CommandResult(true, "Cadastro realizado com sucesso.");
        }
    }
}