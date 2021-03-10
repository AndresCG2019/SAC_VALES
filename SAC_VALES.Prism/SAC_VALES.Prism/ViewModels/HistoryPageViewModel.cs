using Prism.Commands;
using Prism.Navigation;
using System.Text.RegularExpressions;
using SAC_VALES.Common.Models;
using SAC_VALES.Prism.ViewModels;
using SAC_VALES.Common.Services;
using SAC_VALES.Prism;
using System.Diagnostics;

namespace SAC_VALES.Prism.ViewModels
{
    public class HistoryPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private AdminResponse _admin;
        private DelegateCommand _checkIdCommand;
        private bool _isRunning;

        public HistoryPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "History";
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }


        public AdminResponse Admin
        {
            get => _admin;
            set => SetProperty(ref _admin, value);
        }

        public int id { get; set; }

        public DelegateCommand CheckIdCommand => _checkIdCommand ?? (_checkIdCommand = new DelegateCommand(CheckIdAsync));

        private async void CheckIdAsync()
        {
            Debug.WriteLine("ESTOY EJECUTANDO EL METODO");
            if (id == 0)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an id.",
                    "Accept");
                return;
            }

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Debug.WriteLine("URL API");
            Debug.WriteLine(url);

            Response response = await _apiService.GetAdminAsync(id, url, "api", "/Administradores");
            IsRunning = false;
            if (!response.IsSuccess)
            {
                Debug.WriteLine("LA RESPUESTA FALLO");

                await App.Current.MainPage.DisplayAlert(
                    "La respuesta fallo",
                    response.Message,
                    "Accept");
                return;
            }

            Admin = (AdminResponse)response.Result;
        }
    }
}
