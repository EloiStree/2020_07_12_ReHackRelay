using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class UsersBuyRequestManager : MonoBehaviour {

    public UsersBank _linkedBank;

    [Header("Params")]
    [SerializeField]
    private BuyablePrefab [] _buyablePrefabs;
    

    [Header("Event")]
    [SerializeField]
    public PrefabBoughtEvent _onPrefabBought;
    [Serializable]
    public class PrefabBoughtEvent : UnityEvent<string, long, GameObject, RestreamChatMessage> { }

    [Header("Debug")]
    public bool _debug;

    [Serializable]
    public class BuyablePrefab
    {
        public string _idName;
        public long _cost;
        public GameObject _prefab;
    }
    


    void Awake () {
        Restream.AddNewMessageListener(MessageToObjectPop);
        _onPrefabBought.AddListener(DebugDisplay);
    }

    private void DebugDisplay(string arg0, long arg1, GameObject arg2, RestreamChatMessage msg)
    {
        if (_debug)
            Debug.Log(string.Format("Object bought({0}) for {1}$ by {2} ", arg0, arg1, msg.UserName ),arg2);
    }

    private void MessageToObjectPop(RestreamChatMessage newMessage)
    {

        // Is prefix of object present ?
        
        string message = newMessage.Message;

        for (int i = 0; i < _buyablePrefabs.Length; i++)
        {
            int messageIndex = message.IndexOf(_buyablePrefabs[i]._idName);
            if (messageIndex > -1)
            {
                //arguments = trimMessage.Substring(messageIndex + _buyablePrefabs[i]._idName.Length);
                if (_linkedBank.Use(newMessage.UserID, _buyablePrefabs[i]._cost)) {
                    
                    _onPrefabBought.Invoke(_buyablePrefabs[i]._idName, _buyablePrefabs[i]._cost, _buyablePrefabs[i]._prefab, newMessage);
                }
            }
        }
       
    }
    

    public void OnValidate()
    {
        if (_linkedBank == null)
            _linkedBank = FindObjectOfType<UsersBank>();
    }
}
