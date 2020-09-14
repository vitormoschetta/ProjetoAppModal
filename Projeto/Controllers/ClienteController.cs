using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Projeto.Repository;
using Projeto.Domain.Entities;

namespace Projeto.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteRepository _repository;
        public ClienteController(ClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (pageNumber == null) pageNumber = 1;
            var listaModelo = await _repository.BuscarTodos(pageNumber);
            return View(listaModelo);
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Cliente model)
        {
            if (!ModelState.IsValid) return View(model);

            var resultado = await _repository.Cadastrar(model);
            if (resultado.Success == false)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                return View(model);
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _repository.BuscarPorId(id);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Cliente model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError(string.Empty, "Identificador inválido.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Modelo inválido.");
                return View(model);
            }

            var resultado = await _repository.Atualizar(model);
            if (resultado.Success == false)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _repository.BuscarPorId(id);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var resultado = await _repository.Excluir(id);

            if (resultado.Success != true)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                return View();
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Paginacao(int? pageNumber)
        {
            if (pageNumber == null) pageNumber = 1;
            var listaModelo = await _repository.BuscarTodos(pageNumber);
            return PartialView("_TabelaIndex", listaModelo);
        }



        public async Task<IActionResult> Search(int? pageNumber, string parametro)
        {
            var listaModelo = await _repository.Procurar(pageNumber, parametro);
            return PartialView("_TabelaIndex", listaModelo);
        }

    }
}