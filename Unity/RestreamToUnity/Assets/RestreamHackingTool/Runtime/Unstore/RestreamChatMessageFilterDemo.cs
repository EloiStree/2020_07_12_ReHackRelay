using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestreamChatMessageFilterDemo : MonoBehaviour
{
    public RestreamMessageUnityEvent m_onAllowed;
    public RestreamMessageUnityEvent m_onRefused;
    public string[] m_lookingFor = new string[] { "#Question", "#Q" };
    public SearchType m_filterType;
    public enum SearchType { All, AtLeastOne, NoneOf}
    public bool m_ignoreCase;


    public void TryToPush(RestreamChatMessage message) {

        string messageToFilter = m_ignoreCase? message.Message.ToLower(): message.Message;

        bool oneIsNotThere=false;
        bool oneIsThere=false;
        
        for (int i = 0; i < m_lookingFor.Length; i++)
        {
            string toLook = m_ignoreCase ? m_lookingFor[i].ToLower():m_lookingFor[i];
            bool isInIt = messageToFilter.IndexOf(toLook)>-1;
            if (isInIt)
                oneIsThere = true;
            else 
                oneIsNotThere = true;
        }

        if (m_filterType == SearchType.All && !oneIsNotThere)
        {
            m_onAllowed.Invoke(message);
        }

        else if (m_filterType == SearchType.AtLeastOne && oneIsThere)
        {
            m_onAllowed.Invoke(message);
        }
        else if (m_filterType == SearchType.NoneOf && !oneIsThere)
        {
            m_onAllowed.Invoke(message);
        }
        else m_onRefused.Invoke(message);


    }
}
