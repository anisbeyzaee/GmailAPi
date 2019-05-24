using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleEmailAPIAdvanced
{
    class Program
    {
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string[] Scopes = { "https://www.googleapis.com/auth/gmail.send"};
        static string fileName = "Image of the California";
        
        public static void SendIt()
        {
            UserCredential credential;
            
            using (var stream =
              new FileStream("credentials_gmail.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var msg = new AE.Net.Mail.MailMessage
            {
                Subject = "Your Subject",
                Body = "Hello, World, from Gmail API! \n You are Receiving this email as a reciepent of FIRE DETECTION ALARM SYSTEM\n" +
                " Fire has been Detected in Image " + fileName + " at " + DateTime.UtcNow.ToString() + "\n",
                From = new MailAddress("AJEFDservice@gmail.com")
            };
            msg.To.Add(new MailAddress("anisbeyzaee@gmail.com"));
            msg.To.Add(new MailAddress("aniss_b_b@yahoo.com"));
            msg.To.Add(new MailAddress("aniss_b_b@yahoo.com"));
            msg.To.Add(new MailAddress("anisbeyzaee@gmail.com"));
            msg.To.Add(new MailAddress("omlet3d@yahoo.com"));
            msg.To.Add(new MailAddress("aniss_b_b@yahoo.com"));
            msg.ReplyTo.Add(msg.From); // Bounces without this!!
            var msgStr = new StringWriter();
            msg.Save(msgStr);
            
            var gmail = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var result = gmail.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "me").Execute();
            List<MailAddress> list = msg.To.ToList<MailAddress>();
            Console.WriteLine("Message ID {0} sent. All Reciepents {1}", result.Id, msg.To.AsEnumerable<MailAddress>().ToString());
            foreach(MailAddress ma in msg.To.ToList<MailAddress>())
            {
                Console.WriteLine("Receiver : " + ma.Address + " ," + ma.DisplayName + " ," + ma.Host + ", " + ma.User + " ," + ma.ToString());
            }
            Console.Read();
            

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






        //public void SendEmail(MyInternalSystemEmailMessage email)
        //{
        //    var mailMessage = new System.Net.Mail.MailMessage();
        //    mailMessage.From = new System.Net.Mail.MailAddress(email.FromAddress);
        //    mailMessage.To.Add(email.ToRecipients);
        //    mailMessage.ReplyToList.Add(email.FromAddress);
        //    mailMessage.Subject = email.Subject;
        //    mailMessage.Body = email.Body;
        //    mailMessage.IsBodyHtml = email.IsHtml;

        //    foreach (System.Net.Mail.Attachment attachment in email.Attachments)
        //    {
        //        mailMessage.Attachments.Add(attachment);
        //    }

        //    var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

        //    var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
        //    {
        //        Raw = Encode(mimeMessage.ToString())
        //    };

        //    Google.Apis.Gmail.v1.UsersResource.MessagesResource.SendRequest request = service.Users.Messages.Send(gmailMessage, ServiceEmail);

        //    request.Execute();
        //}

        public static string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }


        static void Main(string[] args)
        {

            SendIt();
        }

    }
}
