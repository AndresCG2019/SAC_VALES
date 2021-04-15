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

namespace SAC_VALES.Prism.ViewModels
{
    public class CreateValePageViewModel : ViewModelBase, INavigatedAware
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private TaloneraResponse _talonera;
        private ClieResponse _cliente;
        private ValeResponse _vale;
        private UserResponse _user;
        private DelegateCommand _guardarValeCommand;
        private bool _isRunning;
        private bool _isEnabled;

        public CreateValePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Crear Vale";
            Vale = new ValeResponse();
            Vale.FechaPrimerPago = DateTime.UtcNow;
            _apiService = apiService;
            _navigationService = navigationService;
            IsEnabled = true;
            LoadUser();
        }

        public DelegateCommand GuardarValeCommand => _guardarValeCommand ?? 
            (_guardarValeCommand = new DelegateCommand(GuardarVale));

        public TaloneraResponse Talonera
        {
            get => _talonera;
            set => SetProperty(ref _talonera, value);
        }

        public ValeResponse Vale
        {
            get => _vale;
            set => SetProperty(ref _vale, value);
        }

        public ClieResponse Cliente
        {
            get => _cliente;
            set => SetProperty(ref _cliente, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            Cliente = new ClieResponse();
            Cliente = parameters.GetValue<ClieResponse>("Cliente");

            Talonera = new TaloneraResponse();
            Talonera = parameters.GetValue<TaloneraResponse>("Talonera");
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void GuardarVale() 
        {
            IsEnabled = false;
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión internet.", "Aceptar");
                return;
            }

            if (Vale.NumeroFolio <= 0 || Vale.Monto <= 0 || Vale.CantidadPagos <= 0)
            {
                IsRunning = false;
                IsEnabled = true;

                await App.Current.MainPage
                    .DisplayAlert("Error", "Los campos no pueden tener valores negativos o de 0", "Aceptar");
                return;
            }

            CreateValeRequest request = new CreateValeRequest
            {
                NumeroFolio = Vale.NumeroFolio,
                Monto = Vale.Monto,
                CantidadPagos = Vale.CantidadPagos,
                DistId = User.Dist.id,
                TaloneraId = Talonera.id,
                ClienteId = Cliente.id,
                EmpresaId = Talonera.Empresa.id,
                FechaPrimerPago = Vale.FechaPrimerPago
            };

            Response response = await _apiService.PostVale(url, "/api", "/ValeEntities/PostVale", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;

                await App.Current.MainPage.DisplayAlert("Error", "Algo Salio Mal. Compruebe los valores ingresados", "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Exito", "Se ha creado el vale exitosamente", "Aceptar");

            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/ValesDistPage");
            
            IsEnabled = true;
            IsRunning = false;
        }
    }
}
