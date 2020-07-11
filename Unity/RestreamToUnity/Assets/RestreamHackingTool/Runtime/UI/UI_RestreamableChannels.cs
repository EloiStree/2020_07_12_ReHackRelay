using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RestreamableChannels : MonoBehaviour
{
    public string m_twitch = "https://www.twitch.tv/userid";
    public string m_youtube = "https://www.youtube.com/channel/userid/live";
    public string m_facebook= "https://www.facebook.com/userid";
    public string m_discord = "https://discord.gg/discordchannelid";
    public string m_periscope = "https://www.periscope.tv/userid/";
    public string m_dlive= "https://dlive.tv/userid";
    public string m_linkedin = "https://www.linkedin.com/in/userid/";


    public void OpenWebPage(string url) { Application.OpenURL(url); }
    public void OpenTwitchLive()    { OpenWebPage(m_twitch); }
    public void OpenYoutubeLive()   { OpenWebPage(m_youtube); }
    public void OpenFacebookLive()  { OpenWebPage(m_facebook); }
    public void OpenDiscordLive()   { OpenWebPage( m_discord); }
    public void OpenDLiveLive()     { OpenWebPage(m_dlive); }
    public void OpenPeriscopeLive() { OpenWebPage(m_periscope); }
    public void OpenLinkedInLive() { OpenWebPage(m_linkedin); }
    public void OpenAll() {
        OpenTwitchLive();
        OpenYoutubeLive();
        OpenFacebookLive(); 
        OpenDiscordLive();
        OpenDLiveLive();
        OpenPeriscopeLive(); OpenLinkedInLive();
    }
}
