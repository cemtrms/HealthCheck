using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExampleProject.Services
{

    public class NotifacationService : INotifacationService
    {
        public static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        SmtpClient client = new SmtpClient
        {
            Host = Environment.GetEnvironmentVariable("SMTP_HOST"),
            Credentials = new NetworkCredential
            {
                UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME"),
                Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
            }

        };


        public void SendeMail(string messages,string email)
        {
            SmtpClient client = new SmtpClient
            {
                Host = Environment.GetEnvironmentVariable("SMTP_HOST"),
                Credentials = new NetworkCredential
                {
                    UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME"),
                    Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                }

            };
            MailAddress from = new MailAddress("cem.turmuss@gmail.com",
               "Health Check " + (char)0xD8 + " Sitem",
            System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Body = messages;
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Please control your site " + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            string userState = "test message1";
            client.SendAsync(message, userState);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();
            if (answer.StartsWith("c") && mailSent == false)
            {
                client.SendAsyncCancel();
            }
            // Clean up.
            message.Dispose();
            Console.WriteLine("Goodbye.");
        }
    }
}


