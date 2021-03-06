﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SAC_VALES.Common.Enums;
using SAC_VALES.Common.Helpers;
using SAC_VALES.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SAC_VALES.Prism.ViewModels
{
    public class ValesMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;

        public ValesMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadUser();

            if (Settings.IsLogin)
            {
                if (User.UserType == UserType.Distribuidor)
                {
                    LoadMenusDist();
                }
                else if (User.UserType == UserType.Cliente)
                {
                    LoadMenusCliente();
                }
            }
            else 
            {
                LoadMenusLoggedOut();
            }
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

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

        private void LoadMenusDist()
        {
            List<Menu> menus = new List<Menu>
            {
                 new Menu
                {
                    Icon = "ic_valesMenu",
                    PageName = "ValesDistPage",
                    Title = "Vales Distribuidor"
                },
                 new Menu
                {
                    Icon = "ic_taloneras",
                    PageName = "TalonerasPage",
                    Title = "Taloneras"
                },
                new Menu
                {
                    Icon = "ic_people",
                    PageName = "MisClientesPage",
                    Title = "Mis Clientes"
                },
                new Menu
                {
                    Icon = "ic_login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Logout" : "Login"
                }

            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

        private void LoadMenusCliente()
        {
            List<Menu> menus = new List<Menu>
            {
                 new Menu
                {
                    Icon = "ic_valesMenu",
                    PageName = "ValesCliePage",
                    Title = "Vales Cliente"
                },
                new Menu
                {
                    Icon = "ic_login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Logout" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }


        private void LoadMenusLoggedOut()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Logout" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
