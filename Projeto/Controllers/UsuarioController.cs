using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<Usuario> _userManager;

        public UsuarioController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var listaModelo = await _context.Usuario.ToListAsync();
            return View(listaModelo);
        }

        // public IActionResult Create()
        // {           
        //     Usuario modelo = new Usuario();
        //     return PartialView("_Create", modelo);            
        // }



        // [HttpPost]
        // public async Task<IActionResult> Create(Usuario modelo)
        // {
        //     modelo.EmailConfirmed = true;
        //     _context.Add(modelo);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction("Index");
        // }



        public async Task<IActionResult> Edit(string id)
        {
            var modelo = await _context.Usuario.SingleAsync(x => x.Id == id);
            ViewBag.Perfis = _roleManager.Roles;
            return PartialView("_Edit", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, Usuario modelo)
        {
            if (id != modelo.Id) return NotFound();

            var user = _context.Usuario.FirstOrDefault(x => x.Id == modelo.Id);
            user.UserName = modelo.UserName;
            user.Email = modelo.Email;
            user.Status = modelo.Status;
            user.Perfil = modelo.Perfil;

            if (modelo.Status == "Ativo") user.EmailConfirmed = true;
            else user.EmailConfirmed = false;

            if (!ModelState.IsValid) return NotFound();

            try
            {
                _context.Update(user);
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



        public async Task<IActionResult> Delete(string id)
        {
            var modelo = await _context.Usuario.SingleAsync(x => x.Id == id);
            if (modelo == null) return NotFound();

            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var modelo = await _context.Usuario.SingleAsync(x => x.Id == id);
            _context.Usuario.Remove(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Details(string id)
        {
            var modelo = await _context.Usuario.SingleAsync(m => m.Id == id);

            if (modelo == null) return NotFound();

            return PartialView("_Details", modelo);
        }



        private bool ModelExist(string id)
        {
            return _context.Usuario.Any(x => x.Id == id);
        }


        public async Task<IActionResult> Perfis(string id)
        {
            var Roles = _roleManager.Roles;

            var modelo = await _context.Usuario.SingleAsync(m => m.Id == id);
            ViewBag.PerfilUsuario = await _userManager.GetRolesAsync(modelo);
            ViewBag.UsuarioId = modelo.Id;

            return PartialView("_Perfis", Roles);
        }


        public void AtualizarPerfil(string usuarioId, string roleName, string acao)
        {
            var usuario = _userManager.FindByIdAsync(usuarioId).Result;
            IdentityResult result;

            if (acao == "adicionar")
            {
                result = _userManager.AddToRoleAsync(usuario, roleName).Result;
                usuario.Perfil = roleName;
            }
            else
            {
                result = _userManager.RemoveFromRoleAsync(usuario, roleName).Result;

                var roles = _userManager.GetRolesAsync(usuario).Result;
                if (roles != null)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        usuario.Perfil = roles[i];
                    }
                }
                else
                {
                    usuario.Perfil = string.Empty;
                }

            }

            _context.Update(usuario);
            _context.SaveChangesAsync();


            if (!result.Succeeded) Errors(result);

        }


        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }



        // [HttpPost]
        // public async Task<IActionResult> EditPerfil(string id, Usuario modelo)
        // {   
        //     var user = _context.Usuario.FirstOrDefault(x => x.Id == modelo.Id);

        //     if (user == null) return NotFound();

        //     var result = await _userManager.AddToRoleAsync(user, modelo.Perfil);

        //     if (!result.Succeeded) Errors(result);

        //     return RedirectToAction("Index");
        // }






        // private void Errors(IdentityResult result)
        // {
        //     foreach (IdentityError error in result.Errors)
        //         ModelState.AddModelError("", error.Description);
        // }
    }
}