
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

    }
}
