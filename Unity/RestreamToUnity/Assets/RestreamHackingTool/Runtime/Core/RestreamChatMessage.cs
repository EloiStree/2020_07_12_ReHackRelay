using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace RestreamChatHacking
{
    [Serializable]
    public class RestreamChatMessage
    {

        public RestreamChatMessage()
        { }

        public RestreamChatMessage(string userName, string message)
        {
            UserName = userName;
            Message = message;
            SetDateToNow();
            Platform = ChatPlatform.Unknow;
        }

        
        public string Id
        {
            get { return Timestamp + "|" + UserName; }
        }
        [SerializeField] string m_message;

        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        [SerializeField] string m_userName;

        public string UserName
        {
            get { return m_userName; }
            set { m_userName = value; }
        }

        [SerializeField] string m_when;

        public string When
        {
            get { return m_when; }
            set { m_when = value; }
        }

        [SerializeField] string m_date;

        public string Date
        {
            get { return m_date; }
            set { m_date = value; }
        }
        public DateTime GetWhenAsDateTime()
        {
            return DateTime.ParseExact(Date + When, "yyyy-MM-ddhh:mm:ss", CultureInfo.InvariantCulture);
        }
        public bool IsDateDefined() { return !string.IsNullOrEmpty(m_date); }
        public void SetDateToNow()
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            When = DateTime.Now.ToString("hh:mm:ss");
        }
        
        public double Timestamp { get { return (new DateTime(Year, Month, Day, Hour, Minute, Second).Subtract(new DateTime(1970, 1, 1))).TotalSeconds; } }

        public string GetAsOneLiner()
        {

            return string.Format("RCM¦{0}¦{1}¦{2}¦{3}¦{4}¶",
                       Date, When, UserName, Platform, Message);
        }
        public void SetWithOneLiner(string text)
        {
           // Debug.Log(">" + text+ "<"   );
            text = text.Trim();
            if (!(text.IndexOf("RCM¦") == 0 &&
                text[text.Length - 1] == '¶'))
                return;
            text = text.Substring(4);
            text = text.Substring(0, text.Length - 1);
            string[] token = text.Split('¦');
            if (token.Length != 5)
                return;
            Date = token[0];
            When = token[1];
            UserName = token[2];
            Platform = GetEnumOf(token[3]);
            Message = token[4];
        }

        private ChatPlatform GetEnumOf(string text)
        {
            try
            {
                ChatPlatform platform;
                if (Enum.TryParse(text, out platform))
                    return platform;
                else return ChatPlatform.Unknow;
            }
            catch (Exception) { return ChatPlatform.Unknow; }
        }

        
        private ChatPlatform m_platformId;

        public ChatPlatform Platform
        {
            get { return m_platformId; }
            set { m_platformId = value; }
        }

        
        public int Year { get { return int.Parse(Date.Substring(0, 4)); } }
        
        public int Month { get { return int.Parse(Date.Substring(5, 2)); } }
        
        public int Day { get { return int.Parse(Date.Substring(8, 2)); } }
        
        public int Hour { get { return int.Parse(Date.Substring(0, 2)); } }
        
        public int Minute { get { return int.Parse(Date.Substring(2, 2)); } }
        
        public int Second { get { return int.Parse(Date.Substring(5, 2)); } }

        public UserIdentifier UserID { get { return new UserIdentifier(m_platformId, m_userName); } }

        public void SetPlatform(ChatPlatform platform)
        {
            m_platformId = platform;
        }


        public override string ToString()
        {
            return string.Format("{0}({1},{2}):{3}", When, UserName, Platform, Message);
        }

        public static bool operator ==(RestreamChatMessage obj1, RestreamChatMessage obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.Message == obj2.Message
                    && obj1.When == obj2.When
                    && obj1.UserName == obj2.UserName
                    && obj1.Date == obj2.Date);
        }

        // this is second one '!='
        public static bool operator !=(RestreamChatMessage obj1, RestreamChatMessage obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(RestreamChatMessage other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Message.Equals(other.Message)
                   && When.Equals(other.When)
                   && UserName.Equals(other.UserName)
                    && Date.Equals(Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((RestreamChatMessage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Message.GetHashCode();
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ When.GetHashCode();
                hashCode = (hashCode * 397) ^ UserName.GetHashCode();
                return hashCode;
            }
        }

    }


}
public enum ChatPlatform
{
    Mockup,
    Unknow,
    RestreamInitMessage,
    Twitch,
    Youtube,
    Facebook,
    Restream,
    Discord,
    LinkedIn,
    Periscope,
    DLive
}