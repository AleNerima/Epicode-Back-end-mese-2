using Microsoft.AspNetCore.Mvc;
using DittaSpedizioniApp.Models;
using DittaSpedizioniApp.Services;


namespace DittaSpedizioniApp.Controllers
{
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {
            var clienti = _clienteService.GetClienti();
            return Ok(clienti);
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            _clienteService.AggiungiCliente(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return BadRequest();
            }

            _clienteService.ModificaCliente(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _clienteService.EliminaCliente(id);
            return NoContent();
        }
    }
}
