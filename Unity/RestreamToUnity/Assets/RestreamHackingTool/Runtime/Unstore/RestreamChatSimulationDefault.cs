using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;
using System;

public class RestreamChatSimulationDefault : MonoBehaviour
{
    public TextAsset m_namesFile;
    public TextAsset m_sentencesFile;
    public ChatPlatform[] m_platforms;
    public float m_minTime=0.2f;
    public float m_maxTime=3f;
    public RestreamMessageUnityEvent m_emitted;
    public string[] m_names;
    public string[] m_sentences;
    IEnumerator Start()
    {
        m_names = m_namesFile.text.Split('\n');
        m_sentences = m_sentencesFile.text.Split('\n');
        while (true) {

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(UnityEngine.Random.Range(m_minTime, m_maxTime));
            m_emitted.Invoke(new RestreamChatMessage(GetRandomName(), GetRandomMessage(), GetRandomPlatform()));
        }
        
    }

    private ChatPlatform GetRandomPlatform()
    {
        return m_platforms[UnityEngine.Random.Range(0, m_platforms.Length)];
    }

    private string GetRandomMessage()
    {
        return m_sentences[UnityEngine.Random.Range(0, m_sentences.Length)].Replace('\n', ' ').Trim();
    }

    private string GetRandomName()
    {
        return m_names[UnityEngine.Random.Range(0, m_names.Length)].Replace('\n', ' ').Trim();
    }

}
