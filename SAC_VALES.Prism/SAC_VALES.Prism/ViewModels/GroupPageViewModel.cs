using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Models;
using SAC_VALES.Common.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAC_VALES.Prism.ViewModels
{
    public class GroupPageViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;
        private ValeResponse _vale;

        private DelegateCommand _checkIdCommand;

        public GroupPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            Title = "Vales";

            LoadVales();
        }

        public ValeResponse Vale
        {
            get => _vale;
            set => SetProperty(ref _vale, value);
        }

        private async void LoadVales()
        {
            Debug.WriteLine("ESTOY EJECUTANDO EL METODO");

            string url = App.Current.Resources["UrlAPI"].ToString();
            Debug.WriteLine("URL API");
            Debug.WriteLine(url);

            Response response = await _apiService.GetValesAsync(url, "api", "/ValeEntities");
            if (!response.IsSuccess)
            {
                Debug.WriteLine("LA RESPUESTA FALLO");

                await App.Current.MainPage.DisplayAlert(
                    "La respuesta fallo",
                    response.Message,
                    "Accept");
                return;
            }

            Vale = (ValeResponse)response.Result;
        }


    }
}
