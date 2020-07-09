using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_OSC2Text : MonoBehaviour {

    public OSC oscRef;
    public Text _lastAddress;
    public Text _lastValues;

	// Use this for initialization
	void Start () {
        oscRef.SetAllMessageHandler(OSCToTextDisplay);
	}

    private void OSCToTextDisplay(OscMessage oscM)
    {
        string totalVal = "";
        foreach (var val in oscM.values)
        {
            totalVal += val.ToString();
        }
        _lastAddress.text = oscM.address;
        _lastValues.text = totalVal;
    }
    
}
