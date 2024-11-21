using EcosaveAPI.Controllers;
using EcosaveAPI.Models;
using EcosaveAPI.Models.Responses;
using EcosaveAPI.Repositories.Interfaces;
using EcosaveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class UsuariosControllerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IGptService> _gptServiceMock;
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _gptServiceMock = new Mock<IGptService>();
        _controller = new UsuariosController(_usuarioRepositoryMock.Object, _gptServiceMock.Object);
    }

    [Fact]
    public async Task GetUsuarios_ShouldReturnAllUsuarios()
    {
        // Arrange
        var usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nome = "Teste 1" },
            new Usuario { Id = 2, Nome = "Teste 2" }
        };
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(usuarios);

        // Act
        var result = await _controller.GetUsuarios();
        var okResult = result.Result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.IsType<List<Usuario>>(okResult.Value);
        Assert.Equal(usuarios.Count, ((List<Usuario>)okResult.Value).Count);
    }

    [Fact]
    public async Task GetUsuario_ShouldReturnUsuario_WhenIdExists()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Teste" };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(usuario);

        // Act
        var result = await _controller.GetUsuario(1);
        var okResult = result.Result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.IsType<Usuario>(okResult.Value);
        Assert.Equal(usuario, okResult.Value);
    }

    [Fact]
    public async Task GetUsuario_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Usuario)null);

        // Act
        var result = await _controller.GetUsuario(1);
        var notFoundResult = result.Result as NotFoundObjectResult;

        // Assert
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task PostUsuario_ShouldCreateUsuario()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Novo Usuario" };
        _usuarioRepositoryMock.Setup(repo => repo.AddAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PostUsuario(usuario);
        var createdResult = result.Result as CreatedAtActionResult;

        // Assert
        Assert.NotNull(createdResult);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(usuario, createdResult.Value);
    }

    [Fact]
    public async Task PutUsuario_ShouldUpdateUsuario_WhenIdMatches()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Usuario Atualizado" };
        _usuarioRepositoryMock.Setup(repo => repo.UpdateAsync(usuario)).Returns(Task.CompletedTask);
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(usuario);

        // Act
        var result = await _controller.PutUsuario(1, usuario);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUsuario_ShouldRemoveUsuario_WhenIdExists()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Usuario" };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(usuario);
        _usuarioRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUsuario(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CalcularMediaAvaliacao_ShouldReturnHighConsumptionMessage_WhenMediaExceedsThreshold()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Teste" };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(usuario);
        _usuarioRepositoryMock.Setup(repo => repo.ObterGastoMedioAsync(1)).ReturnsAsync(2300);
        _gptServiceMock.Setup(service => service.ObterDicaReducaoConsumoAsync()).ReturnsAsync("Reduza o uso de energia.");

        // Act
        var result = await _controller.CalcularMediaAvaliacao(1);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as AvaliacaoConsumoResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal("Alto", response.Avaliacao);
        Assert.Equal("Reduza o uso de energia.", response.Dica);
    }

    [Fact]
    public async Task CalcularMediaAvaliacao_ShouldReturnNormalConsumptionMessage_WhenMediaIsWithinThreshold()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nome = "Teste" };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(usuario);
        _usuarioRepositoryMock.Setup(repo => repo.ObterGastoMedioAsync(1)).ReturnsAsync(2200);

        // Act
        var result = await _controller.CalcularMediaAvaliacao(1);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as AvaliacaoConsumoResponse;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal("Normal", response.Avaliacao);
        Assert.Equal("Continue monitorando seu consumo para manter-se eficiente.", response.Dica);
    }
}
