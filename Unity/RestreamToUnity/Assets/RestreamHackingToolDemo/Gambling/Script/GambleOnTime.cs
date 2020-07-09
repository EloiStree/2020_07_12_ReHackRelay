using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GambleOnTime : MonoBehaviour {

    public UsersBank _linkedBank;
    public bool _gambleAllowed = true;

    [Header("Parms")]
    public AnimationCurve _winningRationCurve;
    public float _maxSecondPrecision = 32;
    public float _maxRatio = 100;

    [Header("Users")]
    public List<UserGamble> _gambles;

    [System.Serializable]
    public class UserGamble {
        public UserIdentifier _userId;
        public long _gambledValue;
        public float _gameTime;
    }

    [Header("Debug")]
    public bool _debugDisplay=true;

    public void Awake()
    {
        Restream.Listener.AddNewMessageListener(ListenGamble);
    }

    private void ListenGamble(RestreamChatMessage newMessage)
    {
        string message = newMessage.Message;
        if (!message.ToLower().StartsWith("gamble"))
            return;
        string[] tokens = message.Split(' ');
        if (tokens.Length < 3)
            return;

        long moneyGambled;
        float timeEstimated;

        if (!long.TryParse(tokens[1], out moneyGambled))
            return;
        if (!float.TryParse(tokens[2], out timeEstimated))
            return;

        _linkedBank.Use(newMessage.UserID, moneyGambled);
        _gambles.Add(new UserGamble() { _userId = newMessage.UserID,_gambledValue= moneyGambled ,_gameTime= timeEstimated });


    }

    public void Update()
    {

        if(_debugDisplay)
            if (Input.GetKeyDown(KeyCode.L) & Input.GetKey(KeyCode.LeftControl))
            {
                ApplyGambles(); 
            }
    }

    public void ApplyGambles()
    {
        ApplyGambles( Time.timeSinceLevelLoad);
    }
    public void ApplyGambles(float time)
    {
        for (int i = 0; i < _gambles.Count; i++)
        {
           _linkedBank.Add(_gambles[i]._userId, GetGrainFor(time, _gambles[i]._gameTime, _gambles[i]._gambledValue));
        }
    }

    public float GetRatioFor(float time, UserGamble gamble)
    {
        return GetRatioFor(time, gamble._gameTime);
    }
    public float GetRatioFor(float time, UserIdentifier user) {
        for (int i = 0; i < _gambles.Count; i++)
        {
            if (_gambles[i]._userId.GetID() == user.GetID()) {
                return GetRatioFor(time, _gambles[i]._gameTime);
            }
        }
        return 0;
    }

    public long GetGrainFor(float time, float gambleTime, float gambleCost)
    {
        return (long)(GetRatioFor(time, gambleTime) * gambleCost);
    }

    public float GetRatioFor(float time, float gambleTime) {

        float pourcent = Mathf.Clamp01( Mathf.Abs(time - gambleTime) / _maxSecondPrecision );
            
        float ratio = Mathf.Clamp01(_winningRationCurve.Evaluate(pourcent));
        return ratio*_maxRatio/100f;

    }

}
