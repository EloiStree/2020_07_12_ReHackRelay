using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;
using System.Linq;
using System;

public class AntiSpamRestreamFilterDefault : MonoBehaviour
{
    public RestreamMessageUnityEvent m_onValidate;
    public RestreamMessageUnityEvent m_onRefused;

    public Dictionary<string, UserCreditState> m_userSpamCredit = new Dictionary<string, UserCreditState>();
    public int m_maxSpamCredit=100;
    public int m_spamCreditByTik = 10;
    public float m_timeBetweenTik = 10;
    public int m_consumedByMessage = 20;

    [Header("Debug")]
    public List<UserCreditState> m_listOfSpammers = new List<UserCreditState>();

    public IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenTik);
            AddSpamCredit();
        }
    }
    public void AddSpamCredit() {

        string [] users = m_userSpamCredit.Keys.ToArray();
        m_listOfSpammers.Clear();
        for (int i = 0; i < users.Length; i++)
        {
            int value = m_userSpamCredit[users[i]].GetCredits() ;
            value += m_spamCreditByTik;
            if (value > m_maxSpamCredit)
                value = m_maxSpamCredit;
            m_userSpamCredit[users[i]].SetCredits(value);
            if (value <= 0)
                m_listOfSpammers.Add(m_userSpamCredit[ users[i]]);
        }
        
    }

    public void TryToPush(RestreamChatMessage message) {

        int credit = GetUserCredit(message.UserID);
        bool isSpammer = credit <= 0;
        if (isSpammer)
        {
            m_onRefused.Invoke(message);
        }
        else { 
            m_onValidate.Invoke(message);
        }
        credit -= m_consumedByMessage;
        SetUserCredit(message.UserID, credit);


    }

    private void SetUserCredit(UserIdentifier userID, int credit)
    {

        string id = userID.GetID();
        if (!m_userSpamCredit.ContainsKey(id))
            m_userSpamCredit.Add(id, new UserCreditState(id,  m_maxSpamCredit));

        if (credit > m_maxSpamCredit)
            credit = m_maxSpamCredit;
        m_userSpamCredit[id].SetCredits(credit);
    }

    private int GetUserCredit(UserIdentifier userID)
    {
        string id = userID.GetID();
        if (!m_userSpamCredit.ContainsKey(id))
            m_userSpamCredit.Add(id, new UserCreditState(id, m_maxSpamCredit));
        return m_userSpamCredit[id].GetCredits();
    }

    [Serializable]
    public class UserCreditState {
        public UserCreditState(string name, int creditsStart) {
            m_name = name;
            m_credits = creditsStart;
        }
        [SerializeField] string m_name;
        [SerializeField] int m_credits;

        public int GetCredits()
        {
            return m_credits;
        }

        public void SetCredits(int value)
        {
            m_credits = value;
        }
    }
}
