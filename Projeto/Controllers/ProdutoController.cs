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
            Produto modelo = new Produto();
            return PartialView("_Create", modelo);            
        }


    
        [HttpPost]
        public async Task<IActionResult> Create(Produto modelo)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(int id)
        {
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            return PartialView("_Edit", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Produto modelo)
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

     


        public async Task<IActionResult> Delete(int id)
        {    
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            if (modelo == null) return NotFound();            

            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var modelo =await _context.Produto.SingleAsync(x => x.Id == id);
            _context.Produto.Remove(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        
        public async Task<IActionResult> Details(int id)
        {         
            var modelo =await _context.Produto.SingleAsync(m => m.Id == id);
                                
            if (modelo == null) return NotFound();            

            return PartialView("_Details", modelo);
        }


              
        private bool ModelExist(int id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }
    }
}