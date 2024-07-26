using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AlbergoApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetAllClientiAsync();
            return View("IndexCliente", clienti);
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View("DetailsCliente", cliente);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View("CreateCliente");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodiceFiscale, Nome, Cognome, Email, Telefono, Cellulare, Citta, Provincia")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteService.CreateClienteAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateCliente", cliente);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View("EditCliente", cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente, CodiceFiscale, Nome, Cognome, Email, Telefono, Cellulare, Citta, Provincia")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _clienteService.UpdateClienteAsync(cliente);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View("EditCliente", cliente);
        }


        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View("DeleteCliente", cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteService.DeleteClienteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
