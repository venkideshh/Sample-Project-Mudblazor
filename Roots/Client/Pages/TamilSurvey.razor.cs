using Blazored.LocalStorage;
using Blazored.SessionStorage;
using EDC.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Linq;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace EDC.Client.Pages;

public partial class TamilSurvey
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    public ILocalStorageService LocalStore { get; set; }

    private MeetingEntryModel model = new();
    private bool success;
    public bool IsValid { get; set; }
    public bool Loading { get; set; }

    private IBrowserFile ImageFile { get; set; }

    public DistrictModel[] DistrictList { get; set; } = [];
    public ParliamentConstituencyModel[] ParliamentConstituencyList { get; set; } = [];
    public ConstituencyModel[] ConstituencyList { get; set; } = [];
    public AreaTypeModel[] AreaTypeList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        try
        {
            await LoadData().ConfigureAwait(false);
            model.DistrictId = null;
            //model.AreaTypeId = null;
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
        }
        Loading = false;
    }

    public async Task LoadData()
    {
        await LoadDistricts().ConfigureAwait(false);
        await LoadParConstituencies().ConfigureAwait(false);
        await LoadConstituencies().ConfigureAwait(false);
        //await Task.WhenAll(task1,task2,task3).ConfigureAwait(false);
    }

    private async Task LoadConstituencies()
    {
        try
        {

            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("Api/Survey/Constituencies").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                ConstituencyList = await response.Content.ReadFromJsonAsync<ConstituencyModel[]>().ConfigureAwait(false);
            }

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task LoadParConstituencies()
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("Api/Survey/ParliamentConstituencies").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                ParliamentConstituencyList = await response.Content.ReadFromJsonAsync<ParliamentConstituencyModel[]>().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task LoadDistricts()
    {
        try
        {
            using var client = HttpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("Api/Survey/Districts").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                DistrictList = await response.Content.ReadFromJsonAsync<DistrictModel[]>().ConfigureAwait(false);
            }
            var response1 = await client.GetAsync("Api/Survey/LoadAreaTypes").ConfigureAwait(false);
            if (response1.IsSuccessStatusCode)
            {
                AreaTypeList = await response1.Content.ReadFromJsonAsync<AreaTypeModel[]>().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private void OnValidSubmit(EditContext context)
    {
        success = true;
        StateHasChanged();
    }

    private bool ContainsNumeric(string input)
    {
        IsValid = input.Any(c => char.IsDigit(c));
        return IsValid;
    }

    private bool ContainsSymbol(string input)
    {
        IsValid = input.Any(c => char.IsSymbol(c));
        return IsValid;
    }

    private bool ContainsLetter(string input)
    {
        IsValid = input.Any(c => char.IsLetter(c));
        return IsValid;
    }
    public async Task Save()
    {
        //var createdBy = await LocalStore.GetItemAsStringAsync("MobileNo").ConfigureAwait(false);
        try
        {
            if (model.DistrictId != null && model.ParliamentConstituencyId != null && model.ConstituencyId != null && model.AreaTypeId != null && !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.AreaName) && !string.IsNullOrEmpty(model.PhoneNo) && !string.IsNullOrEmpty(model.Gender) && !string.IsNullOrEmpty(model.Address) && model.ConstituencyId != null && model.BoothNo != null)
            {
                if (model.BoothNo <= 0)
                {
                    Snackbar.Add("பூத் எண்ணை சரிபார்க்கவும்", Severity.Error);
                    model.BoothNo = null;
                    return;
                }
                if (string.IsNullOrEmpty(model.PhoneNo) || !Regex.IsMatch(model.PhoneNo, @"^\d{10}$"))
                {
                    Snackbar.Add("சரியான 10 இலக்க தொலைபேசி எண்ணை உள்ளிடவும்", Severity.Error);
                    model.PhoneNo = null;
                    return;
                }
                if (ImageFile != null)
                {
                    const long maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
                    if (ImageFile.Size <= maxFileSize)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(ImageFile.Name).ToLower();

                        if (allowedExtensions.Contains(extension))
                        {
                            var buffers1 = new byte[ImageFile.Size];
                            await ImageFile.OpenReadStream(maxAllowedSize: maxFileSize).ReadAsync(buffers1);
                            model.Image = ImageFile.Name;
                            model.Content = buffers1;
                        }
                        else
                        {
                            Snackbar.Add("அனுமதிக்கப்பட்ட படக் கோப்பு வகைகள்: .jpg, .jpeg, .png", Severity.Warning);
                            return;
                        }
                    }
                    else
                    {
                        Snackbar.Add("படத்தின் அளவு 5 MB-க்கு குறைவாக இருக்க வேண்டும்.", Severity.Warning);
                        ImageFile = null;
                        return;
                    }


                    using var client = HttpClientFactory.CreateClient("Api");
                    var response = await client.PostAsJsonAsync("Api/Survey", model).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        model = new MeetingEntryModel();
                        ImageFile = null;
                        Snackbar.Add("தகவல் வெற்றிகரமாக சேமிக்கப்பட்டது", Severity.Success);
                        //NavigationManager.NavigateTo("/Saved");
                    }
                }
                else
                {
                    Snackbar.Add("அனைத்து விவரங்களையும் நிரப்பவும்", Severity.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
        }
    }
    public async Task Logout()
    {
        model = new MeetingEntryModel();
        ImageFile = null;
    }

    public void ParliamentConstituencyClear()
    {
        model.ConstituencyId = null;
    }
    public void DistrictClear()
    {
        model.ParliamentConstituencyId = null;
        model.ConstituencyId = null;
    }
}