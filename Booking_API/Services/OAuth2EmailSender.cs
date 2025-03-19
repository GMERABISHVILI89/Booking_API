using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.Net.Mail;

namespace Booking_API.Services
{
    public class OAuth2EmailSender
    {

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly string _senderEmail;
        private readonly ILogger<OAuth2EmailSender> _logger;

        // For this implementation, we'll use token storage rather than a refresh token
        private readonly string _tokenStorePath;

        public OAuth2EmailSender(
                         string clientId,
                         string clientSecret,
                        string refreshToken,  // Added parameter
                        string senderEmail,
                         ILogger<OAuth2EmailSender> logger = null)
            {
                _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
                 _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
                 _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
                _senderEmail = senderEmail ?? throw new ArgumentNullException(nameof(senderEmail));
                _logger = logger;
             }

        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                // Get or create credentials
                var clientSecrets = new ClientSecrets
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret
                };

                // This will handle token refresh automatically
                var credential = new UserCredential(
            new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _clientId,
                        ClientSecret = _clientSecret
                    }
                }),
            "user",
            new TokenResponse { RefreshToken = _refreshToken });

                // Create Gmail service
                var gmailService = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Gmail OAuth2 Sender"
                });

                // Create message
                var emailMessage = CreateMessage(to, subject, body, isBodyHtml);

                // Send message
                await gmailService.Users.Messages.Send(emailMessage, "me").ExecuteAsync();
                _logger?.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }

        private Message CreateMessage(string to, string subject, string body, bool isBodyHtml)
        {
            // Create a standard .NET mail message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            mailMessage.To.Add(to);

            // Convert to MimeKit message
            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            // Convert to Gmail API message format
            using var ms = new MemoryStream();
            mimeMessage.WriteTo(ms);
            var rawMessage = Convert.ToBase64String(ms.ToArray())
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            return new Message { Raw = rawMessage };
        }
    }
}
