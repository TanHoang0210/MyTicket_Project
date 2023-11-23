using Azure.Core;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
namespace MYTICKET.WEB.SERVICE.MailService.Implements
{
    public class EmailSenderService : ServiceBase, IEmailSenderService
    {
        private readonly IOptions<MailSettings> _mailSettings;
        public EmailSenderService(IOptions<MailSettings> setting,ILogger<EmailSenderService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
            _mailSettings = setting;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }

        public async Task SendMail(MailContent mailContent)
        {
            var test = _mailSettings;
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.Value.DisplayName, _mailSettings.Value.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.Value.DisplayName, _mailSettings.Value.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Value.Host, _mailSettings.Value.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Value.Mail, _mailSettings.Value.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);
            }

            smtp.Disconnect(true);
        }
    }
}
