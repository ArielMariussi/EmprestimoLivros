using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Controllers
{
    [Authorize]
    public class EmprestimoController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public EmprestimoController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

         private string GetUserId()
        {
            return _userManager.GetUserId(User) ?? string.Empty;
        }



        public IActionResult Index()
        {
            var userId = GetUserId();

            IEnumerable<Emprestimo> emprestimos = _db.Emprestimos
                .Where(e => e.UserId == userId)
                .ToList();

            return View(emprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Emprestimo emprestimo)
        {

            emprestimo.UserId = GetUserId();
            emprestimo.DataUltimaAtualizacao = DateTime.UtcNow;

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso";

                return RedirectToAction("Index");
            }
            return View(emprestimo);

        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {

            var userId = GetUserId();

            var emprestimo = _db.Emprestimos
                .FirstOrDefault(e =>e.Id == id && e.UserId == userId);

            if (emprestimo == null)
            {
                TempData["MensagemErro"] = "Empréstimo não encontrado!";
                return RedirectToAction("Index");
            }

            return View(emprestimo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Emprestimo emprestimo)
        {
            var userId = GetUserId();

            var emprestimoBanco = _db.Emprestimos
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == emprestimo.Id && e.UserId == userId);

            if(emprestimoBanco == null)
            {
                TempData["MensagemErro"] = "Voce nao tem permissao para editar este empréstimo!";
                return RedirectToAction("Index");
            }

            emprestimo.UserId = userId;
            emprestimo.DataUltimaAtualizacao = DateTime.UtcNow;
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _db.Emprestimos.Update(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizado com sucesso";

                return RedirectToAction("Index");
            }

            return View(emprestimo);

        }

        [HttpGet]
          public IActionResult Excluir(int? id)
        {

            var userId = GetUserId();

            var emprestimo = _db.Emprestimos
                .FirstOrDefault(e => e.Id == id && e.UserId == userId);

            if (emprestimo == null)
            {
                TempData["MensagemErro"] = "Empréstimo não encontrado!";
                return RedirectToAction("Index");
            }

            return View(emprestimo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(Emprestimo emprestimo)
        {
            var userId = GetUserId();

            var emprestimoBanco = _db.Emprestimos
                .FirstOrDefault(e => e.Id == emprestimo.Id && e.UserId == userId);

            if (emprestimoBanco == null)
            {
                TempData["MensagemErro"] = "Empréstimo não encontrado!";
                return RedirectToAction("Index");
            }

            _db.Emprestimos.Remove(emprestimoBanco);
            _db.SaveChanges();
            TempData["MensagemSucesso"] = "Exclusão realizada com sucesso!";
            return RedirectToAction("Index");


        }
    }
}

