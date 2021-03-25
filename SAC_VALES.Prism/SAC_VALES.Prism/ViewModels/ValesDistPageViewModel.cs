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
    public class ValesDistPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private List <ValeResponse> _vales;
        private bool _isRunning;
        private UserResponse _user;

        public ValesDistPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Vales Distribuidor";
            _apiService = apiService;
            LoadUser();
            LoadVales();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<ValeResponse> Vales
        {
            get => _vales;
            set => SetProperty(ref _vales, value);

        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void LoadVales() 
        {
            Debug.WriteLine("LLEGUE A LOAD VALES");
            
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            DistValesRequest request = new DistValesRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService.GetValesByDist(url, "/api/ValeEntities", "/GetValesByDist", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(response.Message);
                return;
            }

            Vales = (List<ValeResponse>)response.Result;
            IsRunning = false;

            Debug.WriteLine("LLEGUE AL FINAL");
        }

    }
}
