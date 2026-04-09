using Moq;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Tests.Services
{
    public class AnalyticsServiceTests
    {
        #region Tests

        [Fact]
        public async Task GetTotalSavingsAsync_ShouldReturnResultFromRepository()
        {
            // Arrange
            var repoMock = new Mock<IAnalyticsRepository>();
            var expectedResult = new AnalyticsResult
            {
                ProcuringEntity = "Entity1",
                SupplierName = "SupplierA",
                TotalAmount = 10000m,
                Savings = 1234m
            };

            repoMock
                .Setup(r => r.GetTotalSavingsAsync())
                .ReturnsAsync(expectedResult);

            var service = new AnalyticsService(repoMock.Object);

            // Act
            var result = await service.GetTotalSavingsAsync();

            // Assert
            Assert.Equal(expectedResult, result);
            repoMock.Verify(r => r.GetTotalSavingsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTopProcurersAsync_ShouldReturnResultsFromRepository()
        {
            // Arrange
            var repoMock = new Mock<IAnalyticsRepository>();
            var expectedResults = new List<AnalyticsResult>
            {
                new AnalyticsResult { ProcuringEntity = "Entity1", SupplierName = "SupplierA", TotalAmount = 5000m, Savings = 500m },
                new AnalyticsResult { ProcuringEntity = "Entity2", SupplierName = "SupplierB", TotalAmount = 7000m, Savings = 700m }
            };

            repoMock
                .Setup(r => r.GetTopProcurersAsync())
                .ReturnsAsync(expectedResults);

            var service = new AnalyticsService(repoMock.Object);

            // Act
            var results = await service.GetTopProcurersAsync();

            // Assert
            Assert.Equal(expectedResults, results);
            repoMock.Verify(r => r.GetTopProcurersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTopSuppliersAsync_ShouldReturnResultsFromRepository()
        {
            // Arrange
            var repoMock = new Mock<IAnalyticsRepository>();
            var expectedResults = new List<AnalyticsResult>
            {
                new AnalyticsResult { ProcuringEntity = "Entity1", SupplierName = "Supplier1", TotalAmount = 3000m, Savings = 300m },
                new AnalyticsResult { ProcuringEntity = "Entity2", SupplierName = "Supplier2", TotalAmount = 4000m, Savings = 400m }
            };

            repoMock
                .Setup(r => r.GetTopSuppliersAsync())
                .ReturnsAsync(expectedResults);

            var service = new AnalyticsService(repoMock.Object);

            // Act
            var results = await service.GetTopSuppliersAsync();

            // Assert
            Assert.Equal(expectedResults, results);
            repoMock.Verify(r => r.GetTopSuppliersAsync(), Times.Once);
        }

        #endregion
    }
}
