using Microsoft.AspNetCore.Mvc;
using DittaSpedizioniApp.Models;
using DittaSpedizioniApp.Services;


namespace DittaSpedizioniApp.Controllers
{
    public class SpedizioneController : ControllerBase
    {
        private readonly SpedizioneService _spedizioneService;

        public SpedizioneController(SpedizioneService spedizioneService)
        {
            _spedizioneService = spedizioneService;
        }

        [HttpGet]
        public ActionResult<List<Spedizione>> Get()
        {
            var spedizioni = _spedizioneService.GetSpedizioni();
            return Ok(spedizioni);
        }

        [HttpGet("{id}")]
        public ActionResult<Spedizione> GetById(int id)
        {
            var spedizione = _spedizioneService.GetSpedizioneById(id);
            if (spedizione == null)
            {
                return NotFound();
            }
            return Ok(spedizione);
        }

        [HttpPost]
        public IActionResult Post(Spedizione spedizione)
        {
            _spedizioneService.AggiungiSpedizione(spedizione);
            return CreatedAtAction(nameof(GetById), new { id = spedizione.IdSpedizione }, spedizione);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Spedizione spedizione)
        {
            if (id != spedizione.IdSpedizione)
            {
                return BadRequest();
            }

            _spedizioneService.ModificaSpedizione(spedizione);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _spedizioneService.EliminaSpedizione(id);
            return NoContent();
        }
    }
}
