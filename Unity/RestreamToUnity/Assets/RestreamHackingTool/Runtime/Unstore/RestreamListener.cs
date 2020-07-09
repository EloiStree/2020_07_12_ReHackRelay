using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;
using System;
using UnityEngine.Events;

public class RestreamListener : MonoBehaviour {

    [Header("Event")]
    public RestreamMessageUnityEvent _onNewMessage;

    [Header("Info")]
    [SerializeField]
    public StringEvent _lastMessage;
    [SerializeField]
    public StringEvent _lastUsername;
    [Serializable]
    public class StringEvent : UnityEvent<string> { }


    void Awake () {
        Restream.Listener.AddNewMessageListener(NotifyNewMessage);
	}
	
	// Update is called once per frame
	void OnDestroy ()
    {
        Restream.Listener.AddNewMessageListener(NotifyNewMessage);

    }

    private void NotifyNewMessage(RestreamChatMessage message)
    {
        _onNewMessage.Invoke(message);
        _lastMessage.Invoke(message.Message);
        _lastUsername.Invoke(message.UserName);
    }
}
