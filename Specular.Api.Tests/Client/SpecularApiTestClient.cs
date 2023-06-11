using System.Net.Http.Headers;
using System.Text;
using NgSoftware.Specular.Api.Client;

namespace NgSoftware.Specular.Api.Tests.Client;

/// <inheritdoc />
internal class SpecularApiTestClient : SpecularApiClient
{
    private readonly IBearerTokenProvider bearerTokenProvider;

    public SpecularApiTestClient(string baseUrl, HttpClient httpClient, IBearerTokenProvider bearerTokenProvider)
        : base(baseUrl, httpClient)
    {
        this.bearerTokenProvider = bearerTokenProvider;
    }

    protected override Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected override Task PrepareRequestAsync(HttpClient client,
        HttpRequestMessage request,
        StringBuilder urlBuilder,
        CancellationToken cancellationToken)
    {
        if (bearerTokenProvider.HasToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenProvider.Token);
        }
        return Task.CompletedTask;
    }

    protected override Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
