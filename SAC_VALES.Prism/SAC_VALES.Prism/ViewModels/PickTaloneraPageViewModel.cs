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
    public class PickTaloneraPageViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<TaloneraResponse> _taloneras;
        private List<TaloneraResponse> _talonerasFiltered;
        private ClieResponse _cliente;
        private bool _isRunning;
        private bool _showCollection;
        private UserResponse _user;
        private DelegateCommand<object> _CreateValeCommand;

        public PickTaloneraPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Elegir Talonera";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadTaloneras();
        }

        public ICommand searchCommand => new Command<string>(SearchTaloneras);
        public DelegateCommand<object> CreateValeCommand => _CreateValeCommand
           ?? (_CreateValeCommand = new DelegateCommand<object>(CreateVale));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool ShowCollection
        {
            get => _showCollection;
            set => SetProperty(ref _showCollection, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<TaloneraResponse> Taloneras
        {
            get => _taloneras;
            set => SetProperty(ref _taloneras, value);
        }

        public List<TaloneraResponse> TalonerasFiltered
        {
            get => _talonerasFiltered;
            set => SetProperty(ref _talonerasFiltered, value);

        }

        public ClieResponse Cliente
        {
            get => _cliente;
            set => SetProperty(ref _cliente, value);

        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void LoadTaloneras()
        {
            IsRunning = true;
            ShowCollection = false;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            TalonerasByDistRequest request = new TalonerasByDistRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService
                .GetTalonerasByDist(url, "/api/TaloneraEntities", "/GetTalonerasByDist", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Taloneras = (List<TaloneraResponse>)response.Result;

            TalonerasFiltered = (List<TaloneraResponse>)response.Result;

            IsRunning = false;
            ShowCollection = true;
        }

        public void SearchTaloneras(string query)
        {
            List<TaloneraResponse> result = Taloneras
                .Where(t => t.Empresa.Email.ToLower().Contains(query.ToLower()) ||
                t.RangoInicio.ToString().Contains(query) ||
                t.RangoFin.ToString().Contains(query)).ToList();

            TalonerasFiltered = result;
        }

        public async void CreateVale(object parameter)
        {
            var p = new NavigationParameters();
            p.Add("Talonera", parameter);
            p.Add("Cliente", Cliente);

            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/CreateValePage", p);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            Cliente = new ClieResponse();
            Cliente = parameters.GetValue<ClieResponse>("Cliente");
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }
    }
}
