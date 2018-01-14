using MailKit.Net.Smtp;
using MimeKit;

namespace Ankieter.Mails
{
    public class MailService : IMailService
    {
        public void SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Test project", "rafkan144@gmail.com"));
            message.To.Add(new MailboxAddress("raf", "rafkan144@gmail.com"));
            message.Subject = "test mail subject";
            message.Body = new TextPart("plain")
            {
                Text = "hello"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false); // działa
                //client.Connect("smtp.gmail.com", 465, true); // do testów
                //client.Connect("smtp.wp.com", 465, true); // do testów
                client.Authenticate("rafkan144@gmail.com", "mySecretPassword");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}