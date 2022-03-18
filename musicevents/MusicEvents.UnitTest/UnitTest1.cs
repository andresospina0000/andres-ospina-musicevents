using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MusicEvents.DataAccess.Repositories;
using MusicEvents.Services;
using MusicEvents.Services.Implementations;
using MusicEvents.Services.Interfaces;
using Xunit;

namespace MusicEvents.UnitTest;

public class UnitTest1 : DbContextUnitTest
{
    [Fact]
    public void SumaTest()
    {
        // Arrange
        int a = 6;
        int b = 7;

        // Act
        var suma = a + b;
        var expected = 11;

        // Assert
        Assert.NotEqual(expected, suma);
    }

    [Fact]
    public void TestPaginacion()
    {
        // Arrange

        var total = 29;
        var rows = 10;

        // Act
        var resultado = Utils.GetTotalPages(total, rows);
        var expected = 3;

        // Assert
        Assert.Equal(expected, resultado);
    }

    [Theory]
    [InlineData(29, 10, 3)]
    [InlineData(110, 10, 11)]
    [InlineData(200, 5, 40)]
    public void TestPaginacionConParametros(int total, int rows, int expected)
    {
        var resultado = Utils.GetTotalPages(total, rows);

        Assert.Equal(expected, resultado);
    }

    [Theory]
    [InlineData("", 10, 10)]
    [InlineData("Concierto", 4, 0)]
    [InlineData("Event", 4, 25)]
    public async Task TestPaginacionEvents(string filter, int rows, int expected)
    {
        // Arrange

        var mapper = new Mock<IMapper>();
        var repository = new EventRepository(Context, mapper.Object);
        var logger = new Mock<ILogger<EventService>>();
        var fileUploader = new Mock<IFileUploader>();

        var service = new EventService(repository, fileUploader.Object, logger.Object);

        // Act.
        var actual = await service.GetAsync(filter, 1, rows);

        // Assert
        Assert.Equal(expected, actual.TotalPages);
    }
}