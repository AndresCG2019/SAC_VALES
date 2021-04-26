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
using System.Threading.Tasks;

namespace SAC_VALES.Prism.ViewModels
{
    public class RegisterClientPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private PostClieAsDistRequest _newUser;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;

        public RegisterClientPageViewModel(INavigationService navigationService,
             IRegexHelper regexHelper, IApiService apiService) : base(navigationService)
        {
            Title = "Registrar Cliente";
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            IsEnabled = true;
            NewUser = new PostClieAsDistRequest();
            LoadUser();
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public PostClieAsDistRequest NewUser
        {
            get => _newUser;
            set => SetProperty(ref _newUser, value);
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

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void RegisterAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión internet.", "Aceptar");
                return;
            }

            NewUser.DistId = User.Dist.id;

            //User.UserTypeId = Role.Id;
            Response response = await _apiService.RegisterClient(url, "/api", "/Account/PostClientAsDist", NewUser);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Éxito", "El cliente se registro exitosamente", "Aceptar");
            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MisClientesPage");
        }

        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Correo", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(NewUser.Nombre) || string.IsNullOrEmpty(NewUser.Apellidos) ||
                string.IsNullOrEmpty(NewUser.Direccion) || string.IsNullOrEmpty(NewUser.Telefono))
            {
                await App.Current.MainPage
                    .DisplayAlert
                    (
                        "Error", "Es necesario que llene los campos de Nombre, Apellidos, Direccion y Telefono ",
                         "Aceptar"
                    );
                return false;
            }

            if (string.IsNullOrEmpty(NewUser.Password) || NewUser.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Contraseña", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(NewUser.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Contraseña", "Aceptar");
                return false;
            }

            if (NewUser.Password != NewUser.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Las Contraseñas No Coinciden", "Aceptar");
                return false;
            }

            return true;
        }
    }
}
