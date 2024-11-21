using EcosaveAPI.Models;
using EcosaveAPI.Models.Responses;
using EcosaveAPI.Repositories.Interfaces;
using EcosaveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcosaveAPI.Controllers
{
    [Route("ecosave/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGptService _gptService;

        public UsuariosController(IUsuarioRepository usuarioRepository, IGptService gptService)
        {
            _usuarioRepository = usuarioRepository;
            _gptService = gptService;
        }

        #region GetUsuarios
        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os usuários", Description = "Retorna uma lista de todos os usuários cadastrados.")]
        [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(IEnumerable<Usuario>))]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(usuarios);
        }
        #endregion

        #region GetUsuario
        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um usuário pelo ID", Description = "Retorna um usuário específico com base no ID informado.")]
        [SwaggerResponse(200, "Usuário encontrado", typeof(Usuario))]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { Mensagem = "Usuário não encontrado." });
            }

            return Ok(usuario);
        }
        #endregion

        #region PostUsuario
        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo usuário", Description = "Cria um novo registro de usuário.")]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(Usuario))]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _usuarioRepository.AddAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }
        #endregion

        #region PutUsuario
        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um usuário", Description = "Atualiza os dados de um usuário existente com base no ID informado.")]
        [SwaggerResponse(204, "Usuário atualizado com sucesso")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(new { Mensagem = "O ID informado não corresponde ao usuário." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _usuarioRepository.UpdateAsync(usuario);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _usuarioRepository.GetByIdAsync(id) == null)
                {
                    return NotFound(new { Mensagem = "Usuário não encontrado." });
                }

                throw;
            }

            return NoContent();
        }
        #endregion

        #region DeleteUsuario
        /// <summary>
        /// Exclui um usuário pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um usuário", Description = "Remove um registro de usuário com base no ID informado.")]
        [SwaggerResponse(204, "Usuário excluído com sucesso")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { Mensagem = "Usuário não encontrado." });
            }

            await _usuarioRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion

        #region CalcularMediaAvaliacao
        /// <summary>
        /// Avalia o gasto médio de um usuário.
        /// </summary>
        [HttpGet("{id}/calcular-media-avaliacao")]
        [SwaggerOperation(Summary = "Avalia o gasto médio de um usuário", Description = "Avalia o gasto médio e retorna uma mensagem personalizada.")]
        [SwaggerResponse(200, "Avaliação realizada com sucesso", typeof(AvaliacaoConsumoResponse))]
        public async Task<IActionResult> CalcularMediaAvaliacao(int id)
        {
            var gasto_brasileiro_energia = 2250;
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new AvaliacaoConsumoResponse
                {
                    Avaliacao = "Erro",
                    Mensagem = "Usuário não encontrado.",
                    Dica = null
                });
            }

            var gastoMedio = await _usuarioRepository.ObterGastoMedioAsync(id);

            if (gastoMedio > gasto_brasileiro_energia)
            {
                var respostaGpt = await _gptService.ObterDicaReducaoConsumoAsync();
                return Ok(new AvaliacaoConsumoResponse
                {
                    Avaliacao = "Alto",
                    Mensagem = "Seu consumo está acima da média.",
                    Dica = respostaGpt
                });
            }

            return Ok(new AvaliacaoConsumoResponse
            {
                Avaliacao = "Normal",
                Mensagem = "Seu consumo está dentro da média.",
                Dica = "Continue monitorando seu consumo para manter-se eficiente."
            });
        }
        #endregion
    }
}