using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace TenEightVideo.Web.Mail
{
    public class GMailManager : MailManager
    {
        private string _certificatePath;
        private string _certificatePassword;
        private string _uniqueId;
        private string _user;

        public GMailManager(string uniqueId, string user, string certificatePath, string certificatePassword, string contentRootPath, string mailTransformPath, ILogger<MailManager> logger) 
            : base(contentRootPath, mailTransformPath, logger)
        {
            _uniqueId = uniqueId;
            _user = user;
            _certificatePath = certificatePath;
            _certificatePassword = certificatePassword;
        }

        protected override void SendEmail(MailAddress sender, MailAddress recipient, string subject, string body, MailAddress? replyTo = null, Dictionary<string, string>? headers = null, MailAddress? bcc = null)
        {
            var credential = GetGmailCredential(_uniqueId, _user, _certificatePath, _certificatePassword).Result;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) =>
                {
                    // Accept all certificates
                    return true;
                };

                //if(System.Net.ServicePointManager.SecurityProtocol != System.Net.SecurityProtocolType.Tls12)
                //{
                //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //}

                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                Logger.LogInformation("Gmail SMTP server connected.");

                // use the access token
                var oauth2 = new SaslMechanismOAuth2(credential.User, credential.Token.AccessToken);
                client.Authenticate(oauth2);//.ConfigureAwait(false);

                //var task= client.AuthenticateAsync("webserver@10-8video.com", "504472410c05032e6103f4098acba375023da669");
                Logger.LogInformation("Gmail SMTP server client authenticated.");

                var message = new MimeMessage()
                {
                    Subject = subject,
                    From = { new MailboxAddress("Sender", sender.Address) },
                    To = { new MailboxAddress("Recipient", recipient.Address) },
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = body
                    },
                };
                if (replyTo != null)
                {
                    message.ReplyTo.Add(new MailboxAddress("Reply To", replyTo.Address));
                }
                if (bcc != null)
                {
                    message.Bcc.Add(new MailboxAddress("Bcc", bcc.Address));
                }
                if (headers != null && headers.Any())
                {
                    foreach (var header in headers.Keys)
                    {
                        message.Headers.Add(header, headers[header]);
                    }
                }

                client.Send(message);

                Logger.LogInformation("Gmail SMTP server message sent.");
                
                client.Disconnect(true);
            }

        }

        public async Task<ServiceAccountCredential> GetGmailCredential(string uniqueId, string user, string certificatePath, string password)
        {
            Logger.LogInformation($"Loading certificate at {certificatePath}");
            //var path = Path.Combine(certificatePath, "leafy-sanctuary-458316-k3-538aeb0c07bd.p12");
            var bytes = File.ReadAllBytes(certificatePath);
            var certificate = X509CertificateLoader.LoadPkcs12(bytes, password);

            //var certificate = new X509Certificate2(certificatePath, password, X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential
                //.Initializer("id-0-8videosmtp@leafy-sanctuary-458316-k3.iam.gserviceaccount.com")
                .Initializer(uniqueId)
            //.Initializer("webserver@10-8video.com")
            {
                // Note: other scopes can be found here: https://developers.google.com/gmail/api/auth/scopes
                Scopes = new[] { GmailService.Scope.GmailSend, GmailService.Scope.MailGoogleCom },
                User = user
            }.FromCertificate(certificate));

            bool success = await credential.RequestAccessTokenAsync(CancellationToken.None).ConfigureAwait(false);
            if (!success)
            {
                Logger.LogError("Failed to request access token.");
                throw new Exception("Failed to request access token.");
            }
            return credential;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        private static string Encode(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
