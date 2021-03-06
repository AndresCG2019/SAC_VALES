﻿using Newtonsoft.Json;
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
    public class VincularClientesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ClieResponse> _clientes;
        private List<ClieResponse> _clientesFiltered;
        private bool _isRunning;
        private UserResponse _user;
        private DelegateCommand<ClieResponse> _vincularClienteCommand;

        public VincularClientesPageViewModel(INavigationService navigationService, IApiService apiService) 
            : base(navigationService)
        {
            Title = "Vincular Cliente";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadClientes();
        }
        public ICommand searchCommand => new Command<string>(SearchClientes);
        public DelegateCommand<ClieResponse> VincularClienteCommand => _vincularClienteCommand
           ?? (_vincularClienteCommand = new DelegateCommand<ClieResponse>(VincularCliente));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<ClieResponse> Clientes
        {
            get => _clientes;
            set => SetProperty(ref _clientes, value);
        }

        public List<ClieResponse> ClientesFiltered
        {
            get => _clientesFiltered;
            set => SetProperty(ref _clientesFiltered, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void LoadClientes()
        {
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                return;
            }

            ClientesByDistRequest request = new ClientesByDistRequest
            {
                DistId = User.Dist.id
            };

            Response response = await _apiService
                .GetAllClients(url, "/api/ClienteEntities", "/GetAllClients");

            if (!response.IsSuccess)
            {
                IsRunning = false;

                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            Clientes = (List<ClieResponse>)response.Result;

            ClientesFiltered = (List<ClieResponse>)response.Result;

            IsRunning = false;
        }

        public async void VincularCliente(ClieResponse parameter)
        {
            bool answer = await App.Current.MainPage
                .DisplayAlert("Vincular Cliente", "¿Deses vincularte a este cliente?", "Si", "No");

            if (answer == true)
            {
                int id = parameter.id;

                IsRunning = true;

                string url = App.Current.Resources["UrlAPI"].ToString();

                var connection = await _apiService.CheckConnectionAsync(url);
                if (!connection)
                {
                    IsRunning = false;

                    await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a internet.", "Aceptar");
                    return;
                }


                VincularClienteRequest request = new VincularClienteRequest
                {
                    DistId = User.Dist.id,
                    ClienteId = id
                };

                Response response = await _apiService
                    .VincularCliente(url, "/api/ClienteEntities", "/VincularCliente", request);

                if (!response.IsSuccess)
                {
                    IsRunning = false;

                    await App.Current.MainPage
                        .DisplayAlert("Error", "Algo ha salido mal. Probablemente usted ya esta vinculado a este cliente", "Aceptar");
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Éxito", "El cliente ha sido vinculado exitosamente", "Aceptar");

                IsRunning = false;

                await _navigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MisClientesPage");
            }

            LoadClientes();
        }

        public void SearchClientes(string query)
        {
            List<ClieResponse> result = Clientes
                .Where(c => c.Nombre.ToLower().Contains(query.ToLower()) ||
                c.Apellidos.ToLower().Contains(query.ToLower()) ||
                c.Email.ToLower().Contains(query.ToLower())).ToList();

            ClientesFiltered = result;
        }
    }
}
