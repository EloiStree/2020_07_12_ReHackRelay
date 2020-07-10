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


    public void SetWith(RestreamChatMessage message) {
        m_messageDisplayed = message;
        Refresh();
    }

    public RestreamChatMessage GetStoredMessage()
    {
        return m_messageDisplayed;
    }

    private void Refresh()
    {
        if (m_messageDisplayed == null)
            return;

        if(m_userName!=null)
            m_userName.text = m_messageDisplayed.UserName;
        if (m_message != null)
            m_message.text = m_messageDisplayed.Message;
        if (m_platfom != null)
            m_platfom.text = m_messageDisplayed.Platform.ToString();
        if (m_timeReceived != null)
            m_timeReceived.text = m_messageDisplayed.When; 


    }
}
