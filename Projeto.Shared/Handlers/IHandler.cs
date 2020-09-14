using System.Threading.Tasks;
using Projeto.Shared.Commands;

namespace Projeto.Shared.Handlers
{
    public interface IHandler<T> where T : ICommand
    {
        Task<ICommandResult> Handle(T command);
    }
}