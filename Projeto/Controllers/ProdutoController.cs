using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.Util;

namespace Projeto.Controllers
{
    public class ProdutoController: Controller
    {
        private readonly ApplicationDbContext _context;
        public ProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var listaModelo = await _context.Produto.ToListAsync();    
            int pageSize = 5; // itens por pagina
            PaginatedList<Produto> ModelComPaginacao = PaginatedList<Produto>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return View(ModelComPaginacao);    
        }

        public IActionResult Create()
        {                       
            return PartialView("_Create");            
        }


    
        [HttpPost]
        public async Task<IActionResult> Create(Produto modelo)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            return PartialView("_Edit", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Produto modelo)
        {   
            if (id != modelo.Id) return NotFound();

            if (!ModelState.IsValid) return NotFound();

            
            try{
                _context.Update(modelo);
                await _context.SaveChangesAsync();                                                                               
            }
            catch (DbUpdateConcurrencyException){
                if (!ModelExist(modelo.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction("Index");                                   
        }

     


        public async Task<IActionResult> Delete(Guid id)
        {    
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            if (modelo == null) return NotFound();            

            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            _context.Produto.Remove(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        
        public async Task<IActionResult> Details(Guid id)
        {         
            var modelo =await _context.Produto.SingleAsync(m => m.Id == id);
                                
            if (modelo == null) return NotFound();            

            return PartialView("_Details", modelo);
        }


        public async Task<IActionResult> Paginacao(int? pageNumber)
        {                            
            var listaModelo = await _context.Produto.ToListAsync();  
            int pageSize = 5;      
            PaginatedList<Produto> ModelComPaginacao = PaginatedList<Produto>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return PartialView("_TabelaIndex", ModelComPaginacao);
        }



        public async Task<IActionResult> Search(int? pageNumber, string parametro)
        {                            
            var query = "select * from produto where nome like '%" + parametro + "%' ";
            query += " or preco like '%" + parametro + "%' ";
            var listaModelo = await _context.Produto.FromSqlRaw(query).ToListAsync();
            int pageSize = 5;      
            PaginatedList<Produto> ModelComPaginacao = PaginatedList<Produto>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return PartialView("_TabelaIndex", ModelComPaginacao);
        }





              
        private bool ModelExist(Guid id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }
    }
}