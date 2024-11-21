using EcosaveAPI.Controllers;
using EcosaveAPI.Models;
using EcosaveAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EcosaveAPI.Tests.Controllers
{
    public class DispositivosControllerTests
    {
        [Fact]
        public async Task GetDispositivos_ReturnsOkResult_WithDispositivos()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Dispositivo>
                {
                    new Dispositivo { Id = 1, Nome = "Dispositivo 1", Modelo = "Modelo A", IdUsuario = 1, IdComodo = 1 },
                    new Dispositivo { Id = 2, Nome = "Dispositivo 2", Modelo = "Modelo B", IdUsuario = 2, IdComodo = 2 }
                });

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.GetDispositivos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dispositivos = Assert.IsAssignableFrom<IEnumerable<Dispositivo>>(okResult.Value);
            Assert.Equal(2, dispositivos.Count());
        }

        [Fact]
        public async Task GetDispositivo_ReturnsNotFound_WhenDispositivoDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Dispositivo)null); // Simulando que o dispositivo não foi encontrado

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.GetDispositivo(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetDispositivo_ReturnsOkResult_WithDispositivo()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            var dispositivo = new Dispositivo { Id = 1, Nome = "Dispositivo 1", Modelo = "Modelo A", IdUsuario = 1, IdComodo = 1 };
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(dispositivo);

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.GetDispositivo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDispositivo = Assert.IsType<Dispositivo>(okResult.Value);
            Assert.Equal(dispositivo.Id, returnedDispositivo.Id);
            Assert.Equal(dispositivo.Nome, returnedDispositivo.Nome);
            Assert.Equal(dispositivo.Modelo, returnedDispositivo.Modelo);
        }

        [Fact]
        public async Task PostDispositivo_ReturnsCreatedAtActionResult_WithDispositivo()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            var dispositivo = new Dispositivo { Id = 1, Nome = "Dispositivo 1", Modelo = "Modelo A", IdUsuario = 1, IdComodo = 1 };
            mockRepository.Setup(repo => repo.AddAsync(dispositivo))
                .Returns(Task.CompletedTask); // Simulando que o dispositivo foi adicionado com sucesso

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.PostDispositivo(dispositivo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDispositivo = Assert.IsType<Dispositivo>(createdAtActionResult.Value);
            Assert.Equal(dispositivo.Id, returnedDispositivo.Id);
        }

        [Fact]
        public async Task PutDispositivo_ReturnsNoContent_WhenDispositivoIsUpdated()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            var dispositivo = new Dispositivo { Id = 1, Nome = "Dispositivo 1", Modelo = "Modelo A", IdUsuario = 1, IdComodo = 1 };
            mockRepository.Setup(repo => repo.UpdateAsync(dispositivo))
                .Returns(Task.CompletedTask); // Simulando que o dispositivo foi atualizado com sucesso

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.PutDispositivo(1, dispositivo);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDispositivo_ReturnsNoContent_WhenDispositivoIsDeleted()
        {
            // Arrange
            var mockRepository = new Mock<IDispositivoRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Dispositivo { Id = 1, Nome = "Dispositivo 1", Modelo = "Modelo A", IdUsuario = 1, IdComodo = 1 });
            mockRepository.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask); // Simulando que o dispositivo foi deletado com sucesso

            var controller = new DispositivosController(mockRepository.Object);

            // Act
            var result = await controller.DeleteDispositivo(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
