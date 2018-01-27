using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;

public class FileUpdateTracker : MonoBehaviour {

    [Serializable]
    public class FileChangedEvent : UnityEvent<string> {}
    public string _filePath;
    [Header("Event")]
    [SerializeField]
    public FileChangedEvent _onFirstLoad;
    [SerializeField]
    public FileChangedEvent _onFileChanged;


    [Header("Debug (Don't Touch)")]
    public string _lastTimeWrited;
    public bool _isUpdated = true;
    public bool _fileUnreadable = false;

    IEnumerator Start () {
        bool firstLoad=true;
        while (true) {

            if (File.Exists(_filePath)) { 
                string lastTimeFileModified = "" + DateTimeToUnixTimestamp(File.GetLastWriteTimeUtc(_filePath)); ;

                if (lastTimeFileModified != _lastTimeWrited) {
                    _lastTimeWrited = lastTimeFileModified;
                    _isUpdated = false;
                }
                if (!_isUpdated) {
                    string fileContent=null;
                    try {
                        fileContent= File.ReadAllText(_filePath);

                        if (firstLoad && !string.IsNullOrEmpty(fileContent))
                            _onFirstLoad.Invoke(fileContent);
                        else if (!string.IsNullOrEmpty(fileContent))
                            _onFileChanged.Invoke(fileContent);

                        _fileUnreadable = false;
                        _isUpdated = true;
                        firstLoad = false;
                    }
                    catch (IOException)
                    {
                        _fileUnreadable = true;
      //                  Debug.Log("File unreadabe");
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        
	}

    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
               new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
    }

    public void SetPath(string path) {
        _filePath = path;
    }
    //
}
