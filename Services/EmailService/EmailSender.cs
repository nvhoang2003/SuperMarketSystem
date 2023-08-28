using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using SendGrid;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.InteropServices.JavaScript;
using Azure.Core;
using Microsoft.AspNetCore.Razor.Language;
using System.Text;
using Org.BouncyCastle.Asn1.Pkcs;
using RazorEngineCore;
using System.Collections.Generic;

namespace SuperMarketSystem.Services.EmailService
{
    public class EmailSender : IEmailService
    {
        private readonly ILogger _logger;
        public readonly IConfiguration _config;
        public MessageOptions _Options { get; } //Set with Secret Manager.
        public EmailSender(IOptions<MessageOptions> optionsAccessor,
                           ILogger<EmailSender> logger,
                           IConfiguration config)
        {
            _Options = optionsAccessor.Value;
            _logger = logger;
            _config = config;
            logger.LogInformation("Create SendMailService");
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {

            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            var emailConfig = _config.GetSection("EmailSettings");
            var authenConfig = _config.GetSection("Authentication:Google");
            var clientId = authenConfig["ClientId"];
            var host = emailConfig["Host"];
            var port = emailConfig["Post"];
            var UseSSL = emailConfig["UseSSL"];
            var UseStartTls = emailConfig["UseStartTls"];
            var emailFrom = emailConfig["EmailFrom"];
            var emailSender = emailConfig["EmailSender"];
            var emailEncoding = emailConfig["EmailEncoding"];
            var key = _Options.EmailSecurity;
            var password = authenConfig["Password"];
            string[] toAdress = toEmail.Split(new string[] { ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                using (var smtpclient = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Thay đổi host và cổng cho MailKit
                    await smtpclient.ConnectAsync(host, 587, SecureSocketOptions.StartTls);
                    // Xác thực nếu cần
                    if (!string.IsNullOrEmpty(emailFrom) && !string.IsNullOrEmpty(password))
                    {

                        await smtpclient.AuthenticateAsync(emailFrom, password);
                    }

                    foreach (var address in toAdress)
                    {
                        var msg = new MimeMessage();
                        msg.From.Add(new MailboxAddress("From", emailFrom));
                        msg.Sender = new MailboxAddress("Sender", emailSender);
                        msg.To.Add(new MailboxAddress("Recipient", address));
                        msg.Subject = subject;
                        var builder = new BodyBuilder();
                        builder.HtmlBody = body;
                        msg.Body = builder.ToMessageBody();
                        // Gửi email
                        await smtpclient.SendAsync(msg);
                        // Tạm dừng 1 giây
                        Thread.Sleep(1000);
                    }
                    // Ngắt kết nối
                    await smtpclient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                //await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("send mail to: " + toEmail);
        }
    }
}
