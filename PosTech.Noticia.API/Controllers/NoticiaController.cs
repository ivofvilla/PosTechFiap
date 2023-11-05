using MediatR;
using Microsoft.AspNetCore.Mvc;
using PosTech.Noticia.API.Commands.Noticia;
using PosTech.Noticia.API.Noticias;
using PosTech.Noticia.API.Noticias.Update;
using PosTech_Fase.Queries;
using PosTech_Fase2.Application.Queries.Noticias.GetById;

namespace PosTech.Noticia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoticiaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoticiaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddNoticiaAsync([FromBody] CreateNoticiaCommand command, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(command, cancellationToken))
            {
                return Ok("Notícia criada com sucesso!"); 
            }

            return BadRequest("Ocorreu um erro!");
        }

        [HttpGet]
        public async Task<ActionResult<GetAllResult>?> GetAllNoticiaAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAllResult>> GetNoticiaById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetNoticiaByIdQuery { Id = id };
            var noticias = await _mediator.Send(query, cancellationToken);

            if (noticias == null)
            {
                return NotFound();
            }

            return Ok(noticias);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNoticia(Guid id, [FromBody] UpdateNoticiaCommand command, CancellationToken cancellationToken)
        {
            command.SetId(id);
            if (await _mediator.Send(command, cancellationToken))
            {
                return Ok("Notícia atualizada com sucesso!"); 
            }

            return BadRequest("Ocorreu um erro!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarNoticia(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteNoticiaCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return Ok("Notícia deletada com sucesso!");
        }
    }
}
