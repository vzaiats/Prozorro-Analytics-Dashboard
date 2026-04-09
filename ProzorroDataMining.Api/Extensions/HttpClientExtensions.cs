using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using ProzorroDataMining.Api.Configurations;
using ProzorroDataMining.Application.Interfaces.Services;
using ProzorroDataMining.Application.Services;

namespace ProzorroDataMining.Api.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddProzorroHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<ITenderIngestionService, TenderIngestionService>((sp, client) =>
        {
            // Base address for Prozorro API
            var options = sp.GetRequiredService<IOptions<ProzorroApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddResilienceHandler("prozorropipeline", builder =>
            {
                // Retry policy: retry up to 3 times with exponential backoff
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential
                });

                // Timeout policy: each request must complete within 30 seconds
                builder.AddTimeout(TimeSpan.FromSeconds(30));

                // Circuit breaker policy: break the circuit if 50% of requests fail
                builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
                {
                    FailureRatio = 0.5,          // 50% failure rate threshold
                    MinimumThroughput = 10,      // minimum number of requests before evaluation
                    BreakDuration = TimeSpan.FromSeconds(30) // circuit stays open for 30 seconds
                });
            });

        return services;
    }
}
