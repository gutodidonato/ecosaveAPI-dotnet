using Moq;
using Xunit;
using EcosaveAPI.Controllers;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcosaveAPI.Tests.Controllers
{
    public class PontosControllerTests
    {
        private readonly Mock<IPontoRepository> _pontoRepositoryMock;
        private readonly PontosController _controller;

        public PontosControllerTests()
        {
            // Inicializando o mock do repositório e o controlador
            _pontoRepositoryMock = new Mock<IPontoRepository>();
            _controller = new PontosController(_pontoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPontos_ReturnsOkResult_WithListOfPontos()
        {
            // Arrange
            var pontos = new List<Ponto>
            {
                new Ponto { Id = 1, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" },
                new Ponto { Id = 2, IdUsuario = 2, ValorPonto = 20.5m, Descricao = "Ponto 2" }
            };

            _pontoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(pontos);

            // Act
            var result = await _controller.GetPontos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Ponto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count); // Verifica se retornou 2 pontos
        }

        [Fact]
        public async Task GetPonto_ReturnsNotFound_WhenPontoDoesNotExist()
        {
            // Arrange
            var idInexistente = 999;
            _pontoRepositoryMock.Setup(repo => repo.GetByIdAsync(idInexistente)).ReturnsAsync((Ponto)null);

            // Act
            var result = await _controller.GetPonto(idInexistente);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result); // Espera um NotFound
        }

        [Fact]
        public async Task GetPonto_ReturnsOkResult_WhenPontoExists()
        {
            // Arrange
            var ponto = new Ponto { Id = 1, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" };
            _pontoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ponto);

            // Act
            var result = await _controller.GetPonto(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Ponto>(okResult.Value);
            Assert.Equal(1, returnValue.Id); // Verifica se o ponto retornado tem o Id correto
        }

        [Fact]
        public async Task PostPonto_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var ponto = new Ponto { Id = 1, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" };
            _pontoRepositoryMock.Setup(repo => repo.AddAsync(ponto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostPonto(ponto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Ponto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id); // Verifica se o Id do ponto está correto
        }

        [Fact]
        public async Task PutPonto_ReturnsNoContent_WhenPontoIsUpdated()
        {
            // Arrange
            var ponto = new Ponto { Id = 1, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" };
            _pontoRepositoryMock.Setup(repo => repo.UpdateAsync(ponto)).Returns(Task.CompletedTask);
            _pontoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ponto);

            // Act
            var result = await _controller.PutPonto(1, ponto);

            // Assert
            Assert.IsType<NoContentResult>(result); // Espera NoContent (204)
        }

        [Fact]
        public async Task PutPonto_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var ponto = new Ponto { Id = 1, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" };

            // Act
            var result = await _controller.PutPonto(999, ponto);

            // Assert
            Assert.IsType<BadRequestResult>(result); // Espera BadRequest (400)
        }

        [Fact]
        public async Task DeletePonto_ReturnsNoContent_WhenPontoIsDeleted()
        {
            // Arrange
            var id = 1;
            var ponto = new Ponto { Id = id, IdUsuario = 1, ValorPonto = 10.5m, Descricao = "Ponto 1" };
            _pontoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(ponto);
            _pontoRepositoryMock.Setup(repo => repo.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePonto(id);

            // Assert
            Assert.IsType<NoContentResult>(result); // Espera NoContent (204)
        }

        [Fact]
        public async Task DeletePonto_ReturnsNotFound_WhenPontoDoesNotExist()
        {
            // Arrange
            var idInexistente = 999;
            _pontoRepositoryMock.Setup(repo => repo.GetByIdAsync(idInexistente)).ReturnsAsync((Ponto)null);

            // Act
            var result = await _controller.DeletePonto(idInexistente);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Espera NotFound (404)
        }
    }
}
