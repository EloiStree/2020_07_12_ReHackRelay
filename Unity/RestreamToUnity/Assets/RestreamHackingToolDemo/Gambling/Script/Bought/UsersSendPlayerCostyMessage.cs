using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UsersSendPlayerCostyMessage : MonoBehaviour
{

    public UsersBank _linkedBank;

    public string _prefixMessage = "send";
    public string _prefixWarning = "warning";

    [Header("Params")]
    [SerializeField]
    public long _messageCost = 500;
    [SerializeField]
    public long _warningCost = 2000;

    [Header("Event")]
    [SerializeField]
    private MessageBoughtEvent _onMessageBought;
    [SerializeField]
    private MessageBoughtEvent _onWarningBought;
    [Serializable]
    public class MessageBoughtEvent : UnityEvent< long, string, RestreamChatMessage> { }

    [Header("Debug")]
    public bool _debug;


    void Awake()
    {
        Restream.Listener.AddNewMessageListener(MessageToObjectPop);
        _onMessageBought.AddListener(DebugDisplayMessage);
        _onWarningBought.AddListener(DebugDisplayWarning);
    }

    private void DebugDisplayMessage( long arg1, string msg , RestreamChatMessage arg2)
    {
        if (_debug)
            Debug.Log(string.Format("Bought message to player({0}): {1}", arg1, msg));
    }
    private void DebugDisplayWarning( long arg1, string msg, RestreamChatMessage arg2)
    {
        if (_debug)
            Debug.Log(string.Format("Bought warning to player({0}): {1}", arg1, msg));
    }

    private void MessageToObjectPop(RestreamChatMessage newMessage)
    {
        string trimMessage = "";
        string message = newMessage.Message;

        int messageIndex = message.IndexOf(_prefixMessage);
        if (messageIndex > -1)
        {
            trimMessage = message.Substring(messageIndex + _prefixMessage.Length);
            if (_linkedBank.Use(newMessage.UserID, _messageCost))
            _onMessageBought.Invoke(_messageCost, trimMessage, newMessage);
        }

        int warningIndex = message.IndexOf(_prefixWarning);
        if (warningIndex > -1)
        {
            trimMessage = message.Substring(warningIndex + _prefixWarning.Length);
            if (_linkedBank.Use(newMessage.UserID, _warningCost))
                _onMessageBought.Invoke(_warningCost, trimMessage, newMessage);
        }
        
    }

    public void OnValidate()
    {
        if (_linkedBank == null)
            _linkedBank = FindObjectOfType<UsersBank>();
    }
}
