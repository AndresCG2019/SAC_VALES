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
        private readonly IApiService _apiService;

        public PagosDistPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Pago Page";
            _apiService = apiService;
        }

        public DelegateCommand<object> MarcarPagadoCommand => _MarcarPagadoCommand
           ?? (_MarcarPagadoCommand = new DelegateCommand<object>(MarcarPagado));

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

            bool answer = await App.Current.MainPage
                .DisplayAlert("Marcar Pago", "¿Quieres marcar este pago como completado?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                string url = App.Current.Resources["UrlAPI"].ToString();
                Response response = await _apiService.MarcarPago(url, "/api/PagoEntities", "/1");

                if (!response.IsSuccess)
                {
                    //IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                    Debug.WriteLine("MENSAJE DE ERROR");
                    Debug.WriteLine(response.Message);
                    return;
                }
            }
        }
    }
}
