using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static RestreamChatHacking.MessageCommunication;

namespace RestreamChatHacking
{
    public class MessageCommunication
    {

        public static string ConvertToJson(RestreamChatMessage message)
        {
            return JsonConvert.SerializeObject(message);
        }
        public static string ConvertToJson(IEnumerable<RestreamChatMessage> messages)
        {
            return JsonConvert.SerializeObject(messages);
        }
        public interface IThrow
        {

            void Send(RestreamChatMessage message);
            void Send(IEnumerable<RestreamChatMessage> messagesGroup);
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
            public enum WriteType { Overriding, Appending }
            public WriteType WriteBy { get; set; }

            public void Send(IEnumerable<RestreamChatMessage> messagesGroup)
            {
                Write(ConvertToJson(messagesGroup));
            }

            public void Send(RestreamChatMessage message)
            {
                Write(ConvertToJson(message));
            }
            public void Write(string json)
            {
                string path = UseRelativePath ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + _filePath : _filePath;

                if (WriteBy == WriteType.Overriding)
                {
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

            private string _password;

            public string Password
            {
                get { return _password; }
                set { _password = value; }
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

            public void Send(IEnumerable<RestreamChatMessage> messagesGroup)
            {
                string msgContent = "";
                foreach (RestreamChatMessage msg in messagesGroup)
                {
                    msgContent += "<p>" + msg.ToString() + "</p>";
                }
                SendMailByGoogle(msgContent);
            }

            public void Send(RestreamChatMessage message)
            {
                SendMailByGoogle(message.ToString());
            }

            public void SendMailByGoogle(string content)
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(YourMail, _password);

                MailMessage mm = new MailMessage(YourMail, TargetMail);
                mm.Subject = "Restream Tchat participants";
                mm.Body = "Chat participant was talking to you: " + content;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

            }
        }

    }
    #region TO DO LATER
    public class ThrowWebPage : IThrow
    {

        private string _pageToCall;
        public string PageToCall
        {
            get { return _pageToCall; }
            set { _pageToCall = value; }
        }

        public enum CommunicationType { GET, POST }
        public CommunicationType Communication { get; set; }

        public void Send(RestreamChatMessage message)
        {
            throw new NotImplementedException();
        }

        public void Send(IEnumerable<RestreamChatMessage> messagesGroup)
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

        public abstract void Send(IEnumerable<RestreamChatMessage> messagesGroup);
        public abstract void Send(RestreamChatMessage message);
    }
    public class ThrowOSC : ThrowNetwork
    {
        public override void Send(RestreamChatMessage message)
        {
            throw new NotImplementedException();
        }

        public override void Send(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            throw new NotImplementedException();
        }
    }

    public class ThrowUDP : ThrowNetwork
    {
        public override void Send(RestreamChatMessage message)
        {
            throw new NotImplementedException();
        }

        public override void Send(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            throw new NotImplementedException();
        }
    }
    public class ThrowTCP : ThrowNetwork
    {
        public override void Send(RestreamChatMessage message)
        {
            throw new NotImplementedException();
        }

        public override void Send(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            throw new NotImplementedException();
        }
    }

    public class ThrowWebSocket : IThrow
    {
        public void Send(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            throw new NotImplementedException();
        }

        public void Send(RestreamChatMessage message)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
    #endregion
}
