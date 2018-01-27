using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestreamChatHacking
{
    
    [Serializable]
    public class RestreamChatMessage
    {


        public UserIdentifier UserID {
            get
            {
                return new UserIdentifier(_platformId, _userName);
            }
        }

        public RestreamChatMessage()
        { }

        public RestreamChatMessage(string userName, string message):this(userName, message, ChatPlatform.MockUp)
        {
           
        }
        public RestreamChatMessage(string userName, string message , ChatPlatform platform )
        {
            UserName = userName;
            Message = message;
            SetDateToNow();
            Platform = platform;
        }

        public string Id
        {
            get { return Timestamp + "|" + UserName; }
        }
        [SerializeField]
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [SerializeField]
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        [SerializeField]
        private string _when;

        public string When
        {
            get { return _when; }
            set { _when = value; }
        }

        [SerializeField]
        private string _date;

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public bool IsDateDefined() { return !string.IsNullOrEmpty(_date); }
        public void SetDateToNow()
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            When = DateTime.Now.ToString("hh:mm:ss");
        }

        public double Timestamp { get { return (new DateTime(Year, Month, Day, Hour, Minute, Second).Subtract(new DateTime(1970, 1, 1))).TotalSeconds; } }

       
        [SerializeField]
        private ChatPlatform _platformId;

        public ChatPlatform Platform
        {
            get { return _platformId; }
            set { _platformId = value; }
        }

        public int Year { get { return int.Parse(Date.Substring(0, 4)); } }
        public int Month { get { return int.Parse(Date.Substring(5, 2)); } }
        public int Day { get { return int.Parse(Date.Substring(8, 2)); } }
        public int Hour { get { return int.Parse(Date.Substring(0, 2)); } }
        public int Minute { get { return int.Parse(Date.Substring(2, 2)); } }
        public int Second { get { return int.Parse(Date.Substring(5, 2)); } }

        public void SetPlatform(ChatPlatform platform)
        {
            _platformId = platform;
        }
        public void SetPlatform(int platformId)
        {
            _platformId = (ChatPlatform)platformId;
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
    public enum ChatPlatform : int
    {
        MockUp = -2,
        Unknow = -1,
        RestreamInitMessage = 0,
        Twitch = 1,
        Youtube = 5,
        Facebook = 37,
        Restream = 100,
        Discord = 1001
    }

    [Serializable]
    public struct UserIdentifier {


        public string GetID() {
            return _platformId + ":" + _userName;
        }

        public UserIdentifier(ChatPlatform platform, string userName) {
            _platformId = platform;
            _userName = userName;

        }


        [SerializeField]
        private ChatPlatform _platformId;

        public ChatPlatform Platform
        {
            get { return _platformId; }
            private set { _platformId = value; }
        }
        [SerializeField]
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            private set { _userName = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}",  UserName, Platform);
        }

        public static bool operator ==(UserIdentifier obj1, UserIdentifier obj2)
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

            return (obj1.UserName == obj2.UserName
                    && obj1.Platform == obj2.Platform);
        }

        // this is second one '!='
        public static bool operator !=(UserIdentifier obj1, UserIdentifier obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(UserIdentifier other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Platform.Equals(other.Platform)
                   && UserName.Equals(other.UserName);
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

            return obj.GetType() == GetType() && Equals((UserIdentifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Platform.GetHashCode();
                hashCode = (hashCode * 397) ^ UserName.GetHashCode();;
                return hashCode;
            }
        }


    }
}