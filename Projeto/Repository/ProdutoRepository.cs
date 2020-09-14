using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.Repository.Interfaces;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProdutoRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Resultado> Cadastrar(ProdutoViewModel viewModel)
        {
            var modelo = _mapper.Map<Produto>(viewModel);
            try
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return new Resultado(true, string.Empty);
            }
            catch (Exception e)
            {
                return new Resultado(false, e.ToString());
            }

        }


        public async Task<ProdutoViewModel> BuscarPorId(Guid id)
        {
            var modelo = await _context.Produto.SingleAsync(x => x.Id == id);
            return _mapper.Map<ProdutoViewModel>(modelo);
        }


        public async Task<Resultado> Atualizar(ProdutoViewModel viewModel)
        {
            try
            {
                var modelo = _mapper.Map<Produto>(viewModel);

                _context.Update(modelo);
                await _context.SaveChangesAsync();
                return new Resultado(true, string.Empty);
            }
            catch (Exception e)
            {
                return new Resultado(false, e.ToString());
            }
        }

        public async Task<Resultado> Excluir(Guid id)
        {
            var modelo = await _context.Produto.SingleAsync(x => x.Id == id);
            try
            {
                _context.Remove(modelo);
                await _context.SaveChangesAsync();
                return new Resultado(true, string.Empty);
            }
            catch (Exception e)
            {
                return new Resultado(false, e.ToString());
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

        public bool Exist(Guid id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }


    }
}