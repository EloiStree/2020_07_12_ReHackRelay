using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class StringToRestreamMessage : MonoBehaviour {


    public RestreamTchatMessage[] _messages;
	// Use this for initialization
	public void JsonToMessages (string json) {
        Debug.Log(
            ":> " + json);
        _messages = JsonConvert.DeserializeObject<RestreamTchatMessage[]>(json);

    }

}
