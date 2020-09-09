using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.Repository;
using Projeto.Util;
using Projeto.ViewModels;

namespace Projeto.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepository _repository;
        public ProdutoController(ProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            if (pageNumber == null) pageNumber = 1;
            var listaModelo = await _repository.BuscarTodos(pageNumber);
            return View(listaModelo);
        }


        public IActionResult Create() => PartialView("_Create");

        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel viewModel)
        {
            var result = await _repository.Cadastrar(viewModel);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var modelo = await _repository.BuscarPorId(id);
            return PartialView("_Edit", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                ModelState.AddModelError(string.Empty, "Identificador inválido.");
                return View(viewModel);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Modelo inválido.");
                return View(viewModel);
            }

            var result = await _repository.Atualizar(viewModel);
            if (result != true)
            {
                ModelState.AddModelError(string.Empty, "Erro Interno.");
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Delete(Guid id)
        {
            var modelo = await _repository.BuscarPorId(id);
            return PartialView("_Delete", modelo);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var result = await _repository.Excluir(id);

            if (result != true)
            {
                ModelState.AddModelError(string.Empty, "Erro Interno.");
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