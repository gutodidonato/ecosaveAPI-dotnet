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
    public class EnderecosController : ControllerBase
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecosController(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        #region GetEnderecos
        /// <summary>
        /// Obtém todos os endereços.
        /// </summary>
        /// <returns>Lista de endereços.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os endereços", Description = "Retorna uma lista de todos os endereços cadastrados.")]
        [SwaggerResponse(200, "Lista de endereços retornada com sucesso", typeof(IEnumerable<Endereco>))]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<IEnumerable<Endereco>>> GetEnderecos()
        {
            var enderecos = await _enderecoRepository.GetAllAsync();
            return Ok(enderecos);
        }
        #endregion

        #region GetEndereco
        /// <summary>
        /// Obtém um endereço pelo ID.
        /// </summary>
        /// <param name="id">ID do endereço.</param>
        /// <returns>Endereço correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um endereço pelo ID", Description = "Retorna um endereço específico com base no ID informado.")]
        [SwaggerResponse(200, "Endereço encontrado", typeof(Endereco))]
        [SwaggerResponse(404, "Endereço não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Endereco>> GetEndereco(int id)
        {
            var endereco = await _enderecoRepository.GetByIdAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            return Ok(endereco);
        }
        #endregion

        #region PostEndereco
        /// <summary>
        /// Adiciona um novo endereço.
        /// </summary>
        /// <param name="endereco">Dados do endereço a ser adicionado.</param>
        /// <returns>Endereço criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo endereço", Description = "Cria um novo registro de endereço.")]
        [SwaggerResponse(201, "Endereço criado com sucesso", typeof(Endereco))]
        [SwaggerResponse(400, "Dados inválidos")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<Endereco>> PostEndereco(Endereco endereco)
        {
            await _enderecoRepository.AddAsync(endereco);
            return CreatedAtAction(nameof(GetEndereco), new { id = endereco.Id }, endereco);
        }
        #endregion

        #region PutEndereco
        /// <summary>
        /// Atualiza um endereço existente.
        /// </summary>
        /// <param name="id">ID do endereço a ser atualizado.</param>
        /// <param name="endereco">Dados atualizados do endereço.</param>
        /// <returns>Resultado da atualização.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um endereço", Description = "Atualiza os dados de um endereço existente com base no ID informado.")]
        [SwaggerResponse(204, "Endereço atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID não corresponde ao endereço")]
        [SwaggerResponse(404, "Endereço não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> PutEndereco(int id, Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return BadRequest();
            }

            try
            {
                await _enderecoRepository.UpdateAsync(endereco);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _enderecoRepository.GetByIdAsync(id) == null)
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

        #region DeleteEndereco
        /// <summary>
        /// Exclui um endereço pelo ID.
        /// </summary>
        /// <param name="id">ID do endereço a ser excluído.</param>
        /// <returns>Resultado da exclusão.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um endereço", Description = "Remove um registro de endereço com base no ID informado.")]
        [SwaggerResponse(204, "Endereço excluído com sucesso")]
        [SwaggerResponse(404, "Endereço não encontrado")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            var endereco = await _enderecoRepository.GetByIdAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }

            await _enderecoRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}