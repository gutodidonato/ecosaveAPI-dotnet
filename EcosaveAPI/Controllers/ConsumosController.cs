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
    public class ConsumosController : ControllerBase
    {
        private readonly IConsumoRepository _consumoRepository;

        public ConsumosController(IConsumoRepository consumoRepository)
        {
            _consumoRepository = consumoRepository;
        }

        #region GetConsumos
        /// <summary>
        /// Obtém todos os consumos.
        /// </summary>
        /// <returns>Lista de consumos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os consumos", Description = "Retorna uma lista de todos os consumos cadastrados.")]
        [SwaggerResponse(200, "Lista de consumos retornada com sucesso", typeof(IEnumerable<Consumo>))]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<IEnumerable<Consumo>>> GetConsumos()
        {
            var consumos = await _consumoRepository.GetAllAsync();
            return Ok(consumos);
        }
        #endregion

        #region GetConsumo
        /// <summary>
        /// Obtém um consumo pelo ID.
        /// </summary>
        /// <param name="id">ID do consumo.</param>
        /// <returns>Consumo correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um consumo pelo ID", Description = "Retorna um consumo específico com base no ID informado.")]
        [SwaggerResponse(200, "Consumo encontrado", typeof(Consumo))]
        [SwaggerResponse(404, "Consumo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Consumo>> GetConsumo(int id)
        {
            var consumo = await _consumoRepository.GetByIdAsync(id);
            if (consumo == null)
            {
                return NotFound();
            }
            return Ok(consumo);
        }
        #endregion

        #region PostConsumo
        /// <summary>
        /// Adiciona um novo consumo.
        /// </summary>
        /// <param name="consumo">Dados do consumo a ser adicionado.</param>
        /// <returns>Consumo criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo consumo", Description = "Cria um novo registro de consumo.")]
        [SwaggerResponse(201, "Consumo criado com sucesso", typeof(Consumo))]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Consumo>> PostConsumo(Consumo consumo)
        {
            await _consumoRepository.AddAsync(consumo);
            return CreatedAtAction(nameof(GetConsumo), new { id = consumo.Id }, consumo);
        }
        #endregion

        #region PutConsumo
        /// <summary>
        /// Atualiza um consumo existente.
        /// </summary>
        /// <param name="id">ID do consumo a ser atualizado.</param>
        /// <param name="consumo">Dados atualizados do consumo.</param>
        /// <returns>Resultado da atualização.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um consumo", Description = "Atualiza os dados de um consumo existente com base no ID informado.")]
        [SwaggerResponse(204, "Consumo atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID não corresponde ao consumo")]
        [SwaggerResponse(404, "Consumo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> PutConsumo(int id, Consumo consumo)
        {
            if (id != consumo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _consumoRepository.UpdateAsync(consumo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _consumoRepository.GetByIdAsync(id) == null)
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

        #region DeleteConsumo
        /// <summary>
        /// Exclui um consumo pelo ID.
        /// </summary>
        /// <param name="id">ID do consumo a ser excluído.</param>
        /// <returns>Resultado da exclusão.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um consumo", Description = "Remove um registro de consumo com base no ID informado.")]
        [SwaggerResponse(204, "Consumo excluído com sucesso")]
        [SwaggerResponse(404, "Consumo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteConsumo(int id)
        {
            var consumo = await _consumoRepository.GetByIdAsync(id);
            if (consumo == null)
            {
                return NotFound();
            }

            await _consumoRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
