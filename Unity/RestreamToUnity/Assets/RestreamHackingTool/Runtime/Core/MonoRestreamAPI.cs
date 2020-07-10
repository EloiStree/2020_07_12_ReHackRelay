using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;

public class MonoRestreamAPI : MonoBehaviour {

    public void AddMessageToApi( RestreamChatMessage message)
    {
        AddMessagesToApi(message);
    }
    public void AddMessagesToApi(params RestreamChatMessage [] messages)
    {
        Restream.AddNewDetectedMessages(messages);
    }
}
