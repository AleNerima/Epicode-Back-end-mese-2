using Microsoft.AspNetCore.Mvc;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;
using System;

namespace DittaSpedizioni.Controllers
{
    public class AggiornamentoSpedizioneController : Controller
    {
        private readonly IAggiornamentoSpedizioneService _aggiornamentoSpedizioneService;

        public AggiornamentoSpedizioneController(IAggiornamentoSpedizioneService aggiornamentoSpedizioneService)
        {
            _aggiornamentoSpedizioneService = aggiornamentoSpedizioneService ?? throw new ArgumentNullException(nameof(aggiornamentoSpedizioneService));
        }

        // GET: /AggiornamentoSpedizione/Index1
        public IActionResult Index1()
        {
            var aggiornamenti = _aggiornamentoSpedizioneService.GetAllAggiornamentiSpedizione();
            return View("Index1", aggiornamenti);
        }

        // GET: /AggiornamentoSpedizione/IndexBySpedizione/{spedizioneId}
        public IActionResult IndexBySpedizione(int spedizioneId)
        {
            var aggiornamenti = _aggiornamentoSpedizioneService.GetAggiornamentiBySpedizioneId(spedizioneId);
            return View("Index1", aggiornamenti);
        }

        // GET: /AggiornamentoSpedizione/Create/{spedizioneId}
        public IActionResult Create(int spedizioneId)
        {
            var aggiornamento = new AggiornamentoSpedizione { Spedizione = spedizioneId };
            return View(aggiornamento);
        }

        // POST: /AggiornamentoSpedizione/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AggiornamentoSpedizione aggiornamento)
        {
            if (ModelState.IsValid)
            {
                _aggiornamentoSpedizioneService.AddAggiornamentoSpedizione(aggiornamento);
                return RedirectToAction(nameof(IndexBySpedizione), new { spedizioneId = aggiornamento.Spedizione });
            }
            return View(aggiornamento);
        }

        // GET: /AggiornamentoSpedizione/Details/{id}
        public IActionResult Details(int id)
        {
            var aggiornamento = _aggiornamentoSpedizioneService.GetAggiornamentoById(id);
            if (aggiornamento == null)
            {
                return NotFound(); // Ritorna una pagina 404 se l'aggiornamento non è trovato
            }
            return View(aggiornamento);
        }

        // Altri metodi come Edit, Delete, ecc. se necessario
    }
}
