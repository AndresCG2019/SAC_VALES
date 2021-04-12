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
    public class TalonerasPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _addCommand;
        private List<TaloneraResponse> _taloneras;
        private List<TaloneraResponse> _talonerasFiltered;
        private bool _isRunning;
        private bool _showCollection;
        private UserResponse _user;

        public TalonerasPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Taloneras";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadTaloneras();
        }

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

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                Debug.WriteLine("EL ID DEL DIST ES...");
                Debug.WriteLine(User.Dist.id);
            }
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddTalonera));

        private async void LoadTaloneras() 
        {
            Debug.WriteLine("LLEGUE A LOAD TALONERAS");

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
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(response.Message);
                return;
            }

            Taloneras = (List<TaloneraResponse>)response.Result;

            TalonerasFiltered = (List<TaloneraResponse>)response.Result;

            IsRunning = false;
            ShowCollection = true;
        }

        private async void AddTalonera() 
        {
            Debug.WriteLine("LLEGUE A ADD TALONERA");
        }
    }
}
