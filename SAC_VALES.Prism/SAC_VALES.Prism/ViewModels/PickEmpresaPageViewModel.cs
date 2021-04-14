using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Helpers;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SAC_VALES.Prism.ViewModels
{
    public class PickEmpresaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<EmpresaResponse> _empresas;
        private List<EmpresaResponse> _empresasFiltered;
        private bool _isRunning;
        private UserResponse _user;
        private DelegateCommand<object> _CreateTaloneraCommand;

        public PickEmpresaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Elegir Empresa";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadEmpresas();
        }
        public ICommand searchCommand => new Command<string>(SearchEmpresas);
        public DelegateCommand<object> CreateTaloneraCommand => _CreateTaloneraCommand
           ?? (_CreateTaloneraCommand = new DelegateCommand<object>(CreateTalonera));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<EmpresaResponse> Empresas
        {
            get => _empresas;
            set => SetProperty(ref _empresas, value);

        }

        public List<EmpresaResponse> EmpresasFiltered
        {
            get => _empresasFiltered;
            set => SetProperty(ref _empresasFiltered, value);

        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                Debug.WriteLine("EL ID DEL DIST ES...");
                Debug.WriteLine(User.Dist.id);
            }
        }

        private async void LoadEmpresas() 
        {
            Debug.WriteLine("LLEGUE A LOAD Empresas");

            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            TalonerasByDistRequest request = new TalonerasByDistRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService.GetEmpresas(url, "/api/EmpresaEntities", "/GetEmpresas");

            if (!response.IsSuccess)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(response.Message);
                return;
            }

            Empresas = (List<EmpresaResponse>)response.Result;

            EmpresasFiltered = (List<EmpresaResponse>)response.Result;

            IsRunning = false;
        }

        public void SearchEmpresas(string query)
        {
            List<EmpresaResponse> result = Empresas
                .Where(e => e.NombreEmpresa.ToLower().Contains(query.ToLower()) ||
                e.Email.ToLower().Contains(query)).ToList();

            EmpresasFiltered = result;
        }

        public async void CreateTalonera(object parameter)
        {
            var p = new NavigationParameters();
            p.Add("Empresa", parameter);

            Debug.WriteLine("LLEGUE A IR A CREAR TALONERA");

            await _navigationService.NavigateAsync("CreateTaloneraPage", p);
        }
    }
}
