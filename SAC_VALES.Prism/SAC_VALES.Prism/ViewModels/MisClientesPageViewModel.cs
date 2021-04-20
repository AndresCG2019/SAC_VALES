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
    public class MisClientesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ClieResponse> _clientes;
        private List<ClieResponse> _clientesFiltered;
        private bool _isRunning;
        private UserResponse _user;
        private DelegateCommand _addCommand;

        public MisClientesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Mis Clientes";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadClientes();
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddCliente));
        public ICommand searchCommand => new Command<string>(SearchClientes);

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
                Debug.WriteLine("EL ID DEL DIST ES...");
                Debug.WriteLine(User.Dist.id);
            }
        }

        private async void LoadClientes()
        {
            Debug.WriteLine("LLEGUE A LOAD Clientes");

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
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(response.Message);
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

        private async void AddCliente()
        {
            Debug.WriteLine("LLEGUE A ADD CLIENTE");
            bool answer = await App.Current.MainPage
               .DisplayAlert("Agregar Cliente", "¿Deseas Registrar un nuevo cliente o vincularte a uno existente?", 
               "Registrar", "Vincular");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                await _navigationService.NavigateAsync("RegisterClientPage");
            }
            else 
            {
                Debug.WriteLine("Elegiste vincular");
            }
        }
    }
}
