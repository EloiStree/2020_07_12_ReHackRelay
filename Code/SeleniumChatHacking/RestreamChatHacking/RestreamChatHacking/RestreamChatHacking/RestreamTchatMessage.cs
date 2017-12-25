namespace RestreamChatHacking
{
    public class RestreamTchatMessage
    {

        public string Id {
            get { return When + "|" + UserName; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _when;

        public string When
        {
            get { return _when; }
            set { _when = value; }
        }


        public enum TchatPlatform : int {
            RestreamInitMessage=0,
            Twitch = 1,
            Youtube = 5,
            Facebook = 37,
            Restream =100,
            Discord=1001
        }
        private TchatPlatform _platformId;

        public TchatPlatform Platform
        {
            get { return _platformId; }
            set { _platformId = value; }
        }

        public void SetPlatform(TchatPlatform platform) {
            _platformId = platform;
        }
        public void SetPlatform(int platformId) {
            _platformId = (TchatPlatform) platformId;
        }

        public override string ToString()
        {
            return string.Format("{0}({1},{2}):{3}", When, UserName, Platform, Message);
        }

    }
}