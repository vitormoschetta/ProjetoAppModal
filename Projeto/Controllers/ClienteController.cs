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
using Projeto.Repository;
using Projeto.ViewModels;

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
        public async Task<IActionResult> Create(ClienteViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var result = await _repository.Cadastrar(modelo);
            if (result.Valido == false)
            {
                ModelState.AddModelError(string.Empty, result.Mensagem);
                return View(modelo);
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var modelo = await _repository.BuscarPorId(id);
            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Cliente modelo)
        {
            if (id != modelo.Id)
            {
                ModelState.AddModelError(string.Empty, "Identificador inválido.");
                return View(modelo);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Modelo inválido.");
                return View(modelo);
            }

            var result = await _repository.Atualizar(modelo);
            if (result != true)
            {
                ModelState.AddModelError(string.Empty, "Erro Interno.");
                return View(modelo);
            }

            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Delete(Guid id)
        {
            var modelo = await _repository.BuscarPorId(id);
            return View(modelo);
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