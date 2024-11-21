using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EcosaveAPI.Controllers
{
    [Route("ecosave/[controller]")]
    [ApiController]
    public class PontosController : ControllerBase
    {
        private readonly IPontoRepository _pontoRepository;

        public PontosController(IPontoRepository pontoRepository)
        {
            _pontoRepository = pontoRepository;
        }

        #region GetPontos
        /// <summary>
        /// Obtém todos os pontos.
        /// </summary>
        /// <returns>Lista de pontos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os pontos", Description = "Retorna uma lista de todos os pontos cadastrados.")]
        [SwaggerResponse(200, "Lista de pontos retornada com sucesso", typeof(IEnumerable<Ponto>))]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<IEnumerable<Ponto>>> GetPontos()
        {
            var pontos = await _pontoRepository.GetAllAsync();
            return Ok(pontos);
        }
        #endregion

        #region GetPonto
        /// <summary>
        /// Obtém um ponto pelo ID.
        /// </summary>
        /// <param name="id">ID do ponto.</param>
        /// <returns>Ponto correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um ponto pelo ID", Description = "Retorna um ponto específico com base no ID informado.")]
        [SwaggerResponse(200, "Ponto encontrado", typeof(Ponto))]
        [SwaggerResponse(404, "Ponto não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Ponto>> GetPonto(int id)
        {
            var ponto = await _pontoRepository.GetByIdAsync(id);
            if (ponto == null)
            {
                return NotFound();
            }
            return Ok(ponto);
        }
        #endregion

        #region PostPonto
        /// <summary>
        /// Adiciona um novo ponto.
        /// </summary>
        /// <param name="ponto">Dados do ponto a ser adicionado.</param>
        /// <returns>Ponto criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo ponto", Description = "Cria um novo registro de ponto.")]
        [SwaggerResponse(201, "Ponto criado com sucesso", typeof(Ponto))]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Ponto>> PostPonto(Ponto ponto)
        {
            await _pontoRepository.AddAsync(ponto);
            return CreatedAtAction(nameof(GetPonto), new { id = ponto.Id }, ponto);
        }
        #endregion

        #region PutPonto
        /// <summary>
        /// Atualiza um ponto existente.
        /// </summary>
        /// <param name="id">ID do ponto a ser atualizado.</param>
        /// <param name="ponto">Dados atualizados do ponto.</param>
        /// <returns>Resultado da atualização.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um ponto", Description = "Atualiza os dados de um ponto existente com base no ID informado.")]
        [SwaggerResponse(204, "Ponto atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID não corresponde ao ponto")]
        [SwaggerResponse(404, "Ponto não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> PutPonto(int id, Ponto ponto)
        {
            if (id != ponto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pontoRepository.UpdateAsync(ponto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _pontoRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region DeletePonto
        /// <summary>
        /// Exclui um ponto pelo ID.
        /// </summary>
        /// <param name="id">ID do ponto a ser excluído.</param>
        /// <returns>Resultado da exclusão.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um ponto", Description = "Remove um registro de ponto com base no ID informado.")]
        [SwaggerResponse(204, "Ponto excluído com sucesso")]
        [SwaggerResponse(404, "Ponto não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeletePonto(int id)
        {
            var ponto = await _pontoRepository.GetByIdAsync(id);
            if (ponto == null)
            {
                return NotFound();
            }

            await _pontoRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}