using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAC_VALES.Prism.ViewModels
{
    public class ValesDistPageViewModel : ViewModelBase
    {
        public ValesDistPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Vales Distribuidor";
        }
    }
}
