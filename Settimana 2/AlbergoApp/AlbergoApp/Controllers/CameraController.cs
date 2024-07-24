using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;


namespace AlbergoApp.Controllers
{
    public class CameraController : Controller
    {
        private readonly ICameraService _cameraService;

        public CameraController(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

      
        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetAllCamereAsync();
            return View("IndexCamera", camere);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var camera = await _cameraService.GetCameraByIdAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View("DetailsCamera", camera);
        }

        public IActionResult Create()
        {
            return View("CreateCamera");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero, Tipo, Prezzo")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                await _cameraService.CreateCameraAsync(camera);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateCamera", camera);
        }

        
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
        public async Task<IActionResult> Edit(int id, [Bind("IdCamera, Numero, Tipo, Prezzo")] Camera camera)
        {
            if (id != camera.IdCamera)
            {
                return NotFound();
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
