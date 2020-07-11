using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RestreamMessagesCount : MonoBehaviour
{


    public int m_twitch = 0;
    public int m_youtube    = 0;
    public int m_facebook   = 0;
    public int m_discord    = 0;
    public int m_periscope  = 0;
    public int m_dlive      = 0;
    public int m_linkedin = 0;
    public int m_allCount = 0;

    public Text m_txtTwitch = null;
    public Text m_txtYoutube = null;
    public Text m_txtFacebook = null;
    public Text m_txtDiscord = null;
    public Text m_txtPeriscope = null;
    public Text m_txtDlive = null;
    public Text m_txtLinkedin = null;
    public Text m_txtAllCount = null;


    public void AddToMessageCount(RestreamChatMessage message) {

        ChatPlatform platform = message.Platform;
        switch (platform)
        {
           
            case ChatPlatform.Twitch:
                m_twitch++;
                if (m_txtTwitch != null)
                    m_txtTwitch.text = "" + m_twitch;
                break;
            case ChatPlatform.Youtube:
                m_youtube++;
                if (m_txtYoutube != null)
                    m_txtYoutube.text = "" + m_youtube;
                break;
            case ChatPlatform.Facebook:
                m_facebook++;
                if (m_txtFacebook != null)
                    m_txtFacebook.text = "" + m_facebook;
                break;
            case ChatPlatform.Discord:
                m_discord++;
                if (m_txtDiscord != null)
                    m_txtDiscord.text = "" + m_discord;
                break;
            case ChatPlatform.LinkedIn:
                m_linkedin++;
                if (m_txtLinkedin != null)
                    m_txtLinkedin.text = "" + m_linkedin;
                break;
            case ChatPlatform.Periscope:
                m_periscope++;
                if (m_txtPeriscope != null)
                    m_txtPeriscope.text = "" + m_periscope;
                break;
            case ChatPlatform.DLive:
                m_dlive++;
                if (m_txtDlive != null)
                    m_txtDlive.text = "" + m_dlive;
                break;
            default:
                break;
        }
        m_allCount++;
        if (m_txtAllCount != null)
            m_txtAllCount.text = ""+m_allCount;
    }





}
