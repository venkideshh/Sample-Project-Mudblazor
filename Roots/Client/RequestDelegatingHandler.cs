using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace EDC.Client;

public class RequestDelegatingHandler(NavigationManager navManager, ILocalStorageService localStore) : DelegatingHandler
{
    private readonly NavigationManager navManager = navManager;
    private readonly ILocalStorageService localStore = localStore;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await localStore.GetItemAsStringAsync("token", cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navManager.NavigateTo("Login");
        }
        return response;
    }
}