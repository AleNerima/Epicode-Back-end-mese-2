using Microsoft.AspNetCore.Mvc;
using DittaSpedizioniApp.Models;
using DittaSpedizioniApp.Services;


namespace DittaSpedizioniApp.Controllers
{
    public class AggiornamentoSpedizioneController : ControllerBase
    {
        private readonly AggiornamentoSpedizioneService _aggiornamentoSpedizioneService;

        public AggiornamentoSpedizioneController(AggiornamentoSpedizioneService aggiornamentoSpedizioneService)
        {
            _aggiornamentoSpedizioneService = aggiornamentoSpedizioneService;
        }

        [HttpGet("spedizione/{id}")]
        public ActionResult<List<AggiornamentoSpedizione>> GetBySpedizioneId(int id)
        {
            var aggiornamenti = _aggiornamentoSpedizioneService.GetAggiornamentiBySpedizioneId(id);
            return Ok(aggiornamenti);
        }

        [HttpPost]
        public IActionResult Post(AggiornamentoSpedizione aggiornamento)
        {
            _aggiornamentoSpedizioneService.AggiungiAggiornamento(aggiornamento);
            return CreatedAtAction(nameof(GetBySpedizioneId), new { id = aggiornamento.Spedizione }, aggiornamento);
        }
    }
}
