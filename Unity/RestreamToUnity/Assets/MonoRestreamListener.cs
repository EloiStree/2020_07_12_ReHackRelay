using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RestreamChatHacking;
public class MonoRestreamListener : MonoBehaviour
{
    public RestreamMessageUnityEvent m_onReceivedMessage;
    public Queue<RestreamChatMessage> m_receivedToPushOnUpdate= new Queue<RestreamChatMessage>();
     
    void Start()
    {
        Restream.Listener.AddNewMessageListener(TriggerUnityEventAtUpdate); 
    }
    private void Update()
    {
        while (m_receivedToPushOnUpdate.Count > 0) {
            m_onReceivedMessage.Invoke(m_receivedToPushOnUpdate.Dequeue());
        }
    }

    private void TriggerUnityEventAtUpdate(RestreamChatMessage newMessage)
    {
        m_receivedToPushOnUpdate.Enqueue(newMessage);
    }

    void OnDestroy()
    {

        Restream.Listener.RemoveNewMessageListener(TriggerUnityEventAtUpdate);
    }
}
