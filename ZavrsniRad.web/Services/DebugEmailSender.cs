using System.Diagnostics;

namespace ZavrsniRad.web.Services
{
    public class DebugEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            // U produkciji ovdje ide SMTP / vanjski provider.
            Debug.WriteLine("=== EMAIL SENT (DEBUG) ===");
            Debug.WriteLine($"TO: {toEmail}");
            Debug.WriteLine($"SUBJECT: {subject}");
            Debug.WriteLine($"BODY: {htmlBody}");
            Debug.WriteLine("==========================");

            return Task.CompletedTask;
        }
    }
}
