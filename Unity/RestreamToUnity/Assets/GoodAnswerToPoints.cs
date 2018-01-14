using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;
using UnityEngine.UI;

// BAD CODE NOT PROUD !! CHANGE LATER. In Rush need to code dirty
public class GoodAnswerToPoints : MonoBehaviour {
    
    public int _twitchCount;
    public int _youtubeCount;
    public int _facebookCount;

    public Text _twichUI;
    public Text _YoutubeUI;
    public Text _facebookUI;

 
  

    public  void AddPointsToWinners(RestreamChatMessage winnerMessage)
    {
        switch (winnerMessage.Platform)
        {
            case RestreamChatMessage.ChatPlatform.Twitch:
                _twichUI.text = "" + (++_twitchCount);
                break;
            case RestreamChatMessage.ChatPlatform.Youtube:
                _YoutubeUI.text = "" + (++_youtubeCount);
                break;
            case RestreamChatMessage.ChatPlatform.Facebook:
                _facebookUI.text = "" + (++_facebookCount);
                break;
            default:
                break;
        }
    }
    
}
