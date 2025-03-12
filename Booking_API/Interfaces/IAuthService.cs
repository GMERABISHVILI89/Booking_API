using Booking_API.Models;
using Booking_API.Models.DTO_s.Auth;
using Booking_API.Models.DTO_s.UserProfile;

namespace Booking_API.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDTO registerDTO);
        Task<ServiceResponse<string>> Login(UserLoginDTO loginDTO);
        Task<ServiceResponse<UserProfileDTO>> GetProfile(int userId);
         
    }
}
