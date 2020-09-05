using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Repository
{
    public class ClienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ClienteRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ErroViewModel> Cadastrar(ClienteViewModel viewModel)
        {
            var existe = await _context.Cliente.FirstOrDefaultAsync(x => x.Nome == viewModel.Nome || x.Cpf == viewModel.Cpf);
            if (existe != null) return new ErroViewModel(false, "Já existe um cadastro com estes parâmetros.");

            try
            {
                var modelo = _mapper.Map<Cliente>(viewModel);

                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return new ErroViewModel(true, string.Empty);
            }
            catch (Exception e)
            {
                return new ErroViewModel(false, e.ToString());
            }

        }


        public async Task<ClienteViewModel> BuscarPorId(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            return _mapper.Map<ClienteViewModel>(modelo);
        }

        public async Task<ErroViewModel> Atualizar(ClienteViewModel viewModel)
        {
            try
            {
                var modelo = _mapper.Map<Cliente>(viewModel);

                _context.Update(modelo);
                await _context.SaveChangesAsync();
                return new ErroViewModel(true, string.Empty);
            }
            catch (Exception e)
            {
                return new ErroViewModel(false, e.ToString());
            }
        }

        public async Task<ErroViewModel> Excluir(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            try
            {
                _context.Remove(modelo);
                await _context.SaveChangesAsync();
                return new ErroViewModel(true, string.Empty);
            }
            catch (Exception e)
            {
                return new ErroViewModel(false, e.ToString());
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
    }
}