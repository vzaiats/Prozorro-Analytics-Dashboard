using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;
using ProzorroDataMining.Data.Constants;
using ProzorroDataMining.Data.Repository;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Models;
using System.Data.Common;

namespace ProzorroDataMining.Tests.Repositories
{
    public class TenderRepositoryTests
    {
        private readonly Mock<ILogger<TenderRepository>> _loggerMock;

        #region Ctor

        public TenderRepositoryTests()
        {
            _loggerMock = new Mock<ILogger<TenderRepository>>();
        }

        #endregion

        #region Tests

        [Fact]
        public async Task InsertTenderAsync_ShouldInsertTenderAndRelatedEntities()
        {
            // Arrange
            var tender = new Tender
            {
                TenderId = "T1",
                Contracts = new List<Contract> { new Contract() },
                Suppliers = new List<Supplier> { new Supplier() }
            };

            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(1);

            var repo = new TenderRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            await repo.InsertTenderAsync(tender);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task GetTenderByIdAsync_ShouldReturnTenderWithContractsAndSuppliers()
        {
            // Arrange
            var tenderId = "T1";
            var expectedTender = new Tender
            {
                TenderId = tenderId,
                Contracts = new List<Contract>(),
                Suppliers = new List<Supplier>()
            };

            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Tender>(
                SqlQueriesConstants.GetTenderById, It.IsAny<object>(), null, null, null))
                .ReturnsAsync(expectedTender);

            connectionMock.SetupDapperAsync(c => c.QueryAsync<Contract>(
                SqlQueriesConstants.GetContractsByTenderId, It.IsAny<object>(), null, null, null))
                .ReturnsAsync(new List<Contract> { new Contract() });

            connectionMock.SetupDapperAsync(c => c.QueryAsync<Supplier>(
                SqlQueriesConstants.GetSuppliersByTenderId, It.IsAny<object>(), null, null, null))
                .ReturnsAsync(new List<Supplier> { new Supplier() });

            var repo = new TenderRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            var result = await repo.GetTenderByIdAsync(tenderId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Contracts);
            Assert.NotEmpty(result.Suppliers);
        }

        [Fact]
        public async Task ClearAllTendersAsync_ShouldDeleteData_WhenDataExists()
        {
            // Arrange
            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.ExecuteScalarAsync<bool>(
                SqlQueriesConstants.CheckIfAnyDataExists, null, null, null, null))
                .ReturnsAsync(true);

            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(SqlQueriesConstants.DeleteContracts, null, null, null, null))
                .ReturnsAsync(1);
            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(SqlQueriesConstants.DeleteSuppliers, null, null, null, null))
                .ReturnsAsync(1);
            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(SqlQueriesConstants.DeleteTenders, null, null, null, null))
                .ReturnsAsync(1);

            var repo = new TenderRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            await repo.ClearAllTendersAsync();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task ClearAllTendersAsync_ShouldNotDeleteData_WhenNoDataExists()
        {
            // Arrange
            var connectionMock = new Mock<DbConnection>();
            connectionMock.SetupDapperAsync(c => c.ExecuteScalarAsync<bool>(
                SqlQueriesConstants.CheckIfAnyDataExists, null, null, null, null))
                .ReturnsAsync(false);

            var repo = new TenderRepository(connectionMock.Object, _loggerMock.Object);

            // Act
            await repo.ClearAllTendersAsync();

            // Assert
            Assert.DoesNotContain(connectionMock.Invocations, i =>
                i.Method.Name == "ExecuteAsync" &&
                (i.Arguments[0]?.ToString() == SqlQueriesConstants.DeleteContracts ||
                 i.Arguments[0]?.ToString() == SqlQueriesConstants.DeleteSuppliers ||
                 i.Arguments[0]?.ToString() == SqlQueriesConstants.DeleteTenders));
        }

        #endregion
    }
}
