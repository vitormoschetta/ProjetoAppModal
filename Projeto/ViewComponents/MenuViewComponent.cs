using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Microsoft.EntityFrameworkCore;

namespace Projeto.ViewComponents
{
    public class MenuViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public MenuViewComponent(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string Perfil)
        {
            var x = await _context.Cliente.SingleOrDefaultAsync(x => x.Cpf == "0");

            switch (Perfil)
            {
                case "Admin": 
                    return View("_MenuAdmin");
                case "Escola":
                    return View("_MenuUsuario");               
                default:
                    return View("_MenuUsuario");
            }
                       


        }
    }
}