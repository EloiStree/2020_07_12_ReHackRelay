using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class RestreamMathGame : MonoBehaviour {


    public int _maxValue=99;
    public int _minValue=-99;


    [Header("UI")]
    public Text _equationUI;
    public Text _correctAnswerUI;
    [Header("Debug")]
    public string _equation;
    public float _answer;

    [Header("Debug")]
    [SerializeField]
    public WinnerEvent _onWinnerDetected;
    [Serializable]
    public class WinnerEvent: UnityEvent<RestreamChatMessage> {}

    public enum Operation:int { Add, Sub,  Mod, Multi, Div }

    void Awake () {
        Restream.AddNewMessageListener(CheckForGoodAnswer);
        SetNewEquation();

    }

    private void SetNewEquation() {

        if (!string.IsNullOrEmpty(_equation))
            _correctAnswerUI.text = _equation + " = " + _answer;
        else _correctAnswerUI.text = "";
        int a = UnityEngine.Random.Range(_minValue, _maxValue);
        int b = UnityEngine.Random.Range(_minValue, _maxValue);
        Operation op = (Operation) UnityEngine.Random.Range(0, 2);
        string opString=" ";


        switch (op)
        {
            case Operation.Add: opString = "+"; _answer = a + b;
                break;
            case Operation.Sub: opString = "-"; _answer = a - b;
                break;
            case Operation.Div: opString = "/"; _answer = a / b;
                break;
            case Operation.Multi : opString = "*"; _answer = a * b;
                break;
            case Operation.Mod: opString = "%"; _answer = a % b;
                break;
            default:
                break;
        }
        _equation = string.Format("{0} {2} {1}", a, b, opString);
        _equationUI.text = _equation + " = ?";

    }
    private void CheckForGoodAnswer(RestreamChatMessage newMessage)
    {
        float userAnswer = 0;
        if (float.TryParse(newMessage.Message, out userAnswer)) {
            if (userAnswer == _answer) {
                _onWinnerDetected.Invoke(newMessage);
                SetNewEquation();
            }
        }
        
    }
    
}
