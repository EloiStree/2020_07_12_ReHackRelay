using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeToTextUI : MonoBehaviour {

    public Text _textUI;
	// Use this for initialization
	void Start () {
        InvokeRepeating("Refresh", 0,0.25f);
	}
    void Refresh() {
        if(_textUI)
        _textUI.text = DateTime.Now.ToString("HH:MM:ss");
    }
	
}
