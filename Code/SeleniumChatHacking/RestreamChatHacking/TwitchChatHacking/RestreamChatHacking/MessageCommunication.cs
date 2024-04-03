using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static RestreamChatHacking.MessageCommunication;
using Rug.Osc;

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

            void SendChatMessage(RestreamChatMessage message);
            void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup);
        }

        public class ThrowFile : IThrow
        {
            public bool UseRelativePath { get; set; }
            private string m_filePath;

            public string FilePath
            {
                get { return m_filePath; }
                set { m_filePath = value; }
            }
            public enum WriteType { Overriding, Appending }
            public WriteType WriteBy { get; set; }

            public void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup)
            {
                Write(ConvertToJson(messagesGroup));
            }

            public void SendChatMessage(RestreamChatMessage message)
            {
                Write(ConvertToJson(message));
            }
            public void Write(string json)
            {
                string path = UseRelativePath ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + m_filePath : m_filePath;

                if (WriteBy == WriteType.Overriding)
                {
                    try
                    {
                        File.WriteAllText(path, json);
                    }
                    catch (Exception) { }
                }
                else if (WriteBy == WriteType.Appending)
                {
                    try
                    {
                        File.AppendAllText(path, json);
                    }
                    catch (Exception) { }

                }

            }
        }


        public class ThrowGoogleMail : IThrow
        {

            private string m_password;

            public string Password
            {
                get { return m_password; }
                set { m_password = value; }
            }


            private string m_targetMail;

            public string TargetMail
            {
                get { return m_targetMail; }
                set { m_targetMail = value; }
            }
            private string m_yourMail;

            public string YourMail
            {
                get { return m_yourMail; }
                set { m_yourMail = value; }
            }

            public void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup)
            {
                string msgContent = "";
                foreach (RestreamChatMessage msg in messagesGroup)
                {
                    msgContent += "<p>" + msg.ToString() + "</p>";
                }
                SendMailByGoogle(msgContent);
            }

            public void SendChatMessage(RestreamChatMessage message)
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
                client.Credentials = new System.Net.NetworkCredential(YourMail, m_password);

                MailMessage mm = new MailMessage(YourMail, TargetMail);
                mm.Subject = "Restream Tchat participants";
                mm.Body = "Chat participant was talking to you: " + content;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

            }
        }

    }
    public class ThrowUDP : IThrow
    {
        public string m_address;
        public int m_portOut = 2512;
        //public int m_portIn = 2511;

        public Sender m_sender=null;
        //public Receiver _receiver;

        public ThrowUDP(string address, int portOut)
        {
            //  m_portIn = portIn;
            m_address = address;
            m_portOut = portOut;

            try
            {

                m_sender = new Sender(address,portOut);
            }
            catch (Exception) { Console.WriteLine("UDP SENDER NOT ALLOWED"); }
        }
        //void SendMessage(IEnumerable<RestreamChatMessage> messagesGroup)
        //{
        //    foreach (var message in messagesGroup)
        //    {
        //        this.SendMessage(message);
        //    }

        //}

        void SendMessage(RestreamChatMessage message)
        {
            //Console.WriteLine("UDP -> " + message);
            if (m_sender != null && message != null)
                m_sender.Send(message.GetAsOneLiner());
        }

        public string CompressedMessage(RestreamChatMessage msg)
        {
            //To Change later to a really compressed string
            return msg.ToString();
        }

        void IThrow.SendChatMessage(RestreamChatMessage message)
        {
            SendMessage(message);
        }

        void IThrow.SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            throw new Exception();
          //  SendMessage(messagesGroup);
        }

        public class Receiver
        {
            private int _usedPort;
            public Receiver(int port)
            {
                _usedPort = port;
                udp = new UdpClient(_usedPort);
                StartListening();
            }

            private readonly UdpClient udp;
            private void StartListening()
            {
                this.udp.BeginReceive(Receive, new object());
            }
            private void Receive(IAsyncResult ar)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, _usedPort);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                string message = Encoding.Unicode.GetString(bytes);
                StartListening();
            }
        }

        public class Sender : IDisposable
        {
            public string m_address;
            public int m_usedPort;
            public UdpClient m_client;
            public IPEndPoint ip;
            public Sender(string address, int port)
            {
                m_address = address;
                m_usedPort = port;
                IPAddress ipa = string.IsNullOrEmpty(m_address) ? IPAddress.Broadcast : IPAddress.Parse(m_address);
                ip = new IPEndPoint(ipa, m_usedPort);
                m_client = new UdpClient();
                //m_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                m_client.Connect(ip);
            }
            public void Send(string message)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(message);
                m_client.Send(bytes, bytes.Length);

            }
            public void Dispose()
            {
                m_client.Close();
            }

            public void Ping()
            {
                Send("Ping");
            }
        }


        
    }
    #region TO DO LATER
    //public class ThrowWebPage : IThrow
    //{

    //    private string _pageToCall;
    //    public string PageToCall
    //    {
    //        get { return _pageToCall; }
    //        set { _pageToCall = value; }
    //    }

    //    public enum CommunicationType { GET, POST }
    //    public CommunicationType Communication { get; set; }

    //    public void SendChatMessage(RestreamChatMessage message)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #region NETWORK

    public abstract class ThrowNetwork : IThrow, IDisposable
    {

        public string Addresse { get; set; }
        public int PortOut { get; set; }

        public abstract void CreateNetwork();
        public abstract void DestroyNetwork();

        public ThrowNetwork(string address, int port) {
            PortOut = port;
            Addresse = address;
            CreateNetwork();
        }

        public void Dispose()
        {
            DestroyNetwork();
        }

        public abstract void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup);
        public abstract void SendChatMessage(RestreamChatMessage message);
    }
    public class ThrowOSC : ThrowNetwork
    {

        public ThrowOSC(string address, int port) : base(address, port) { }

        OscSender sender;
        public override void CreateNetwork()
        {
            IPAddress address = IPAddress.Broadcast;
            if (!string.IsNullOrEmpty(Addresse)) 
                address = IPAddress.Parse(Addresse);
            sender = new OscSender(address, base.PortOut);
            sender.Connect();
        }

        public override void DestroyNetwork()
        {
            if(sender!=null)
                sender.Close();
        }

        public override void SendChatMessage(RestreamChatMessage message)
        {
                try
                {
                    sender.Send(new OscMessage("/RCH_Message", message.GetAsOneLiner()));
                }
                catch (System.Net.Sockets.SocketException) {
                    Console.WriteLine("ERROR ! The port "+ PortOut+" is used by some an other application.");

                }
        }

        public override void SendChatMessage(IEnumerable<RestreamChatMessage> messagesGroup)
        {
            foreach (var message in messagesGroup)
            {
                SendChatMessage(message);
            }
           
        }
    }

   
    #endregion
    #endregion
}
