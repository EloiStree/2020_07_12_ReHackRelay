using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestreamChatHacking
{
    [Serializable]
    public class RestreamTchatMessage
    {

        public RestreamTchatMessage()
        {   }

        public string Id
        {
            get { return When + "|" + UserName; }
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
            _date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public enum TchatPlatform : int
        {
            RestreamInitMessage = 0,
            Twitch = 1,
            Youtube = 5,
            Facebook = 37,
            Restream = 100,
            Discord = 1001
        }
        private TchatPlatform _platformId;

        public TchatPlatform Platform
        {
            get { return _platformId; }
            set { _platformId = value; }
        }

        public void SetPlatform(TchatPlatform platform)
        {
            _platformId = platform;
        }
        public void SetPlatform(int platformId)
        {
            _platformId = (TchatPlatform)platformId;
        }

        public override string ToString()
        {
            return string.Format("{0}({1},{2}):{3}", When, UserName, Platform, Message);
        }

        public static bool operator ==(RestreamTchatMessage obj1, RestreamTchatMessage obj2)
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
        public static bool operator !=(RestreamTchatMessage obj1, RestreamTchatMessage obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(RestreamTchatMessage other)
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

            return obj.GetType() == GetType() && Equals((RestreamTchatMessage)obj);
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