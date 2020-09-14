using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Projeto.Domain.Commands;
using Projeto.Domain.Entities;
using Projeto.Domain.Utils;

namespace Projeto.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> CpfExists(string cpf);

        Task<CommandResult> Cadastrar(Cliente model);


        Task<Cliente> BuscarPorId(Guid id);


        Task<CommandResult> Atualizar(Cliente model);


        Task<CommandResult> Excluir(Guid id);


        Task<PaginatedList<Cliente>> BuscarTodos(int? pageNumber);


        Task<PaginatedList<Cliente>> Procurar(int? pageNumber, string parametro);

    }
}