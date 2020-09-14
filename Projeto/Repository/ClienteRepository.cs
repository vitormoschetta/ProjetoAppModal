using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Domain.Commands;
using Projeto.Domain.Entities;
using Projeto.Domain.Repositories;
using Projeto.Domain.Utils;

namespace Projeto.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ClienteRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<CommandResult> Cadastrar(Cliente model)
        {
            if (await CpfExists(model.Cpf) == true)
                return new CommandResult(false, "CPF já cadastrado.");

            try
            {
                var modelo = _mapper.Map<Cliente>(model);

                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return new CommandResult(true, string.Empty);
            }
            catch (Exception e)
            {
                return new CommandResult(false, e.ToString());
            }

        }


        public async Task<Cliente> BuscarPorId(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            return modelo;
        }

        public async Task<CommandResult> Atualizar(Cliente model)
        {
            try
            {
                var modelo = _mapper.Map<Cliente>(model);

                _context.Update(modelo);
                await _context.SaveChangesAsync();
                return new CommandResult(true, string.Empty);
            }
            catch (Exception e)
            {
                return new CommandResult(false, e.ToString());
            }
        }

        public async Task<CommandResult> Excluir(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            try
            {
                _context.Remove(modelo);
                await _context.SaveChangesAsync();
                return new CommandResult(true, string.Empty);
            }
            catch (Exception e)
            {
                return new CommandResult(false, e.ToString());
            }
        }


        public async Task<PaginatedList<Cliente>> BuscarTodos(int? pageNumber)
        {
            var listaModelo = await _context.Cliente.ToListAsync();
            int pageSize = 5; // itens por página / paginação
            PaginatedList<Cliente> ModelComPaginacao = PaginatedList<Cliente>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return ModelComPaginacao;
        }


        public async Task<PaginatedList<Cliente>> Procurar(int? pageNumber, string parametro)
        {
            var query = "select * from Cliente where nome like '%" + parametro + "%' ";
            query += " or cpf like '%" + parametro + "%' ";
            var listaModelo = await _context.Cliente.FromSqlRaw(query).ToListAsync();
            int pageSize = 5;
            PaginatedList<Cliente> ModelComPaginacao = PaginatedList<Cliente>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return ModelComPaginacao;
        }

        private bool Exist(Guid id)
        {
            return _context.Cliente.Any(x => x.Id == id);
        }

        public async Task<bool> CpfExists(string cpf)
        {
            var existe = await _context.Cliente.FirstOrDefaultAsync(x => x.Cpf == cpf);
            if (existe != null)
                return true;

            return false;
        }

    }
}