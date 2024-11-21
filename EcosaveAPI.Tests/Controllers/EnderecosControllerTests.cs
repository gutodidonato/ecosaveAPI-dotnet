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
    public class EnderecosControllerTests
    {
        private readonly Mock<IEnderecoRepository> _mockEnderecoRepository;
        private readonly EnderecosController _controller;

        public EnderecosControllerTests()
        {
            _mockEnderecoRepository = new Mock<IEnderecoRepository>();
            _controller = new EnderecosController(_mockEnderecoRepository.Object);
        }

        [Fact]
        public async Task GetEnderecos_ReturnsOkResult_WithListOfEnderecos()
        {
            // Arrange
            var enderecos = new List<Endereco>
            {
                new Endereco { Id = 1, CEP = "12345-678", Numero = 100, Complemento = "Apto 101" },
                new Endereco { Id = 2, CEP = "98765-432", Numero = 200, Complemento = "Casa" }
            };

            _mockEnderecoRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enderecos);

            // Act
            var result = await _controller.GetEnderecos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Endereco>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedEnderecos = Assert.IsType<List<Endereco>>(okResult.Value);
            Assert.Equal(2, returnedEnderecos.Count);
        }

        [Fact]
        public async Task GetEndereco_ReturnsNotFound_WhenEnderecoDoesNotExist()
        {
            // Arrange
            _mockEnderecoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Endereco)null);

            // Act
            var result = await _controller.GetEndereco(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Endereco>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetEndereco_ReturnsOkResult_WhenEnderecoExists()
        {
            // Arrange
            var endereco = new Endereco { Id = 1, CEP = "12345-678", Numero = 100, Complemento = "Apto 101" };
            _mockEnderecoRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(endereco);

            // Act
            var result = await _controller.GetEndereco(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Endereco>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedEndereco = Assert.IsType<Endereco>(okResult.Value);
            Assert.Equal(1, returnedEndereco.Id);
        }

        [Fact]
        public async Task PostEndereco_ReturnsCreatedAtAction_WhenEnderecoIsCreated()
        {
            // Arrange
            var newEndereco = new Endereco { IdUsuario = 1, CEP = "12345-678", Numero = 100, Complemento = "Apto 101" };
            _mockEnderecoRepository.Setup(repo => repo.AddAsync(newEndereco)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostEndereco(newEndereco);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Endereco>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetEndereco", createdAtActionResult.ActionName);
            Assert.Equal(newEndereco, createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutEndereco_ReturnsNoContent_WhenEnderecoIsUpdated()
        {
            // Arrange
            var updatedEndereco = new Endereco { Id = 1, IdUsuario = 1, CEP = "98765-432", Numero = 200, Complemento = "Casa" };
            _mockEnderecoRepository.Setup(repo => repo.UpdateAsync(updatedEndereco)).Returns(Task.CompletedTask);
            _mockEnderecoRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(updatedEndereco);

            // Act
            var result = await _controller.PutEndereco(1, updatedEndereco);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEndereco_ReturnsNoContent_WhenEnderecoIsDeleted()
        {
            // Arrange
            var enderecoToDelete = new Endereco { Id = 1, CEP = "12345-678", Numero = 100, Complemento = "Apto 101" };
            _mockEnderecoRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(enderecoToDelete);
            _mockEnderecoRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEndereco(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

[Fact]
public async Task DeleteEndereco_ReturnsNotFound_WhenEnderecoDoesNotExist()
{
    // Arrange
    var idInexistente = 999;  
    _mockEnderecoRepository
        .Setup(repo => repo.GetByIdAsync(idInexistente))
        .ReturnsAsync((Endereco)null); 

    var controller = new EnderecosController(_mockEnderecoRepository.Object);

    // Act
    var result = await controller.DeleteEndereco(idInexistente);

    // Assert
    Assert.IsType<NotFoundResult>(result);  
}
    }
}
