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
        private bool _isRunning;
        private bool _showCollection;

        public PagosDistPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Pagos";
            _apiService = apiService;
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
                Debug.WriteLine("HOLA ITERANDO PAGOS");
                Debug.WriteLine(Vale.Pagos[i].id);
                pagos.Add(Vale.Pagos[i]);
            }

            Pagos = pagos;


            Debug.WriteLine("LLEGUE A NAVEGANDO");
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void MarcarPagado(object parameter)
        {
            Debug.WriteLine("LLEGUE A MARCAR PAGADO");

            string url = App.Current.Resources["UrlAPI"].ToString();

            bool answer = await App.Current.MainPage
                .DisplayAlert("Marcar Pago", "¿Quieres cambiar el estado de este pago?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

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
                    Debug.WriteLine("MENSAJE DE ERROR");
                    Debug.WriteLine(response.Message);
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
                    Debug.WriteLine("MENSAJE DE ERROR");
                    Debug.WriteLine(responsePagos.Message);
                    return;
                }

                Pagos = (List<PagoResponse>)responsePagos.Result;

                IsRunning = false;
                ShowCollection = true;

                Debug.WriteLine("ACABE");
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
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(responsePagos2.Message);
                return;
            }

            Pagos = (List<PagoResponse>)responsePagos2.Result;

            IsRunning = false;
            ShowCollection = true;
        }

        public async void CancelarVale() 
        {
            await App.Current.MainPage.DisplayAlert("Error", "error", "Aceptar");
        }
    }
}
