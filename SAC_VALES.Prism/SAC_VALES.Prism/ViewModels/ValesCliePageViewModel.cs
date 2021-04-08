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
    public class ValesCliePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ValeResponse> _vales;
        private List<PagoResponse> _pagos;
        private List<ValeResponse> _valesFiltered;
        private bool _isRunning;
        private UserResponse _user;
        private DelegateCommand<object> _GoToValeCommand;

        public ValesCliePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Vales Cliente";
            _apiService = apiService;
            _navigationService = navigationService;
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
                Debug.WriteLine("EL ID DEL CLIENTE ES...");
                Debug.WriteLine(User.Clie.id);
            }
        }

        private async void LoadVales()
        {
            Debug.WriteLine("LLEGUE A LOAD VALES");

            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            DistValesRequest request = new DistValesRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService.GetValesByDist(url, "/api/ValeEntities", "/GetValesByClie", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                Debug.WriteLine("MENSAJE DE ERROR");
                Debug.WriteLine(response.Message);
                return;
            }

            Vales = (List<ValeResponse>)response.Result;
            Pagos = new List<PagoResponse>();

            for (int i = 0; i < Vales.Count; i++)
            {
                Debug.WriteLine("HOLA");
                for (int j = 0; j < Vales[i].Pagos.Count; j++)
                {
                    Debug.WriteLine("HOLA");
                    Debug.WriteLine(Vales[i].Pagos[j].id);
                    Pagos.Add(Vales[i].Pagos[j]);
                }
                Debug.WriteLine("HOLA");
            }

            ValesFiltered = (List<ValeResponse>)response.Result;

            IsRunning = false;
        }

        public void SearchVales(string query)
        {
            List<ValeResponse> result = Vales
                .Where(v => v.NumeroFolio.ToString().Contains(query) ||
                v.Monto.ToString().Contains(query) ||
                v.Cliente.Email.ToLower().Contains(query.ToLower())).ToList();

            ValesFiltered = result;
        }

        public async void GoToVale(object parameter)
        {
            var p = new NavigationParameters();
            p.Add("Vale", parameter);

            await _navigationService.NavigateAsync("PagosDistPage", p);
        }

    }
}
