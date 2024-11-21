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
    public class DispositivosController : ControllerBase
    {
        private readonly IDispositivoRepository _dispositivoRepository;

        public DispositivosController(IDispositivoRepository dispositivoRepository)
        {
            _dispositivoRepository = dispositivoRepository;
        }

        #region GetDispositivos
        /// <summary>
        /// Obtém todos os dispositivos.
        /// </summary>
        /// <returns>Lista de dispositivos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os dispositivos", Description = "Retorna uma lista de todos os dispositivos cadastrados.")]
        [SwaggerResponse(200, "Lista de dispositivos retornada com sucesso", typeof(IEnumerable<Dispositivo>))]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<IEnumerable<Dispositivo>>> GetDispositivos()
        {
            var dispositivos = await _dispositivoRepository.GetAllAsync();
            return Ok(dispositivos);
        }
        #endregion

        #region GetDispositivo
        /// <summary>
        /// Obtém um dispositivo pelo ID.
        /// </summary>
        /// <param name="id">ID do dispositivo.</param>
        /// <returns>Dispositivo correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um dispositivo pelo ID", Description = "Retorna um dispositivo específico com base no ID informado.")]
        [SwaggerResponse(200, "Dispositivo encontrado", typeof(Dispositivo))]
        [SwaggerResponse(404, "Dispositivo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Dispositivo>> GetDispositivo(int id)
        {
            var dispositivo = await _dispositivoRepository.GetByIdAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }
            return Ok(dispositivo);
        }
        #endregion

        #region PostDispositivo
        /// <summary>
        /// Adiciona um novo dispositivo.
        /// </summary>
        /// <param name="dispositivo">Dados do dispositivo a ser adicionado.</param>
        /// <returns>Dispositivo criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo dispositivo", Description = "Cria um novo registro de dispositivo.")]
        [SwaggerResponse(201, "Dispositivo criado com sucesso", typeof(Dispositivo))]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Dispositivo>> PostDispositivo(Dispositivo dispositivo)
        {
            await _dispositivoRepository.AddAsync(dispositivo);
            return CreatedAtAction(nameof(GetDispositivo), new { id = dispositivo.Id }, dispositivo);
        }
        #endregion

        #region PutDispositivo
        /// <summary>
        /// Atualiza um dispositivo existente.
        /// </summary>
        /// <param name="id">ID do dispositivo a ser atualizado.</param>
        /// <param name="dispositivo">Dados atualizados do dispositivo.</param>
        /// <returns>Resultado da atualização.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um dispositivo", Description = "Atualiza os dados de um dispositivo existente com base no ID informado.")]
        [SwaggerResponse(204, "Dispositivo atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID não corresponde ao dispositivo")]
        [SwaggerResponse(404, "Dispositivo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> PutDispositivo(int id, Dispositivo dispositivo)
        {
            if (id != dispositivo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _dispositivoRepository.UpdateAsync(dispositivo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dispositivoRepository.GetByIdAsync(id) == null)
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

        #region DeleteDispositivo
        /// <summary>
        /// Exclui um dispositivo pelo ID.
        /// </summary>
        /// <param name="id">ID do dispositivo a ser excluído.</param>
        /// <returns>Resultado da exclusão.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um dispositivo", Description = "Remove um registro de dispositivo com base no ID informado.")]
        [SwaggerResponse(204, "Dispositivo excluído com sucesso")]
        [SwaggerResponse(404, "Dispositivo não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteDispositivo(int id)
        {
            var dispositivo = await _dispositivoRepository.GetByIdAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            await _dispositivoRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}