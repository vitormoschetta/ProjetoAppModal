using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;

namespace Projeto.Controllers
{    
    public class PerfilController: Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public PerfilController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public ViewResult Index() => View(_roleManager.Roles);

        public IActionResult Create()
        {
            IdentityRole role = new IdentityRole();
            return PartialView("_Create", role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }


        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var modelo = await _roleManager.FindByIdAsync(id);
            return PartialView("_Edit", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, IdentityRole modelo)
        {
            if (id != modelo.Id) return NotFound();

            if (!ModelState.IsValid) return NotFound();
            
            try
            {
                await _roleManager.UpdateAsync(modelo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExist(modelo.Id).Result)
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Delete(string id)
        {
            var modelo = await _roleManager.FindByIdAsync(id);
            if (modelo == null) return NotFound();

            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var modelo = await _roleManager.FindByIdAsync(id);
            if (modelo != null)
            {
                var result = await _roleManager.DeleteAsync(modelo);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");

            return View("Index", _roleManager.Roles);

        }



        public async Task<IActionResult> Details(string id)
        {
            var modelo = await _roleManager.FindByIdAsync(id);

            if (modelo == null) return NotFound();

            return PartialView("_Details", modelo);
        }



        private async Task<bool> ModelExist(string id)
        {
            var modelo = await _roleManager.FindByIdAsync(id);
            return await _roleManager.RoleExistsAsync(modelo.Name);
        }

       

    }
}