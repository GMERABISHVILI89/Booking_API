using Booking_API.Models;
using Booking_API.Models.DTO_s.Auth;

namespace Booking_API.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDTO registerDTO);
        Task<ServiceResponse<string>> Login(UserLoginDTO loginDTO);
    }
}
