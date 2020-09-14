using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Controllers
{   
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
           if (User.Identity.IsAuthenticated)
            {
                var usuario = _context.Usuario.SingleOrDefault(x => x.UserName == User.Identity.Name);
                switch (usuario.Perfil) 
                {                            
                    case "Admin": 
                        return RedirectToAction("Index", "Usuario");
                    case "Usuario":
                        return RedirectToAction("Index", "Produto");                  
                    default:
                        return RedirectToAction("Index", "Produto");
                }       
      
            }
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


   
    }
}
