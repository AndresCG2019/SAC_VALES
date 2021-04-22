using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SAC_VALES.Common.Helpers;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using SAC_VALES.Prism.Helpers;
using Xamarin.Forms;
using SAC_VALES.Prism.ViewModels;
using SAC_VALES.Prism;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SAC_VALES.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private ImageSource _image;
        private UserRequest _user;
        private Role _role;
        private ObservableCollection<Role> _roles;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;

        public RegisterPageViewModel(
            INavigationService navigationService,
            IRegexHelper regexHelper,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = "Registrar";
            IsEnabled = true;
            User = new UserRequest();
            Roles = new ObservableCollection<Role>(CombosHelper.GetRoles());
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public Role Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
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


            User.UserTypeId = Role.Id;
            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account", User);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            // SE LOGEA AL USUARIO TRAS REGISTRARSE

              TokenRequest request = new TokenRequest
            {
                Contraseña = User.Password,
                Username = User.Email
            };

            Response Tokenresponse = await _apiService.GetTokenAsync(url, "/api/Account", "/CreateToken", request);

            if (!Tokenresponse.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Error de login", "Aceptar");
                return;
            }

            TokenResponse token = (TokenResponse)Tokenresponse.Result;
            EmailRequest emailRequest = new EmailRequest
            {
                Email = User.Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MainPage");

        }

        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Correo", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.Nombre) || string.IsNullOrEmpty(User.Apellidos) || 
                string.IsNullOrEmpty(User.Direccion) || string.IsNullOrEmpty(User.Telefono)) 
            {
                await App.Current.MainPage
                    .DisplayAlert
                    (
                        "Error", "Es necesario que llene los campos de Nombre, Apellidos, Direccion y Telefono ",
                         "Aceptar"
                    );
                return false;
            }

            if (Role == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe elegir un rol.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Contraseña", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error de Contraseña", "Aceptar");
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Las Contraseñas No Coinciden", "Aceptar");
                return false;
            }

            return true;
        }
    }
}
