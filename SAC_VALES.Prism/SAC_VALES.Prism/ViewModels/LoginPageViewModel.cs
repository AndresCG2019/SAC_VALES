using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using SAC_VALES.Common.Helpers;
using SAC_VALES.Prism.Views;
using System.Diagnostics;
using SAC_VALES.Common.Enums;

namespace SAC_VALES.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;

        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
           _navigationService = navigationService;
           _apiService = apiService;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

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

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Error de correo",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Error de contraseña",
                    "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
           
            TokenRequest request = new TokenRequest
            {
                Contraseña = Password,
                Username = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "/api/Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Error de login", "Aceptar");
                Password = string.Empty;
                return;
            }


            TokenResponse token = (TokenResponse)response.Result;
            EmailRequest emailRequest = new EmailRequest
            {
                Email = Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response2.Result;

            if (userResponse.UserType == UserType.Admin || userResponse.UserType == UserType.Empresa)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Solo puedes iniciar sesion como cliente o distribuidor", "Aceptar");
                Password = string.Empty;
                return;
            }

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MainPage");
            Password = string.Empty;

        }

        private async void RegisterAsync()
        {
            Debug.WriteLine("LLEGUE A REGISTER ASYNC");
            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/RegisterPage");
        }
    }
}
