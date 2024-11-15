using Blazored.LocalStorage;
using Blazored.SessionStorage;
using EDC.Client.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace EDC.Client.Pages;

public partial class EntryList
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }


    [Inject]
    public ILocalStorageService LocalStore { get; set; }

    private EntryReportModel model = new();

    public List<EntryReportModel> EntriesList { get; set; } = new List<EntryReportModel>();
    public List<EntryReportModel> EntryCountList { get; set; } = new List<EntryReportModel>();
    List<EntryReportModel> SortedEntryCountList { get; set; }

    public DateTime? Entrydate { get; set; }

    public bool Loading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        try
        {
            await LoadEntries().ConfigureAwait(false);
            await base.OnInitializedAsync();
            Entrydate = DateTime.Today;
            LoadData();

        }
        catch (Exception ex)
        {
            Snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
        }
        Loading = false;
    }

    private async Task LoadEntries()
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"Api/Survey/EntryList").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<EntryReportModel[]>().ConfigureAwait(false);
                EntriesList = content.ToList();
                LoadData();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

 private void LoadData()
{
    try
    {
        EntryCountList = EntriesList.ToList();
        if (Entrydate != null)
        {
            SortedEntryCountList = EntryCountList
                .Where(x => x.EntryDate.HasValue && x.EntryDate.Value.ToShortDateString() == Entrydate.Value.Date.ToShortDateString())
                .OrderByDescending(x => x.RecordCount)
                .ToList();
        }
        else
        {
            SortedEntryCountList = EntryCountList.OrderByDescending(x => x.RecordCount).ToList();
        }
    }
    catch (Exception ex)
    {
        Snackbar.Add(ex.Message, Severity.Error);
    }
}


}
