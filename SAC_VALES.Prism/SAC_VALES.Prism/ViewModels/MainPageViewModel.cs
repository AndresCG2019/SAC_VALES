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

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                Title = User.UserType.ToString();
            }
            else 
            {
                Title = "Sesion no Iniciada";
            }
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
    }
}
