using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAC_VALES.Prism.ViewModels
{
    public class PickClientPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ClieResponse> _clientes;
        private List<ClieResponse> _clientesFiltered;
        private bool _isRunning;
        private UserResponse _user;

        public PickClientPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Elegir Cliente";
            _navigationService = navigationService;
            _apiService = apiService;
        }
    }
}
