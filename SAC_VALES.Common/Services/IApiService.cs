
using SAC_VALES.Common.Models;
using System.Threading.Tasks;

namespace SAC_VALES.Common.Services
{
     public interface IApiService
    {
        Task<Response>  GetAdminAsync(int id, string urlBase, string servicePrefix, string controller);
        Task<Response> GetValesAsync(string urlBase, string servicePrefix, string controller);
    }
}
