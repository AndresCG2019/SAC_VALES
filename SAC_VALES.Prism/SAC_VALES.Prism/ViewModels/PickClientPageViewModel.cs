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
    public class PickClientPageViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ClieResponse> _clientes;
        private List<ClieResponse> _clientesFiltered;
        private bool _isRunning;
        private UserResponse _user;
        private DelegateCommand<object> _CreateValeCommand;

        public PickClientPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Elegir Cliente";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadClientes();
        }
        public ICommand searchCommand => new Command<string>(SearchClientes);
        public DelegateCommand<object> CreateValeCommand => _CreateValeCommand
           ?? (_CreateValeCommand = new DelegateCommand<object>(CreateVale));

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

        public List<ClieResponse> Clientes
        {
            get => _clientes;
            set => SetProperty(ref _clientes, value);

        }

        public List<ClieResponse> ClientesFiltered
        {
            get => _clientesFiltered;
            set => SetProperty(ref _clientesFiltered, value);

        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void LoadClientes()
        {
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            ClientesByDistRequest request = new ClientesByDistRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService
                .GetClientsByDist(url, "/api/ClienteEntities", "/GetClientesByDist", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Clientes = (List<ClieResponse>)response.Result;

            ClientesFiltered = (List<ClieResponse>)response.Result;

            IsRunning = false;
        }

        public void SearchClientes(string query)
        {
            List<ClieResponse> result = Clientes
                .Where(c => c.Nombre.ToLower().Contains(query.ToLower()) ||
                c.Apellidos.ToLower().Contains(query.ToLower()) ||
                c.Email.ToLower().Contains(query.ToLower())).ToList();     

            ClientesFiltered = result;
        }

        public async void CreateVale(object parameter)
        {
            var p = new NavigationParameters();
            p.Add("Cliente", parameter);

            await _navigationService.NavigateAsync("PickTaloneraPage", p);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            LoadClientes();
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }
    }
}
