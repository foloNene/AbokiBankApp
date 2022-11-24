using AbokiData.Configuration;
using AbokiData.Models.DTOs.Requests;
using AbokiData.Models.DTOs.Responses;
using System.Threading.Tasks;

namespace AbokiAPI.Services
{
    public interface IUserRepository
    {
        Task<RegistrationResponse> LoginAsync(UserLoginRequest user);
        Task<AuthResult> RefreshTokenAsync(TokenRequest tokenRequest);
        Task<RegistrationResponse> RegisterUserAsync(UserRegistrationDto user);
    }
}