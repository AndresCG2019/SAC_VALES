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
    public class PagosDistPageViewModel : ViewModelBase, INavigatedAware
    {

        private ValeResponse _vale;
        private List<PagoResponse> _pagos;
        private DelegateCommand<object> _MarcarPagadoCommand;
        private DelegateCommand _CancelarCommand;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private bool _isRunning;
        private bool _showCollection;

        public PagosDistPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Pagos";
            _apiService = apiService;
            _navigationService = navigationService;
            ShowCollection = true;
        }

        public DelegateCommand<object> MarcarPagadoCommand => _MarcarPagadoCommand
           ?? (_MarcarPagadoCommand = new DelegateCommand<object>(MarcarPagado));

        public DelegateCommand CancelarCommand => _CancelarCommand
           ?? (_CancelarCommand = new DelegateCommand(CancelarVale));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool ShowCollection
        {
            get => _showCollection;
            set => SetProperty(ref _showCollection, value);
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

        public async void MarcarPagado(object parameter)
        {
            string url = App.Current.Resources["UrlAPI"].ToString();

            bool answer = await App.Current.MainPage
                .DisplayAlert("Marcar Pago", "¿Quieres cambiar el estado de este pago?", "Si", "No");

            if (answer == true)
            {
                IsRunning = true;
                ShowCollection = false;

                var connection = await _apiService.CheckConnectionAsync(url);
                if (!connection)
                {
                    IsRunning = false;
                    ShowCollection = true;

                    await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                    return;
                }

                Response response = await _apiService.MarcarPago(url, "/api/PagoEntities", ("/") + parameter.ToString());

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    ShowCollection = true;

                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                    return;
                }

                PagosByValeRequest request = new PagosByValeRequest
                {
                    ValeId = Vale.id
                };

                Response responsePagos = await _apiService.GetPagosByVale(url, "/api/PagoEntities", "/GetPagosByVale", request );

                if (!responsePagos.IsSuccess)
                {
                    IsRunning = false;
                    ShowCollection = true;

                    await App.Current.MainPage.DisplayAlert("Error", responsePagos.Message, "Aceptar");
                    return;
                }

                Pagos = (List<PagoResponse>)responsePagos.Result;

                IsRunning = false;
                ShowCollection = true;
            }

            IsRunning = true;
            ShowCollection = false;

            PagosByValeRequest request2 = new PagosByValeRequest
            {
                ValeId = Vale.id
            };

            Response responsePagos2 = await _apiService.GetPagosByVale(url, "/api/PagoEntities", "/GetPagosByVale", request2);

            if (!responsePagos2.IsSuccess)
            {
                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Error", responsePagos2.Message, "Aceptar");
                return;
            }

            Pagos = (List<PagoResponse>)responsePagos2.Result;

            IsRunning = false;
            ShowCollection = true;
        }

        public async void CancelarVale() 
        {
            string url = App.Current.Resources["UrlAPI"].ToString();

            bool answer = await App.Current.MainPage
                .DisplayAlert("Cancelar Vale", "¿Deseas cancelar este vale?", "Si", "No");

            if (answer == true)
            {
                IsRunning = true;
                ShowCollection = false;

                var connection = await _apiService.CheckConnectionAsync(url);
                if (!connection)
                {
                    IsRunning = false;
                    ShowCollection = true;

                    await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                    return;
                }

                Response response = await _apiService.CancelarVale(url, "/api/ValeEntities", ("/") + Vale.id);

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    ShowCollection = true;

                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                    return;
                }

                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Exito", "Se ha cancelado el vale exitosamente", "Aceptar");

                await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/ValesDistPage");
            } 
        }
    }
}
