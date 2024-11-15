using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using EDC.Client.Models;
using EDC.Shared.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;

namespace EDC.Client.Components;

public class BaseComponent : ComponentBase
{
    #region Dependencies

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected ISnackbar Snackbar { get; set; }

    [Inject]
    protected ILogger<BaseComponent> Logger { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    [Obsolete("Use HttpClientFactory Property")]
    protected HttpClient HttpClient { get; set; }

    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; }

    [Inject]
    protected ILocalStorageService LocalStore { get; set; }

    #endregion Dependencies
    public enum Roles
    {
        IT,
        HOD,
        Admin,
        Engineer
    }

    #region Properties

    protected bool Processing { get; set; } = false;
    public const string MESSAGES_NOTIFICATION_CENTER = "NOTIFICATION_CENTER";
    public const string MESSAGES_TOP = "TOP";
    public const string MESSAGES_DIALOG = "DIALOG";
    public const string MESSAGES_CARD = "CARD";
    protected string UserType { get; set; } = string.Empty;
    protected bool isHOD { get; set; }
    protected bool isITAdmin { get; set; }

    #endregion Properties

    protected void BeginProcess()
    {
        Processing = true;
    }

    protected void EndProcess()
    {
        Processing = false;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }

    }

    #region Process


    protected void EndProcessAndTriggerStateChange()
    {
        EndProcess();
        StateHasChanged();
    }

    protected void EndProcessIfStarted()
    {
        if (Processing)
        {
            EndProcessAndTriggerStateChange();
        }
    }

    #endregion Process

    #region Errors

    protected void ShowError(string message, string actualError)
    {
        Logger.LogWarning($"EDC - {message}:\r\n{actualError}");
        Snackbar.Add(message, Severity.Error);
    }

    protected void ShowSuccess(string message)
    {
        Snackbar.Add(message, Severity.Success);
    }

    protected void ShowError(string actualError)
    {
        ShowError(actualError, string.Empty);
    }

    protected void ShowError(Exception ex)
    {
        Logger.LogError(ex, "EDC - An unexpected error occurred");
        Snackbar.Add("An unexpected error occurred", Severity.Error);
    }

    protected void ShowApiError(string actualError)
    {
        ShowError("Received Failure Response from Server", actualError);
    }

    protected async Task ShowApiErrorAsync(HttpContent content)
    {
        var error = await content.ReadAsStringAsync().ConfigureAwait(false);
        ShowApiError(error);
    }

    protected async Task ShowApiErrorAsync(HttpResponseMessage response)
    {
        var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            ShowError(error);
        }
        else
        {
            ShowApiError(error);
        }
    }

    #endregion Errors

    #region API Calls

    protected async Task<T[]> LoadItemsAsync<T>(string apiPath) where T : class
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"Api/{apiPath}").ConfigureAwait(false);
            var result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T[]>().ConfigureAwait(false);
            }
            else
            {
                await ShowApiErrorAsync(response).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        return [];
    }

    protected async Task<T> LoadItemAsync<T>(string apiPath) where T : class
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"Api/{apiPath}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
            }
            else
            {
                await ShowApiErrorAsync(response.Content).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        return default;
    }
    protected async Task<T> AddItemAsync<T>(string apiPath, T item) where T : class
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync($"Api/{apiPath}", item).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
            }
            else
            {
                await ShowApiErrorAsync(response.Content).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        return default;
    }

    protected async Task<HttpResponseMessage> AddItemAsync1(string apiPath, MultipartFormDataContent content)
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.PostAsync($"Api/{apiPath}", content).ConfigureAwait(false);
            return response; 
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return null; 
        }
    }


    protected async Task<T> EditItemAsync<T>(string apiPath, T item) where T : class
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.PutAsJsonAsync($"Api/{apiPath}", item).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
            }
            else
            {
                await ShowApiErrorAsync(response.Content).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        return default;
    }

    protected async Task<bool> DeleteItemAsync(string apiPath)
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.DeleteAsync($"Api/{apiPath}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                await ShowApiErrorAsync(response.Content).ConfigureAwait(false);
                return false;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return false;
        }
    }
    #endregion API Calls
}