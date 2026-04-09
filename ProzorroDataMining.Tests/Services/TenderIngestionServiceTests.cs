using Microsoft.Extensions.Logging;
using Moq;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Models;
using System.Net;

namespace ProzorroDataMining.Tests.Services
{
    public class TenderIngestionServiceTests
    {
        #region Tests

        [Fact]
        public async Task FetchAndStoreRecentTendersAsync_ShouldInsertTender_UsingFakeHandler()
        {
            // Arrange
            var repoMock = new Mock<ITenderRepository>();
            var loggerMock = new Mock<ILogger<TenderIngestionService>>();

            bool insertCalled = false;

            repoMock
                .Setup(r => r.InsertTenderAsync(It.IsAny<Tender>()))
                .Callback<Tender>(t =>
                {
                    insertCalled = true;
                    Assert.Equal("TENDER-123", t.TenderId);
                })
                .Returns(Task.CompletedTask);

            var httpClient = new HttpClient(new FakeHandler())
            {
                BaseAddress = new Uri("https://public-api.prozorro.gov.ua/api/2.5/")
            };

            var service = new TenderIngestionService(httpClient, repoMock.Object, loggerMock.Object);

            // Act
            await service.FetchAndStoreRecentTendersAsync();

            // Assert
            Assert.True(insertCalled, "InsertTenderAsync was not called");
        }
    }

    #endregion

    #region Handler

    public class FakeHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri!.AbsolutePath;
            var query = request.RequestUri!.Query;

            if (path.EndsWith("/tenders") && query.Contains("descending=1"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{ ""data"": [ { ""id"": ""TENDER-123"" } ] }")
                });
            }

            if (path.Contains("/tenders/9301f7a4c50c4629a8fb19cd9f2134b8") || path.Contains("/tenders/TENDER-123"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{ ""data"": { ""id"": ""TENDER-123"" } }")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }

    #endregion
}
