using Projeto.Shared.Commands;

namespace Projeto.Domain.Commands
{
    public class CommandResult : ICommandResult
    {
        // Uma classe de retorno, semalhante ao ResultViewModel
        // Veja que temos dois construtores.
        // Isso quer dizer que podemos instanciar essa classe passando ou não parametros
        public CommandResult()
        { }

        public CommandResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}