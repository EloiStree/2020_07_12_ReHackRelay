using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Mail;

namespace RestreamChatHacking
{
    class Program
    {
        static void Main(string[] args)
        {


            //TO ADD TO ARGS
            // Config file path with
            // Limite of message to store in memory
            // File Path where to store new messages
            // File Path where to store all messages
            // Filter / Mails messages ex: @Eloi -> mail with the message
            // OSC xor UDP addresses to send new messages info
            // Webhock addresses to send messages info
            Console.Out.WriteLine(Environment.CurrentDirectory);



            AccessRestreamCode d = new AccessRestreamCode();

            d._onMessageDetected += SaveAndNotify;
            d._onMessageDetected += LaunchStreaming;
            d._onMessageDetected += StopStreaming;

            d.SetupTest();
            d.TheAccessRestreamCodeTest();
            d.TeardownTest();


            string answer = "";
            while (answer != "q") {

                
                Console.WriteLine("Do you want to (q)uit");
                answer =  Console.ReadLine();
            }

        }

        private static void LaunchStreaming (RestreamTchatMessage message)
        {
            if (message.Message.Contains("!nexthackathon"))
            {

                Console.WriteLine("Action: Try to launch streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StartStreaming.bat");

            }
        }

        private static void StopStreaming(RestreamTchatMessage message)
        {
            if (message.Message.Contains("!okthanks"))
            {
                Console.WriteLine("Action: Try to stop streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StopStreaming.bat");

            }
        }

        public static int maxMessagesTracked = 30;
        private static Queue<RestreamTchatMessage> _lastMessages = new Queue<RestreamTchatMessage>(maxMessagesTracked);

        //BAD CODE CHANGE LATER -->
        public static MessageCommunication.IThrow recentMessagesFile = new MessageCommunication.ThrowFile() { WriteBy = MessageCommunication.ThrowFile.WriteType.Overriding, FilePath = "LastMessages.json", UseRelativePath = true };
        public static MessageCommunication.IThrow allMessagesFile = new MessageCommunication.ThrowFile() { WriteBy = MessageCommunication.ThrowFile.WriteType.Appending, FilePath = "AllMessages.json", UseRelativePath = true };

        public static MessageCommunication.IThrow mailMeIfTagged = new MessageCommunication.ThrowGoogleMail() {};
        private static void SaveAndNotify(RestreamTchatMessage message)
        {
            while (_lastMessages.Count > maxMessagesTracked)
                _lastMessages.Dequeue();
            _lastMessages.Enqueue(message);
            //File.AppendAllText
            recentMessagesFile.Send(_lastMessages);
            allMessagesFile.Send(_lastMessages);
          // DONE BUT NEET TO BE LINKED TO EXTERNAL FILE WITH MAIL AND PASSWORD OUT OF THE GIT.
		  //  if (message.Message.Contains("JamsCenter")) {
          //      mailMeIfTagged.Send(message);
          //  }
        } 
        //<--BAD CODE CHANGE LATER

    }
    public class MessageCommunication
    {

        public static string ConvertToJson(RestreamTchatMessage message)
        {
            return JsonConvert.SerializeObject(message);
        }
        public static string ConvertToJson(IEnumerable<RestreamTchatMessage> messages)
        {
            return JsonConvert.SerializeObject(messages);
        }
        public interface IThrow
        {

            void Send(RestreamTchatMessage message);
            void Send(IEnumerable<RestreamTchatMessage> messagesGroup);
        }

        public class ThrowFile : IThrow
        {
            public bool UseRelativePath { get; set; }
            private string _filePath;

            public string FilePath
            {
                get { return _filePath; }
                set { _filePath = value; }
            }
            public enum WriteType { Overriding, Appending}
            public WriteType WriteBy { get; set; }

            public void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                Write(ConvertToJson(messagesGroup));
            }

            public void Send(RestreamTchatMessage message)
            {
                Write(ConvertToJson(message));
            }
            public void Write(string json) {
                string path = UseRelativePath ? Environment.CurrentDirectory + "/" + _filePath: _filePath;

                if (WriteBy == WriteType.Overriding) {
                    File.WriteAllText(path, json);
                }
                else if (WriteBy == WriteType.Appending)
                {
                    File.AppendAllText(path, json);

                }

            }
        }

      
        public class ThrowGoogleMail : IThrow
        {

            private string Password;

            public string MyProperty
            {
                get { return Password; }
                set { Password = value; }
            }


            private string _targetMail;

            public string TargetMail
            {
                get { return _targetMail; }
                set { _targetMail = value; }
            }
            private string _yourMail;

            public string YourMail
            {
                get { return _yourMail; }
                set { _yourMail = value; }
            }

            public void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                string msgContent = "";
                foreach (RestreamTchatMessage msg in messagesGroup)
                {
                    msgContent += "<p>" + msg.ToString() + "</p>";
                }
                SendMailByGoogle(msgContent);
            }

            public void Send(RestreamTchatMessage message)
            {
                SendMailByGoogle(message.ToString());
            }

            public void SendMailByGoogle(string content) {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(YourMail, Password);

                MailMessage mm = new MailMessage(YourMail, TargetMail);
                mm.Subject = "Restream Tchat participants";
                mm.Body = "Chat participant was talking to you: " + content;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                
            }
        }
        #region TO DO LATER
        public class ThrowWebPage: IThrow
        {

            private string _pageToCall;
            public string PageToCall
            {
                get { return _pageToCall; }
                set { _pageToCall = value; }
            }

            public enum CommunicationType { GET, POST}
            public CommunicationType Communication { get; set; }

            public void Send(RestreamTchatMessage message)
            {
                throw new NotImplementedException();
            }

            public void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                throw new NotImplementedException();
            }
        }

        #region NETWORK

        public abstract class ThrowNetwork : IThrow
        {

            public string Addresse { get; set; }
            public int PortIn { get; set; }
            public int PortOut { get; set; }

            public abstract void Send(IEnumerable<RestreamTchatMessage> messagesGroup);
            public abstract void Send(RestreamTchatMessage message);
        }
        public class ThrowOSC : ThrowNetwork
        {
            public override void Send(RestreamTchatMessage message)
            {
                throw new NotImplementedException();
            }

            public override void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                throw new NotImplementedException();
            }
        }

        public class ThrowUDP : ThrowNetwork
        {
            public override void Send(RestreamTchatMessage message)
            {
                throw new NotImplementedException();
            }

            public override void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                throw new NotImplementedException();
            }
        }
        public class ThrowTCP : ThrowNetwork
        {
            public override void Send(RestreamTchatMessage message)
            {
                throw new NotImplementedException();
            }

            public override void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                throw new NotImplementedException();
            }
        }

        public class ThrowWebSocket : IThrow
        {
            public void Send(IEnumerable<RestreamTchatMessage> messagesGroup)
            {
                throw new NotImplementedException();
            }

            public void Send(RestreamTchatMessage message)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
        #endregion
    }

}
