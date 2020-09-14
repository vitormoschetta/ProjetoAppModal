using System;
using System.Threading.Tasks;
using Projeto.Models;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Resultado> Cadastrar(ProdutoViewModel viewModel);

        Task<ProdutoViewModel> BuscarPorId(Guid id);

        Task<Resultado> Atualizar(ProdutoViewModel viewModel);

        Task<Resultado> Excluir(Guid id);

        Task<PaginatedList<Produto>> BuscarTodos(int? pageNumber);

        Task<PaginatedList<Produto>> Procurar(int? pageNumber, string parametro);

        bool Exist(Guid id);
    }
}