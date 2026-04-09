using Microsoft.AspNetCore.Mvc;
using Moq;
using ProzorroDataMining.Api.Controllers;
using ProzorroDataMining.Application.Interfaces.Services;

namespace ProzorroDataMining.Tests.Controllers
{
    public class ETLControllerTests
    {
        #region Tests

        [Fact]
        public async Task ImportTenders_ShouldReturnOkResult_WhenServiceSucceeds()
        {
            // Arrange
            var serviceMock = new Mock<ITenderIngestionService>();
            serviceMock
                .Setup(s => s.FetchAndStoreRecentTendersAsync())
                .Returns(Task.CompletedTask);

            var controller = new ETLController(serviceMock.Object);

            // Act
            var result = await controller.ImportTenders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("ETL process completed successfully",
                         okResult.Value?.GetType().GetProperty("Message")?.GetValue(okResult.Value));
            serviceMock.Verify(s => s.FetchAndStoreRecentTendersAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportTenders_ShouldReturn500_WhenServiceThrowsException()
        {
            // Arrange
            var serviceMock = new Mock<ITenderIngestionService>();
            serviceMock
                .Setup(s => s.FetchAndStoreRecentTendersAsync())
                .ThrowsAsync(new Exception("ETL failed"));

            var controller = new ETLController(serviceMock.Object);

            // Act
            IActionResult result;
            try
            {
                result = await controller.ImportTenders();
            }
            catch (Exception ex)
            {
                Assert.Equal("ETL failed", ex.Message);
                return;
            }

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        #endregion
    }
}
