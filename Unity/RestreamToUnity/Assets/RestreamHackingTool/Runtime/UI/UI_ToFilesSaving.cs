using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToFilesSaving : MonoBehaviour
{
    public InputField m_directoryPath;
    public Toggle m_isOn;


    public void SaveIfRequested(RestreamChatMessage message) {
        try
        {
            if (!m_isOn.isOn)
                return;
            if (string.IsNullOrEmpty(m_directoryPath.text))
                return;
            if (!Directory.Exists(m_directoryPath.text))
                Directory.CreateDirectory(m_directoryPath.text);
            File.WriteAllText(m_directoryPath.text + "\\" + GetFileName(ref message), message.GetAsOneLiner());
        }
        catch (Exception e) {
            Debug.LogWarning(GetFileName(ref message)+"\n"+ e.StackTrace);
        }
    }

    private string GetFileName(ref RestreamChatMessage message)
    {
        string fileTitle = message.Date + "_" + message.When + "_" + message.UserName;
        Regex rgx = new Regex("[^a-zA-Z0-9_]");
        fileTitle = rgx.Replace(fileTitle, "");
        return  fileTitle+".txt";
    }

    public void SetWithAppData(string relativeFolderName) {
        m_directoryPath.text = Application.persistentDataPath + "/" + relativeFolderName;
    }

    public void OpenDirectory() {

        Application.OpenURL(m_directoryPath.text);
    }


    private void Awake()
    {
        m_directoryPath.text= PlayerPrefs.GetString(m_playprefId,"");
        m_isOn.isOn=  PlayerPrefs.GetInt(m_playprefId + "ison", 0)==1;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(m_playprefId, m_directoryPath.text);
        PlayerPrefs.SetInt(m_playprefId+"ison", m_isOn.isOn?1:0);

    }
    public string m_playprefId;

    private void Reset()
    {
        m_playprefId = (UnityEngine.Random.value * 1000000000f)+"_"+ (UnityEngine.Random.value * 1000000000f) ;
    }
}
