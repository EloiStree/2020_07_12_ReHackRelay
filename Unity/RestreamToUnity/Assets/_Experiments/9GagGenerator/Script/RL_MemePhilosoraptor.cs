using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;

public class RL_MemePhilosoraptor : MonoBehaviour {


    public MemePhilosoraptor _philosoraptor;
    public void SetMessage (RestreamChatMessage message) {

        if (_philosoraptor != null && message != null)
        {
            string[] tokens = message.Message.Split('|');
            if (tokens.Length==3 && tokens[0].ToLower().Contains("philosoraptor")) 
            _philosoraptor.SetText(tokens[1], tokens[2]);
        }
    }

}
