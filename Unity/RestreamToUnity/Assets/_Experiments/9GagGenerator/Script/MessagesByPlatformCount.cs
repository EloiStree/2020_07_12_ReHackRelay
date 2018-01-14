using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessagesByPlatformCount : MonoBehaviour {

    public int _youtubeCount;
    public int _twitchCount;
    public int _facebookCount;

    void Start () {

        Restream.AddNewMessageListener(CountPlatfomrInteraction);

    }

    private void CountPlatfomrInteraction(RestreamChatMessage newMessage)
    {
        switch (newMessage.Platform)
        {
            case RestreamChatMessage.ChatPlatform.Unknow:
                break;
            case RestreamChatMessage.ChatPlatform.RestreamInitMessage:
                break;
            case RestreamChatMessage.ChatPlatform.Twitch:
                _twitchCount++;
                break;
            case RestreamChatMessage.ChatPlatform.Youtube:
                _youtubeCount++;
                break;
            case RestreamChatMessage.ChatPlatform.Facebook:
                _facebookCount++;
                break;
            case RestreamChatMessage.ChatPlatform.Restream:
                break;
            case RestreamChatMessage.ChatPlatform.Discord:
                break;
            default:
                break;
        }

    }

    public void Clear() {
       _youtubeCount= _twitchCount= _facebookCount=0;
    }
}
