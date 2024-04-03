using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Linq;


namespace RestreamChatHacking
{
    /// The role of this class:
    /// - to store the messages as instance
    /// - dectect new messages
    /// - deal possible stream of 2000 active participants
    public class RamMemoryRegisterOfMessages
    {

        public DateTime m_startOfPoint = DateTime.Now;
//        public Queue<RestreamChatMessage> m_messagesInMemory = new Queue<RestreamChatMessage>();
        public Dictionary<string, long> m_stringIdArchive = new Dictionary<string, long>();
        public StringBuilder m_archivedMessages = new StringBuilder();

        public void AddMessageToRegister(ref RestreamChatMessage message,out  bool wasPresent)
        {
            string id = message.GetUniqueId();
            wasPresent = m_stringIdArchive.ContainsKey(id);
            if (!wasPresent)
                m_stringIdArchive.Add(id,(long)(DateTime.Now- m_startOfPoint).TotalSeconds);//Should and the timestamp
        }


        public void  GetMemoryInEstimationUsed(out long charCount, out long memoryBigEstimationInBytes) {
            List<string> keys = m_stringIdArchive.Keys.ToList() ;
            charCount = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                charCount += keys[i].Length;
            }
            memoryBigEstimationInBytes = charCount * 4 + keys.Count * 8;
        }

        public void AddMessagesAndRecovertNewInList(ref List<RestreamChatMessage> foundMessageInPage, out List<RestreamChatMessage> newMessageInPage)
        {
            newMessageInPage = new List<RestreamChatMessage>();
            bool wasInRecord;
            for (int i = 0; i < foundMessageInPage.Count; i++)
            {
                RestreamChatMessage refMessage = foundMessageInPage[i];
                AddMessageToRegister(ref refMessage, out  wasInRecord);
                if (!wasInRecord) { 
                    newMessageInPage.Add(refMessage);
                }

            }
        }

    }
}