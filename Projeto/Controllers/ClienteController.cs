using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Projeto.Util;
using System;

namespace Projeto.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public ClienteController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        // Using Dapper se for preciso
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return connection;
        }


        public async Task<IActionResult> Index(int? pageNumber)
        {
            var listaModelo = await _context.Cliente.ToListAsync();          
            int pageSize = 5; // itens por pagina
            PaginatedList<Cliente> ModelComPaginacao = PaginatedList<Cliente>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return View(ModelComPaginacao);
        }

        public IActionResult Create()
        {
            return PartialView("_CreateExists", new Cliente());
        }



        [HttpPost]
        public async Task<IActionResult> Create(Cliente modelo)
        {            
            if (!ModelState.IsValid){
                return NotFound();
            } 
          
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<string> CpfExistsCreate(string Cpf)
        {
            var modelo = await _context.Cliente.SingleOrDefaultAsync(x => x.Cpf == Cpf);
            if (modelo != null) return "true";
            return "false";
        }

        public async Task<string> CpfExistsEdit(Guid id, string Cpf)
        {
            var result = "true";
            //var modelo = await _context.Cliente.SingleOrDefaultAsync(x => x.Cpf == Cpf);
            var lista = await _context.Cliente.Where(x => x.Cpf == Cpf).ToListAsync();
            if (lista.Count() < 1 ) result = "false";
            if (lista.Count() == 1){
                if(lista[0].Id == id) result = "false";
                else result = "true";
            }         
            return result;           
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            return PartialView("_EditExists", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Cliente modelo)
        {
            if (id != modelo.Id) return NotFound();

            if (!ModelState.IsValid) return NotFound();


            try
            {
                _context.Update(modelo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExist(modelo.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Delete(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            if (modelo == null) return NotFound();

            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(x => x.Id == id);
            _context.Cliente.Remove(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Details(Guid id)
        {
            var modelo = await _context.Cliente.SingleAsync(m => m.Id == id);

            if (modelo == null) return NotFound();

            return PartialView("_Details", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> BuscaDinamica(string texto, string parametro, int? pageNumber)
        {            
            if (texto == null) texto = "";         
            var listaModelo = await _context.Cliente.FromSqlRaw("select * from cliente where " + parametro + " like '%" + texto + "%'").ToListAsync();  
            int pageSize = 5;      
            PaginatedList<Cliente> ModelComPaginacao = PaginatedList<Cliente>.Create(listaModelo, pageNumber ?? 1, pageSize);
            return PartialView("_TabelaIndex", ModelComPaginacao);
        }

        private bool ModelExist(Guid id)
        {
            return _context.Cliente.Any(x => x.Id == id);
        }



        private async void DapperFunction()
        {
            using (var con = new SqlConnection(GetConnection()))
            {
                for (int i = 0; i < 50; i++)
                {
                    await con.ExecuteAsync("insert into cliente(nome, dataNascimento) values(@nome, @dataNascimento)", 
                            new {nome = "vitor" + i.ToString(), dataNascimento = "28/05/1989"} );
                }
            }
        }



        
    }
}