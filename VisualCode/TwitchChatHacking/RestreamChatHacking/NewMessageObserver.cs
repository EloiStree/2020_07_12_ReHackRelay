using System;
using System.Collections.Generic;

namespace RestreamChatHacking
{
    public class NewMessageObserver
    {
        public List<RestreamChatMessage> m_previous= new List<RestreamChatMessage>();
        public List<RestreamChatMessage> m_current = new List<RestreamChatMessage>();

        public void SetWithLastRefresh(List<RestreamChatMessage> foundMessageInPage)
        {
            m_previous = m_current;
            m_current = foundMessageInPage;
        }

        public void GetNewMessage(out List<RestreamChatMessage> detectAsNewMessage)
        {

            if (m_previous == null || m_previous.Count == 0)
            { 
                detectAsNewMessage = m_current;
                return;
            }
            



            int currentIndexNewMessage = m_current.Count - 1;
            RestreamChatMessage lastPrevious = m_previous[m_previous.Count-1];
            
            ///Look for similarity
            for (int i = m_current.Count-1; i > -1; i--)
            {
                if (AreEquals(lastPrevious, m_current[i])) {
                    currentIndexNewMessage = i;
                    break;
                }
            }
            if (currentIndexNewMessage == 0)
            {
                detectAsNewMessage = m_current;
            }
            else { 
             //For if that not the same message copy past;

            
            }
            if (currentIndexNewMessage == m_current.Count - 1) {
                if (AreEquals(m_previous[m_previous.Count - 1], m_current[currentIndexNewMessage])) {
                    detectAsNewMessage = new List<RestreamChatMessage>();
                    return;
                }
            }



            detectAsNewMessage = new List<RestreamChatMessage>();
            for (int i = currentIndexNewMessage; i < m_current.Count; i++)
            {
                detectAsNewMessage.Add(m_current[i]);
            }





        }

        private bool AreEquals(in RestreamChatMessage lastPrevious, in RestreamChatMessage restreamChatMessage)
        {
            return
                lastPrevious.UserName.Length == restreamChatMessage.UserName.Length && lastPrevious.Message.Length == restreamChatMessage.Message.Length &&
                lastPrevious.UserName == restreamChatMessage.UserName && lastPrevious.Message == restreamChatMessage.Message;
           
        }
    }
}