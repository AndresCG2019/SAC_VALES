using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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

        public TalonerasPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Taloneras";
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddTalonera));

        private async void AddTalonera() 
        {
            Debug.WriteLine("LLEGUE A ADD TALONERA");
        }
    }
}
