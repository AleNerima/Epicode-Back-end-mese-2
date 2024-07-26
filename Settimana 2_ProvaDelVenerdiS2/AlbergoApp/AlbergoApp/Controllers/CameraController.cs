using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AlbergoApp.Controllers
{
    public class CameraController : Controller
    {
        private readonly ICameraService _cameraService;

        public CameraController(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetAllCamereAsync();
            return View("IndexCamera", camere);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View("DetailsCamera", camera);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View("CreateCamera");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero, Tipologia, Descrizione")] Camera camera)
        {
            // Validazione per il campo Tipologia
            if (camera.Tipologia != "doppia" && camera.Tipologia != "singola")
            {
                ModelState.AddModelError("Tipologia", "La tipologia deve essere 'doppia' o 'singola'.");
            }

            if (ModelState.IsValid)
            {
                await _cameraService.CreateCameraAsync(camera);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateCamera", camera);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View("EditCamera", camera);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCamera, Numero, Tipologia, Descrizione")] Camera camera)
        {
            if (id != camera.IdCamera)
            {
                return NotFound();
            }

           
            if (camera.Tipologia != "doppia" && camera.Tipologia != "singola")
            {
                ModelState.AddModelError("Tipologia", "La tipologia deve essere 'doppia' o 'singola'.");
            }

            if (ModelState.IsValid)
            {
                var updated = await _cameraService.UpdateCameraAsync(camera);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View("EditCamera", camera);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View("DeleteCamera", camera);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cameraService.DeleteCameraAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
