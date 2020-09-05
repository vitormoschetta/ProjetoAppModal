using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Repository
{
    public class ProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProdutoRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<dynamic> Cadastrar(ProdutoViewModel viewModel)
        {
            var modelo = _mapper.Map<Produto>(viewModel);
            try
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }


        public async Task<ProdutoViewModel> BuscarPorId(Guid id)
        {
            var modelo = await _context.Produto.SingleAsync(x => x.Id == id);
            return _mapper.Map<ProdutoViewModel>(modelo);
        }


        public async Task<dynamic> Atualizar(Produto modelo)
        {
            try
            {
                _context.Update(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<dynamic> Excluir(Guid id)
        {
            var modelo = await _context.Produto.SingleAsync(x => x.Id == id);
            try
            {
                _context.Remove(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }


        public async Task<PaginatedList<Produto>> BuscarTodos(int? pageNumber)
        {
            var listaModelo = await _context.Produto.ToListAsync();
            int pageSize = 5; // itens por página / paginação
            PaginatedList<Produto> ModelComPaginacao = PaginatedList<Produto>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return ModelComPaginacao;
        }


        public async Task<PaginatedList<Produto>> Procurar(int? pageNumber, string parametro)
        {
            var query = "select * from produto where nome like '%" + parametro + "%' ";
            query += " or preco like '%" + parametro + "%' ";
            var listaModelo = await _context.Produto.FromSqlRaw(query).ToListAsync();
            int pageSize = 5;
            PaginatedList<Produto> ModelComPaginacao = PaginatedList<Produto>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return ModelComPaginacao;
        }

        private bool Exist(Guid id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }
    }
}