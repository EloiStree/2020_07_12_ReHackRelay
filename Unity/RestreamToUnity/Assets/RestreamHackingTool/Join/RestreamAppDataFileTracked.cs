using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FileUpdateTracker))]
public class RestreamAppDataFileTracked : MonoBehaviour {


    public FileUpdateTracker _fileTracker;

    public string _appDataPath = " ";
    public string _lastMessages = "lastmessages.json";

    
    void Awake () {

        SetAppDataPath();
        SetTracker();
    }
	
    public void Reset()
    {
        SetAppDataPath();
        SetTracker();
    }
    public void OnValidate()
    {
        SetAppDataPath();
        SetTracker();
    }

    private void SetAppDataPath()
    {
        _appDataPath =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RestreamChatHacking";
    }
    private void SetTracker() {
        if (_fileTracker == null)
            _fileTracker=GetComponent<FileUpdateTracker>();

        if (_fileTracker != null)
            _fileTracker.SetPath(GetMessagesPath());
    }

    private string GetMessagesPath()
    {
        return _appDataPath + "\\" + _lastMessages;
    }
}
