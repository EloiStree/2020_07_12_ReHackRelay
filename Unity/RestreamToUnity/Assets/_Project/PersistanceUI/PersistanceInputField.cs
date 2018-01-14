using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistanceInputField : MonoBehaviour {


    public string _fieldId;
    public InputField _fieldTracked;
	// Use this for initialization
	void Awake ()
    {
        if (!IsDefined())
            return;
        Import();
        _fieldTracked.onEndEdit.AddListener(Record);

    }

    private void Record(string text)
    {
        if (!IsDefined())
            return;
        PlayerPrefs.SetString(_fieldId, text);
    }
    private void Import()
    {
        if (!IsDefined())
            return;
        _fieldTracked.text = PlayerPrefs.GetString(_fieldId, _fieldTracked.text);
    }
   



    private bool IsDefined()
    {
        return !string.IsNullOrEmpty(_fieldId);
    }
    private void OnValidate()
    {
        CheckForInputField();
    }
    private void Reset()
    {
        CheckForInputField();

    }
    private void CheckForInputField()
    {
        if (_fieldTracked == null) {
            _fieldTracked = GetComponent<InputField>();
        if (string.IsNullOrEmpty(_fieldId) && _fieldTracked != null)
            _fieldId = "" + _fieldTracked.GetInstanceID();
        }
    }

}
