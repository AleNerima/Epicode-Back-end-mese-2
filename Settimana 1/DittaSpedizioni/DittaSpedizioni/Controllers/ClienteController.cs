using Microsoft.AspNetCore.Mvc;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;


namespace DittaSpedizioni.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public IActionResult Index()
        {
            var clienti = _clienteService.GetClienti();
            return View(clienti);
        }

        public IActionResult Details(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _clienteService.AddCliente(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }


        public IActionResult Edit(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _clienteService.UpdateCliente(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _clienteService.DeleteCliente(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
