using System;
using System.Threading.Tasks;
using Projeto.Models;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Task<ResultMessage> Cadastrar(ClienteViewModel viewModel);

        Task<ClienteViewModel> BuscarPorId(Guid id);

        Task<ResultMessage> Atualizar(ClienteViewModel viewModel);

        Task<ResultMessage> Excluir(Guid id);

        Task<PaginatedList<Cliente>> BuscarTodos(int? pageNumber);

        Task<PaginatedList<Cliente>> Procurar(int? pageNumber, string parametro);

        bool Exist(Guid id);
    }
}