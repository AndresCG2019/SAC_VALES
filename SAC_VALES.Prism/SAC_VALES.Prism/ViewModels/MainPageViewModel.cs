using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Enums;
using SAC_VALES.Common.Helpers;
using SAC_VALES.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SAC_VALES.Prism.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private UserResponse _user;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;

            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                Title = User.UserType.ToString();

                Redirect();
            }
            else 
            {
                Title = "Sesion no Iniciada";
                RedirectLogin();
            }
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public async void Redirect() 
        {
            
            if (User.UserType == UserType.Cliente)
            {
                await _navigationService.NavigateAsync("ValesMasterDetailPage/NavigationPage/ValesCliePage");
            }

            if (User.UserType == UserType.Distribuidor)
            {
                await _navigationService.NavigateAsync("ValesMasterDetailPage/NavigationPage/ValesDistPage");
            }
        }

        public async void RedirectLogin() 
        {
            await _navigationService.NavigateAsync("ValesMasterDetailPage/NavigationPage/LoginPage");
        }
    }
}
