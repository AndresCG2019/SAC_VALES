
using SAC_VALES.Common.Models;
using System.Threading.Tasks;

namespace SAC_VALES.Common.Services
{
     public interface IApiService
    {
        Task<Response>  GetAdminAsync(int id, string urlBase, string servicePrefix, string controller);
        Task<Response> GetValesAsync(string urlBase, string servicePrefix, string controller);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);
        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);
        Task<Response> GetValesByDist(string urlBase, string servicePrefix, string controller, DistValesRequest request);
        Task<Response> MarcarPago(string urlBase, string servicePrefix, string controller);
        Task<Response> GetPagosByVale(string urlBase, string servicePrefix, string controller, PagosByValeRequest request);
        Task<Response> GetValesByClie(string urlBase, string servicePrefix, string controller, ClieValesRequest request);
        Task<Response> GetTalonerasByDist(string urlBase, string servicePrefix, string controller, TalonerasByDistRequest request);
        Task<Response> GetEmpresas(string urlBase, string servicePrefix, string controller);
        Task<Response> PostTalonera(string urlBase, string servicePrefix, string controller, CreateTaloneraRequest request);
        Task<Response> PostVale(string urlBase, string servicePrefix, string controller, CreateValeRequest request);
        Task<Response> GetClientsByDist(string urlBase, string servicePrefix, string controller, ClientesByDistRequest request);
        Task<Response> RegisterClient(string urlBase, string servicePrefix, string controller, PostClieAsDistRequest userRequest);
        Task<Response> GetAllClients(string urlBase, string servicePrefix, string controller);
        Task<Response> VincularCliente(string urlBase, string servicePrefix, string controller, VincularClienteRequest request);
        Task<bool> CheckConnectionAsync(string url);

    }
}
