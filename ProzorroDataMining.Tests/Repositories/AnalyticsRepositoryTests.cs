using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;
using ProzorroDataMining.Data.Repository;
using ProzorroDataMining.Domain.Results;
using System.Data.Common;

namespace ProzorroDataMining.Tests.Repositories
{
    public class AnalyticsRepositoryTests
    {
        private readonly Mock<ILogger<AnalyticsRepository>> _loggerMock;

        #region Ctor

        public AnalyticsRepositoryTests()
        {
            _loggerMock = new Mock<ILogger<AnalyticsRepository>>();
        }

        #endregion

        #region Tests

        [Fact]
        public async Task GetTotalSavingsAsync_ShouldReturnSavings()
        {
            // Arrange
            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.ExecuteScalarAsync<decimal>(
                It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(1234m);

            var repo = new AnalyticsRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            var result = await repo.GetTotalSavingsAsync();

            // Assert
            Assert.Equal(1234m, result.Savings);
        }

        [Fact]
        public async Task GetTopProcurersAsync_ShouldReturnResults()
        {
            // Arrange
            var expected = new List<AnalyticsResult>
            {
                new AnalyticsResult { ProcuringEntity = "Entity1", SupplierName = "SupplierA", TotalAmount = 5000m, Savings = 500m }
            };

            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.QueryAsync<AnalyticsResult>(
                It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(expected);

            var repo = new AnalyticsRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            var results = await repo.GetTopProcurersAsync();

            // Assert
            Assert.Single(results);
            Assert.Equal("Entity1", ((List<AnalyticsResult>)results)[0].ProcuringEntity);
        }

        [Fact]
        public async Task GetTopSuppliersAsync_ShouldReturnResults()
        {
            // Arrange
            var expected = new List<AnalyticsResult>
            {
                new AnalyticsResult { ProcuringEntity = "EntityX", SupplierName = "Supplier1", TotalAmount = 3000m, Savings = 300m }
            };

            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.QueryAsync<AnalyticsResult>(
                It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(expected);

            var repo = new AnalyticsRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            var results = await repo.GetTopSuppliersAsync();

            // Assert
            Assert.Single(results);
            Assert.Equal("Supplier1", ((List<AnalyticsResult>)results)[0].SupplierName);
        }

        #endregion
    }
}
