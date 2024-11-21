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
    public class ComodosController : ControllerBase
    {
        private readonly IComodoRepository _comodoRepository;

        public ComodosController(IComodoRepository comodoRepository)
        {
            _comodoRepository = comodoRepository;
        }

        #region GetComodos
        /// <summary>
        /// Recupera todos os cômodos.
        /// </summary>
        /// <returns>Lista de cômodos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os cômodos", Description = "Recupera todos os cômodos registrados no sistema.")]
        [SwaggerResponse(200, "Retorna a lista de cômodos.", typeof(IEnumerable<Comodo>))]
        public async Task<ActionResult<IEnumerable<Comodo>>> GetComodos()
        {
            var comodos = await _comodoRepository.GetAllAsync();
            return Ok(comodos);
        }
        #endregion

        #region GetComodoById
        /// <summary>
        /// Recupera um cômodo específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cômodo.</param>
        /// <returns>O cômodo solicitado.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter um cômodo pelo ID", Description = "Recupera os detalhes de um cômodo utilizando o seu ID.")]
        [SwaggerResponse(200, "Retorna os detalhes do cômodo.", typeof(Comodo))]
        [SwaggerResponse(404, "Cômodo não encontrado.")]
        public async Task<ActionResult<Comodo>> GetComodo(int id)
        {
            var comodo = await _comodoRepository.GetByIdAsync(id);
            if (comodo == null)
            {
                return NotFound();
            }
            return Ok(comodo);
        }
        #endregion

        #region PostComodo
        /// <summary>
        /// Adiciona um novo cômodo.
        /// </summary>
        /// <param name="comodo">Detalhes do cômodo.</param>
        /// <returns>O cômodo criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adicionar um novo cômodo", Description = "Registra um novo cômodo no sistema.")]
        [SwaggerResponse(201, "Cômodo criado com sucesso.", typeof(Comodo))]
        public async Task<ActionResult<Comodo>> PostComodo(Comodo comodo)
        {
            await _comodoRepository.AddAsync(comodo);
            return CreatedAtAction(nameof(GetComodo), new { id = comodo.Id }, comodo);
        }
        #endregion

        #region PutComodo
        /// <summary>
        /// Atualiza um cômodo existente.
        /// </summary>
        /// <param name="id">ID do cômodo.</param>
        /// <param name="comodo">Detalhes atualizados do cômodo.</param>
        /// <returns>Sem conteúdo.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar um cômodo", Description = "Atualiza os detalhes de um cômodo existente.")]
        [SwaggerResponse(204, "Cômodo atualizado com sucesso.")]
        [SwaggerResponse(400, "Requisição inválida (ID não corresponde).")]
        [SwaggerResponse(404, "Cômodo não encontrado.")]
        public async Task<IActionResult> PutComodo(int id, Comodo comodo)
        {
            if (id != comodo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _comodoRepository.UpdateAsync(comodo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _comodoRepository.GetByIdAsync(id) == null)
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

        #region DeleteComodo
        /// <summary>
        /// Deleta um cômodo pelo ID.
        /// </summary>
        /// <param name="id">ID do cômodo.</param>
        /// <returns>Sem conteúdo.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletar um cômodo", Description = "Remove um cômodo do sistema utilizando seu ID.")]
        [SwaggerResponse(204, "Cômodo deletado com sucesso.")]
        [SwaggerResponse(404, "Cômodo não encontrado.")]
        public async Task<IActionResult> DeleteComodo(int id)
        {
            var comodo = await _comodoRepository.GetByIdAsync(id);
            if (comodo == null)
            {
                return NotFound();
            }

            await _comodoRepository.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
