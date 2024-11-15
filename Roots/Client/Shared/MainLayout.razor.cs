using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR.Client;


namespace EDC.Client.Shared
{
    public partial class MainLayout
    {
        public string Namefirstletter { get; set; }
        #region Dependencies

        [Inject]
        public AppTheme AppTheme { get; set; }

        [Inject]
        protected IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        protected ILogger<MainLayout> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }


        [Inject]
        protected ILocalStorageService LocalStore { get; set; }

        #endregion Dependencies


        private IDialogReference _dialog;
        private HubConnection _hubConnnection;


    }
}