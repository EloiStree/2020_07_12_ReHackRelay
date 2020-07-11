using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class UI_RestreamMessageDefault : MonoBehaviour
{
    public RestreamChatMessage m_messageDisplayed;
    public Text m_userName;
    public Text m_message;
    public Text m_platfom;
    public Text m_timeReceived;
    public bool m_hide;

  
    public void CopyInClipboard()
    {
        UnityClipboard.Set(m_messageDisplayed.GetAsOneLiner());
    }


    public void SetWith(RestreamChatMessage message) {
        m_messageDisplayed = message;
        m_hide =! message.IsCorrectlyDefined();
        Refresh();
    }

    public RestreamChatMessage GetStoredMessage()
    {
        return m_messageDisplayed;
    }

    public void Refresh()
    {
        if (m_messageDisplayed == null)
            return;
         m_hide = !m_messageDisplayed.IsCorrectlyDefined();

        if (m_userName!=null)
            m_userName.text = m_hide?"": m_messageDisplayed.UserName;
        if (m_message != null)
            m_message.text = m_hide ? "" : m_messageDisplayed.Message;
        if (m_platfom != null)
            m_platfom.text = m_hide ? "" : m_messageDisplayed.Platform.ToString();
        if (m_timeReceived != null)
            m_timeReceived.text = m_hide ? "" : m_messageDisplayed.When; 


    }
}
