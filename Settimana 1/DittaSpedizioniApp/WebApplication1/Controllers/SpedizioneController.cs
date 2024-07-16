using Microsoft.AspNetCore.Mvc;
using DittaSpedizioniApp.Models;
using DittaSpedizioniApp.Services;


namespace DittaSpedizioniApp.Controllers
{
    public class SpedizioneController : Controller
    {
        private readonly SpedizioneService _spedizioneService;

        public SpedizioneController(SpedizioneService spedizioneService)
        {
            _spedizioneService = spedizioneService;
        }

        public IActionResult Index()
        {
            var spedizioni = _spedizioneService.GetSpedizioni();
            return View(spedizioni);
        }

        public IActionResult Details(int id)
        {
            var spedizione = _spedizioneService.GetSpedizioneById(id);
            if (spedizione == null)
            {
                return NotFound();
            }
            return View(spedizione);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                _spedizioneService.AggiungiSpedizione(spedizione);
                return RedirectToAction(nameof(Index));
            }
            return View(spedizione);
        }

        public IActionResult Edit(int id)
        {
            var spedizione = _spedizioneService.GetSpedizioneById(id);
            if (spedizione == null)
            {
                return NotFound();
            }
            return View(spedizione);
        }

        [HttpPost]
        public IActionResult Edit(int id, Spedizione spedizione)
        {
            if (id != spedizione.IdSpedizione)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _spedizioneService.ModificaSpedizione(spedizione);
                return RedirectToAction(nameof(Index));
            }
            return View(spedizione);
        }

        public IActionResult Delete(int id)
        {
            var spedizione = _spedizioneService.GetSpedizioneById(id);
            if (spedizione == null)
            {
                return NotFound();
            }
            return View(spedizione);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _spedizioneService.EliminaSpedizione(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
