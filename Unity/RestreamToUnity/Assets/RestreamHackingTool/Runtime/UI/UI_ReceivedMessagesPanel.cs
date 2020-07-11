using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class UI_ReceivedMessagesPanel : MonoBehaviour
{
    public UI_RestreamMessageDefault m_lastMessage;
    public UI_RestreamMessageDefault [] m_previousMessage;

    private void Awake()
    {
        m_lastMessage.Refresh(); 
        for (int i = 0; i < m_previousMessage.Length; i++)
        {
            m_previousMessage[i].Refresh();
        }
    }

    public void SetWithNewMessageReceived(RestreamChatMessage message) {
        RestreamChatMessage tmpNew = null, tmpOld = m_lastMessage.GetStoredMessage();
        m_lastMessage.SetWith( message);
        tmpNew = tmpOld;
        for (int i = 0; i < m_previousMessage.Length; i++)
        {
            tmpOld = m_previousMessage[i].GetStoredMessage();
            m_previousMessage[i].SetWith(tmpNew);
            tmpNew = tmpOld;
        }

    }

}
