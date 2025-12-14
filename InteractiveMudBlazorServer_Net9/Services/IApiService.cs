using System.Threading.Tasks;
using InteractiveMudBlazorServer_Net9.Dto;

namespace InteractiveMudBlazorServer_Net9.Services
{
    internal interface IApiService
    {
        Task<string> GetGreetingFromApiAsync();
    }
}
