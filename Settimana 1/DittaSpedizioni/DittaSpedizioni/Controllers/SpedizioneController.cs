using Microsoft.AspNetCore.Mvc;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;

namespace DittaSpedizioni.Controllers
{
    public class SpedizioneController : Controller
    {
        private readonly ISpedizioneService _spedizioneService;

        public SpedizioneController(ISpedizioneService spedizioneService)
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                _spedizioneService.AddSpedizione(spedizione);
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Spedizione spedizione)
        {
            if (id != spedizione.IdSpedizione)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _spedizioneService.UpdateSpedizione(spedizione);
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
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _spedizioneService.DeleteSpedizione(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
