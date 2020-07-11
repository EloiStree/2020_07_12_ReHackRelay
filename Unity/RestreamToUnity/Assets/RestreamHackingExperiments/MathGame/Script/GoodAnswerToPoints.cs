using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;
using UnityEngine.UI;

// BAD CODE NOT PROUD !! CHANGE LATER. In Rush need to code dirty
public class GoodAnswerToPoints : MonoBehaviour {

    [System.Serializable]
    public class PlatformToPoint {
        public ChatPlatform _platform;
        public int _count;
        public Text _display;

        public void AddPoints(int value)
        {
            _count += value;
            _display.text =""+ _count;
        }
    }

    public PlatformToPoint[] _pointsListener = {
        new PlatformToPoint() { _platform= ChatPlatform.Twitch},
        new PlatformToPoint() { _platform= ChatPlatform.Facebook},
        new PlatformToPoint() { _platform= ChatPlatform.Youtube},
        new PlatformToPoint() { _platform= ChatPlatform.Discord}
    };


 
  

    public void AddPointsToWinners(RestreamChatMessage winnerMessage)
    {
        foreach (PlatformToPoint point in _pointsListener)
        {
            if (winnerMessage.Platform == point._platform)
                point.AddPoints(1);

            
        }
    }
    
}
