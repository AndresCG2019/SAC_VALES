using Prism;
using Prism.Ioc;
using SAC_VALES.Prism.ViewModels;
using SAC_VALES.Prism.Views;
using SAC_VALES.Common.Services;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace SAC_VALES.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ValesMasterDetailPage, ValesMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryPage, HistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupPage, GroupPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ReportPage, ReportPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
        }
    }
}
