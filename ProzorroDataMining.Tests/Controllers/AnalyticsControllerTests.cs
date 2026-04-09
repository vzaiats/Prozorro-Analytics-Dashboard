using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProzorroDataMining.Api.Controllers;
using ProzorroDataMining.Application.DTOs;
using ProzorroDataMining.Application.Interfaces.Services;
using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Tests.Controllers
{
    public class AnalyticsControllerTests
    {
        #region Tests

        [Fact]
        public async Task GetTotalSavings_ShouldReturnOkResult_WithMappedDto()
        {
            // Arrange
            var serviceMock = new Mock<IAnalyticsService>();
            var mapperMock = new Mock<IMapper>();

            var savingsResult = new AnalyticsResult { Savings = 1234m };
            var savingsDto = new SavingsDto { TotalSavings = 1234m };

            serviceMock.Setup(s => s.GetTotalSavingsAsync())
                       .ReturnsAsync(savingsResult);

            mapperMock.Setup(m => m.Map<SavingsDto>(savingsResult))
                      .Returns(savingsDto);

            var controller = new AnalyticsController(serviceMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetTotalSavings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<SavingsDto>(okResult.Value);
            Assert.Equal(1234m, dto.TotalSavings);
            serviceMock.Verify(s => s.GetTotalSavingsAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<SavingsDto>(savingsResult), Times.Once);
        }

        [Fact]
        public async Task GetTopProcurers_ShouldReturnOkResult_WithMappedDtos()
        {
            // Arrange
            var serviceMock = new Mock<IAnalyticsService>();
            var mapperMock = new Mock<IMapper>();

            var procurersResult = new List<AnalyticsResult>
            {
                new AnalyticsResult { ProcuringEntity = "Entity1", TotalAmount = 5000m }
            };
            var procurerDtos = new List<ProcurerStatsDto>
            {
                new ProcurerStatsDto { ProcuringEntity = "Entity1", TotalAmount = 5000m }
            };

            serviceMock.Setup(s => s.GetTopProcurersAsync())
                       .ReturnsAsync(procurersResult);

            mapperMock.Setup(m => m.Map<IEnumerable<ProcurerStatsDto>>(procurersResult))
                      .Returns(procurerDtos);

            var controller = new AnalyticsController(serviceMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetTopProcurers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ProcurerStatsDto>>(okResult.Value);
            Assert.Single(dtos);
            Assert.Equal("Entity1", dtos.First().ProcuringEntity);
        }

        [Fact]
        public async Task GetTopSuppliers_ShouldReturnOkResult_WithMappedDtos()
        {
            // Arrange
            var serviceMock = new Mock<IAnalyticsService>();
            var mapperMock = new Mock<IMapper>();

            var suppliersResult = new List<AnalyticsResult>
            {
                new AnalyticsResult { SupplierName = "Supplier1", TotalAmount = 3000m }
            };
            var supplierDtos = new List<SupplierStatsDto>
            {
                new SupplierStatsDto { SupplierName = "Supplier1", TotalAmount = 3000m }
            };

            serviceMock.Setup(s => s.GetTopSuppliersAsync())
                       .ReturnsAsync(suppliersResult);

            mapperMock.Setup(m => m.Map<IEnumerable<SupplierStatsDto>>(suppliersResult))
                      .Returns(supplierDtos);

            var controller = new AnalyticsController(serviceMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetTopSuppliers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<SupplierStatsDto>>(okResult.Value);
            Assert.Single(dtos);
            Assert.Equal("Supplier1", dtos.First().SupplierName);
        }

        #endregion
    }
}
