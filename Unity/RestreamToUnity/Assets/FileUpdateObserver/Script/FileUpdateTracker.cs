using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;

public class FileUpdateTracker : MonoBehaviour {

    [Serializable]
    public class FileChangedEvent : UnityEvent<string> {}
    public string _pathToTrack;
    [Header("Event")]
    [SerializeField]
    public FileChangedEvent _onFileChanged;


    [Header("Debug")]
    public string _lastTimeWrited;
    public bool _isUpdated = true;
    public bool _fileUnreadable = false;

    IEnumerator Start () {
        while (true) {
            string lastTimeFileModified = "" + DateTimeToUnixTimestamp(File.GetLastWriteTimeUtc(_pathToTrack)); ;

            if (lastTimeFileModified != _lastTimeWrited) {
                _lastTimeWrited = lastTimeFileModified;
                _isUpdated = false;
            }
            if (!_isUpdated) {
                string fileContent=null;
                try {
                    fileContent= File.ReadAllText(_pathToTrack);
                    _fileUnreadable = false;
                    _isUpdated = true;
                }
                catch (IOException)
                {
                    _fileUnreadable = true;
  //                  Debug.Log("File unreadabe");
                }
                if(!string.IsNullOrEmpty(fileContent))
                 _onFileChanged.Invoke(fileContent);
            }

            yield return new WaitForEndOfFrame();
        }
        
	}

    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
               new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
    }
    //
}
