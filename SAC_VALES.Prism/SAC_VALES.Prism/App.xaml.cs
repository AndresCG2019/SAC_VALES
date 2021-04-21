using Prism;
using Prism.Ioc;
using SAC_VALES.Prism.ViewModels;
using SAC_VALES.Prism.Views;
using SAC_VALES.Common.Services;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Syncfusion.Licensing;
using SAC_VALES.Common.Helpers;

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
            SyncfusionLicenseProvider.RegisterLicense("NDA5NzAyQDMxMzgyZTM0MmUzME1saHpGNllxTE82aHBqK0NHekk5eWVrWCs3U2JWMkw5YThKdjFIQ3dNSDA9");

            InitializeComponent();

            await NavigationService.NavigateAsync("/ValesMasterDetailPage/NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ValesMasterDetailPage, ValesMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryPage, HistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupPage, GroupPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ReportPage, ReportPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<ValesDistPage, ValesDistPageViewModel>();
            containerRegistry.RegisterForNavigation<ValesCliePage, ValesCliePageViewModel>();
            containerRegistry.RegisterForNavigation<PagosDistPage, PagosDistPageViewModel>();
            containerRegistry.RegisterForNavigation<PagosCliePage, PagosCliePageViewModel>();
            containerRegistry.RegisterForNavigation<TalonerasPage, TalonerasPageViewModel>();
            containerRegistry.RegisterForNavigation<PickClientPage, PickClientPageViewModel>();
            containerRegistry.RegisterForNavigation<PickEmpresaPage, PickEmpresaPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateTaloneraPage, CreateTaloneraPageViewModel>();
            containerRegistry.RegisterForNavigation<PickTaloneraPage, PickTaloneraPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateValePage, CreateValePageViewModel>();
            containerRegistry.RegisterForNavigation<MisClientesPage, MisClientesPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterClientPage, RegisterClientPageViewModel>();
            containerRegistry.RegisterForNavigation<VincularClientesPage, VincularClientesPageViewModel>();
        }
    }
}
