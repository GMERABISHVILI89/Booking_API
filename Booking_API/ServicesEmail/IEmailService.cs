using Booking_API.EmailHelper;

namespace Booking_API.ServicesEmail
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest); 
    }
}
