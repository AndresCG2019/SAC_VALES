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
    public class CreateTaloneraPageViewModel : ViewModelBase, INavigatedAware
    {
        private EmpresaResponse _empresa;
        private TaloneraResponse _talonera;
        private DelegateCommand _createTaloneraCommand;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private UserResponse _user;
        private readonly INavigationService _navigationService;

        public CreateTaloneraPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            LoadUser();
            Title = "Crear Talonera";
            _apiService = apiService;
            _navigationService = navigationService;
            Talonera = new TaloneraResponse();
        }

        public EmpresaResponse Empresa
        {
            get => _empresa;
            set => SetProperty(ref _empresa, value);
        }

        public TaloneraResponse Talonera
        {
            get => _talonera;
            set => SetProperty(ref _talonera, value);
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

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public DelegateCommand CreateTaloneraCommand =>
            _createTaloneraCommand ?? (_createTaloneraCommand = new DelegateCommand(CreateTalonera));

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void CreateTalonera()
        {
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }


            CreateTaloneraRequest request = new CreateTaloneraRequest
            {
                DistId = User.Dist.id,
                EmpresaId = Empresa.id,
                RangoInicio = Talonera.RangoInicio,
                RangoFin = Talonera.RangoFin
            };

            // VALIDACIONES

            if (request.RangoInicio >= request.RangoFin || request.RangoFin <= request.RangoInicio ||
                request.RangoInicio <= 0 || request.RangoFin <= 0)
            {
                await App.Current.MainPage
                    .DisplayAlert("Error de Rango", "Asegurese de ingresar un rango de folios válido", "Aceptar");
                return;
            }

            Response response = await _apiService.PostTalonera(url, "/api/TaloneraEntities", "/PostTalonera", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Algo salio mal ...", "Aceptar");
                return;
            }

            IsRunning = false;

            await App.Current.MainPage.DisplayAlert("Exito", "Se ha creado la talonera exitosamente", "Aceptar");

            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/TalonerasPage");

        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            Empresa = parameters.GetValue<EmpresaResponse>("Empresa");
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }
    }
}
