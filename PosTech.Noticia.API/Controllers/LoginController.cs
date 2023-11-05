using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosTech.Usuario.API.Commands;
using PosTech_Fase.Queries.Login;

namespace PosTech.Noticia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] GetUsuarioQuery usuario, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(usuario, cancellationToken);
            if (result != null)
            {
                return Ok(result.Token);
            }

            return NotFound("Usuário não encontrado!");
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(CreateUsurarioCommand command, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(command, cancellationToken);
            if (result)
            {
                return Created("", "Usuário cadastrado!");
            }

            return BadRequest();
        }

    }
}
