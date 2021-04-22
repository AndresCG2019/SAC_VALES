using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Helpers;
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
    public class ValesDistPageViewModel : ViewModelBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List <ValeResponse> _vales;
        private List<PagoResponse> _pagos;
        private List<ValeResponse> _valesFiltered;
        private bool _isRunning;
        private bool _showCollection;
        private UserResponse _user;
        private DelegateCommand<object> _GoToValeCommand;
        private DelegateCommand _addCommand;

        public ValesDistPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Vales Distribuidor";
            _apiService = apiService;
            _navigationService = navigationService;
            ShowCollection = true;
            LoadUser();
            LoadVales();
        }

        public ICommand searchCommand => new Command<string>(SearchVales);
        public DelegateCommand<object> GoToValeCommand => _GoToValeCommand 
            ?? (_GoToValeCommand = new DelegateCommand<object>(GoToVale));

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

        public List<ValeResponse> Vales
        {
            get => _vales;
            set => SetProperty(ref _vales, value);

        }

        public List<PagoResponse> Pagos
        {
            get => _pagos;
            set => SetProperty(ref _pagos, value);
        }

        public List<ValeResponse> ValesFiltered
        {
            get => _valesFiltered;
            set => SetProperty(ref _valesFiltered, value);

        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddVale));

        private async void LoadVales() 
        {   
            IsRunning = true;
            ShowCollection = false;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            DistValesRequest request = new DistValesRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService.GetValesByDist(url, "/api/ValeEntities", "/GetValesByDist", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                ShowCollection = true;

                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Vales = (List<ValeResponse>)response.Result;
            Pagos = new List<PagoResponse>();

            for (int i = 0; i < Vales.Count; i++)
            {
                for (int j = 0; j < Vales[i].Pagos.Count; j++)
                {
                    Pagos.Add(Vales[i].Pagos[j]);
                }
            }

            ValesFiltered = (List<ValeResponse>)response.Result;

            IsRunning = false;
            ShowCollection = true;
        }

        public void SearchVales(string query)
        {
            List<ValeResponse> result = Vales
                .Where(v => v.NumeroFolio.ToString().Contains(query) || 
                v.Monto.ToString().Contains(query) ||
                v.Cliente.Email.ToLower().Contains(query.ToLower())).ToList();

            ValesFiltered = result;
        }

        private async void AddVale()
        {
            await _navigationService.NavigateAsync("PickClientPage");
        }

        public async void GoToVale(object parameter) 
        {
            var p = new NavigationParameters();
            p.Add("Vale", parameter);
            
            await _navigationService.NavigateAsync("PagosDistPage", p);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            LoadVales();
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {

        }

    }
}
