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
    public class ConsumosControllerTests
    {
        [Fact]
        public async Task GetConsumos_ReturnsOkResult_WithConsumos()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Consumo>
                {
                    new Consumo { Id = 1, IdDispositivo = 1, ConsumoKWh = 150.5m, Custo = 75.25m },
                    new Consumo { Id = 2, IdDispositivo = 2, ConsumoKWh = 200.0m, Custo = 100.00m }
                });

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.GetConsumos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var consumos = Assert.IsAssignableFrom<IEnumerable<Consumo>>(okResult.Value);
            Assert.Equal(2, consumos.Count());
        }

        [Fact]
        public async Task GetConsumo_ReturnsNotFound_WhenConsumoDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Consumo)null); // Simulando que o consumo não foi encontrado

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.GetConsumo(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetConsumo_ReturnsOkResult_WithConsumo()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            var consumo = new Consumo { Id = 1, IdDispositivo = 1, ConsumoKWh = 150.5m, Custo = 75.25m };
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(consumo);

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.GetConsumo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedConsumo = Assert.IsType<Consumo>(okResult.Value);
            Assert.Equal(consumo.Id, returnedConsumo.Id);
            Assert.Equal(consumo.ConsumoKWh, returnedConsumo.ConsumoKWh);
            Assert.Equal(consumo.Custo, returnedConsumo.Custo);
        }

        [Fact]
        public async Task PostConsumo_ReturnsCreatedAtActionResult_WithConsumo()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            var consumo = new Consumo { Id = 1, IdDispositivo = 1, ConsumoKWh = 150.5m, Custo = 75.25m };
            mockRepository.Setup(repo => repo.AddAsync(consumo))
                .Returns(Task.CompletedTask); // Simulando que o consumo foi adicionado com sucesso

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.PostConsumo(consumo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedConsumo = Assert.IsType<Consumo>(createdAtActionResult.Value);
            Assert.Equal(consumo.Id, returnedConsumo.Id);
        }

        [Fact]
        public async Task PutConsumo_ReturnsNoContent_WhenConsumoIsUpdated()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            var consumo = new Consumo { Id = 1, IdDispositivo = 1, ConsumoKWh = 150.5m, Custo = 75.25m };
            mockRepository.Setup(repo => repo.UpdateAsync(consumo))
                .Returns(Task.CompletedTask); // Simulando que o consumo foi atualizado com sucesso

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.PutConsumo(1, consumo);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteConsumo_ReturnsNoContent_WhenConsumoIsDeleted()
        {
            // Arrange
            var mockRepository = new Mock<IConsumoRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Consumo { Id = 1, IdDispositivo = 1, ConsumoKWh = 150.5m, Custo = 75.25m });
            mockRepository.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask); // Simulando que o consumo foi deletado com sucesso

            var controller = new ConsumosController(mockRepository.Object);

            // Act
            var result = await controller.DeleteConsumo(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
