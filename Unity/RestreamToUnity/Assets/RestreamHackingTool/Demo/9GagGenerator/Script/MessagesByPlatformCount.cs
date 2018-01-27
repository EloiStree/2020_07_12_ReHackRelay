using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MessagesByPlatformCount : MonoBehaviour {
    

    void Start () {

        Restream.AddNewMessageListener(CountPlatfomrInteraction);

    }

    [System.Serializable]
    public class PlatformToCount
    {
        public ChatPlatform _platform;
        public int _count;
        public Text _display;

        public void AddPoints(int value)
        {
            _count += value;
            _display.text = "" + _count;
        }
    }

    public PlatformToCount[] _messagesCount = {
        new PlatformToCount() { _platform= ChatPlatform.Twitch},
        new PlatformToCount() { _platform= ChatPlatform.Facebook},
        new PlatformToCount() { _platform= ChatPlatform.Youtube},
        new PlatformToCount() { _platform= ChatPlatform.Discord}
    };





    public void CountPlatfomrInteraction(RestreamChatMessage winnerMessage)
    {
        foreach (PlatformToCount point in _messagesCount)
        {
            if (winnerMessage.Platform == point._platform)
                point.AddPoints(1);


        }
    }
   
}
