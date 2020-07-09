using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class JsonToRestreamMessage : MonoBehaviour {

    [Serializable]
    public class NewMessageEvent : UnityEvent<RestreamChatMessage> { }

    [Header("Event")]
    [SerializeField]
    public NewMessageEvent _onMessagesDetected;
    public List<RestreamChatMessage> _messages = new List<RestreamChatMessage>();

    [Header("Debug")]
    public bool _useDebug;


    public void Awake()
    {
            _onMessagesDetected.AddListener(DisplayMessage);
    }

    private void DisplayMessage(RestreamChatMessage messages)
    {
        if (_useDebug)
            Debug.Log("> " + messages.Message);
    }

    public void SetMessagesFromJson (string json)
    {
        RestreamChatMessage[] messages = JsonUtility.FromJson<RestreamChatMessage[]>(json);
        _messages = messages.ToList<RestreamChatMessage>();
    }
    public void AddNewMessagesFromJson(string json)
    {
        RestreamChatMessage[] messages = JsonUtility.FromJson<RestreamChatMessage[]>(json);
        RestreamChatMessage[] newMessages = messages.Except(_messages).ToArray();

        for (int i = 0; i < newMessages.Length; i++)
        {

            _messages.Add(newMessages[i]);
            _onMessagesDetected.Invoke(newMessages[i]);

        }

    }

}
