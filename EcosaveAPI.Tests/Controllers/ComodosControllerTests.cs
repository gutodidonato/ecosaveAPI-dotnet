using EcosaveAPI.Controllers;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EcosaveAPI.Tests
{
    public class ComodosControllerTests
    {
        [Fact]
        public async Task GetComodos_ReturnsOkResult_WithListOfComodos()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodos = new List<Comodo>
            {
                new Comodo { Id = 1, Nome = "Cômodo 1" },
                new Comodo { Id = 2, Nome = "Cômodo 2" }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(comodos);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.GetComodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);  // Verificando o tipo de resposta
            var returnedComodos = Assert.IsType<List<Comodo>>(okResult.Value);
            Assert.Equal(2, returnedComodos.Count);
        }

        [Fact]
        public async Task GetComodo_ExistingId_ReturnsOkResult_WithComodo()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodo = new Comodo { Id = 1, Nome = "Cômodo Teste" };

            mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(comodo);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.GetComodo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);  // Verificando o tipo de resposta
            var returnedComodo = Assert.IsType<Comodo>(okResult.Value);
            Assert.Equal(comodo.Id, returnedComodo.Id);
            Assert.Equal(comodo.Nome, returnedComodo.Nome);
        }

        [Fact]
        public async Task GetComodo_NotExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Comodo)null);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.GetComodo(999);  // Non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);  // Verificando o tipo de resposta
        }

        [Fact]
        public async Task PostComodo_ValidComodo_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodo = new Comodo { Id = 1, Nome = "Cômodo Novo" };

            mockRepo.Setup(repo => repo.AddAsync(comodo)).Returns(Task.CompletedTask);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.PostComodo(comodo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);  // Verificando o tipo de resposta
            var returnedComodo = Assert.IsType<Comodo>(createdAtActionResult.Value);
            Assert.Equal(comodo.Id, returnedComodo.Id);
            Assert.Equal(comodo.Nome, returnedComodo.Nome);
        }

        [Fact]
        public async Task PutComodo_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodo = new Comodo { Id = 1, Nome = "Cômodo Atualizado" };

            mockRepo.Setup(repo => repo.UpdateAsync(comodo)).Returns(Task.CompletedTask);
            mockRepo.Setup(repo => repo.GetByIdAsync(comodo.Id)).ReturnsAsync(comodo);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.PutComodo(1, comodo);

            // Assert
            Assert.IsType<NoContentResult>(result);  // Verificando o tipo de resposta
        }

        [Fact]
        public async Task PutComodo_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodo = new Comodo { Id = 1, Nome = "Cômodo Atualizado" };

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.PutComodo(999, comodo);  // Invalid ID

            // Assert
            Assert.IsType<BadRequestResult>(result);  // Verificando o tipo de resposta
        }

        [Fact]
        public async Task DeleteComodo_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            var comodo = new Comodo { Id = 1, Nome = "Cômodo Deletado" };

            mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(comodo);
            mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComodo(1);

            // Assert
            Assert.IsType<NoContentResult>(result);  // Verificando o tipo de resposta
        }

        [Fact]
        public async Task DeleteComodo_NotExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockRepo = new Mock<IComodoRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Comodo)null);

            var controller = new ComodosController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComodo(999);  // Non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result);  // Verificando o tipo de resposta
        }
    }
}