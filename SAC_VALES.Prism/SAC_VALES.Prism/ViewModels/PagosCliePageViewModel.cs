using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SAC_VALES.Prism.ViewModels
{
    public class PagosCliePageViewModel : ViewModelBase, INavigatedAware
    {

        private ValeResponse _vale;
        private List<PagoResponse> _pagos;
        private readonly IApiService _apiService;
        private bool _isRunning;

        public PagosCliePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Pagos";
            _apiService = apiService;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ValeResponse Vale
        {
            get => _vale;
            set => SetProperty(ref _vale, value);
        }

        public List<PagoResponse> Pagos
        {
            get => _pagos;
            set => SetProperty(ref _pagos, value);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            Vale = parameters.GetValue<ValeResponse>("Vale");

            List<PagoResponse> pagos = new List<PagoResponse>();

            for (int i = 0; i < Vale.Pagos.Count; i++)
            {
                pagos.Add(Vale.Pagos[i]);
            }

            Pagos = pagos;
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }
    }
}
