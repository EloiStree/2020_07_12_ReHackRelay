using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RestreamChatHacking
{
    [System.Serializable]
    public class ChatHackerConfigurationByJson
    {
        public static ChatHackerConfigurationByJson Instance = new ChatHackerConfigurationByJson();

        [System.Serializable]
        public class Debug
        {
            public bool DisplayMessage = true;

        }

        private Debug m_debug;
        public Debug DebugOption
        {
            get
            {
                if (m_debug == null)
                    m_debug = new Debug();
                return m_debug;
            }
        }
        #region RESTREAM IRC
        public bool IsRestreamDefined() { return !string.IsNullOrEmpty(m_restreamEmbedURL); }
        public string GetRestreamChatURL() { return m_restreamEmbedURL; }
        public string m_restreamEmbedURL;
        #endregion


        //#region FACEBOOK LINKS
        //public bool IsFacebookPostDefined() { return _facebookPostURL.Length > 0; }
        //public string [] GetFacebokPostURL() { return _facebookPostURL; }
        //public string [] _facebookPostURL;

        //#endregion

        #region STORAGE INFO
        public int m_maxMessagesTracked = 30;
        public int MaximumMessagesTracked { get { return m_maxMessagesTracked; } }
        #endregion

        #region FILTER


        public bool m_useMaxiumMessageSize = false;
        private int m_maximumMessageSize = 1024;
        public bool m_useOsc = true;
        public string m_oscAddress = "127.0.0.1";
        public int m_oscPort = 2513;
        public bool m_useUdp = true;
        public string[] m_udpAddressListeners = new string[] { "127.0.0.1" };
        public int m_udpPort = 2512;
        public bool m_useAllMessagesFile = true;
        public bool m_useLastMessagesFile = false;
        public bool m_allowSteamLaunchingFromChat = false;
        public bool m_allowSteamKillingFromChat = false;
        public string m_killStreamWord = "!stopstreaming";
        public string m_startStreamKeyword = "!startstreaming";
        public bool m_sendMockingMessages = false;
        public bool m_launchRestreamWithTheApp=true;
        public int m_timeBetweenChatCheck=500;
        public bool m_hideInterfaceAtStart=true;
        public bool m_isFirstTimeTheAppIsLaunch=true;
        public int m_windowWidth=400;
        public int m_windowHeight=1000;
        public int MaximumMessageSize
        {
            get { return m_maximumMessageSize; }
            set { m_maximumMessageSize = value; }
        }




        #endregion
        #region SAVE AND LOAD
        public static string GetJson()
        {
            return JsonConvert.SerializeObject(Instance ,Formatting.Indented,
           new JsonConverter[] { new StringEnumConverter() });
        }

        public static void SetFromJson(string json)
        {
            ChatHackerConfigurationByJson config = null;
            try
            {
                config = JsonConvert.DeserializeObject<ChatHackerConfigurationByJson>(json);

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }

            if (config != null)
                Instance = config;
        }


        public void SetRestreamChatURL(string url)
        {
            m_restreamEmbedURL = url;
        }

        public bool IsRequestingFakeMessagesToDebug()
        {
            return m_sendMockingMessages;
        }

        public int GetTimeBetweenChatCheckRequrested()
        {
            return m_timeBetweenChatCheck;
        }

        internal bool IsUserRequestToHideInterface()
        {
            return m_hideInterfaceAtStart;
        }

        internal bool IsFirstTimeForUser()
        {
            return m_isFirstTimeTheAppIsLaunch;
        }
        #endregion

    }
}
