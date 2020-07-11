
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using System.IO;

namespace RestreamChatHacking
{
    public delegate void OnRestreamMessageEvent(RestreamChatMessage newMessage);
    [Serializable]
    public class RestreamMessageUnityEvent : UnityEvent<RestreamChatMessage> { }

    public class Restream {

        public class Listener { 
            #region MESSAGE DETECTOR
           


            private static OnRestreamMessageEvent m_onNewMessage;
            public static  void AddNewMessageListener(OnRestreamMessageEvent toDo)
            {
                m_onNewMessage += toDo;
            }
            public static void RemoveNewMessageListener(OnRestreamMessageEvent toDo)
            {
                m_onNewMessage -= toDo;
            }
            public static void NotifyNewMessage(RestreamChatMessage message) {
                if (message != null && m_onNewMessage != null)
                    m_onNewMessage(message);
            }
            #endregion
        }

        #region NOTIFY
        public static RestreamChatMessage CreateInstance(string pseudo, string message, ChatPlatform platform = ChatPlatform.Mockup) {
            RestreamChatMessage msg = new RestreamChatMessage(pseudo, message);
            msg.SetPlatform(platform);
            return msg;
        }

        public static void AddNewDetectedMessages(params RestreamChatMessage[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                Memory.AddMessageToRegister(messages[i]);
            }
        }

        #endregion


        

        public class Access
        {
            public static RestreamChatMessage GetLastMessage() { return Memory.Messages.First(); }
            public static RestreamChatMessage [] GetLastMessages(int count) {
                if (count < 1) count = 1;
                return Memory.Messages.Take(count).ToArray(); }

            public static RestreamChatMessage[] GetMessagesOfUser(string userName)
            {
                return Memory.Messages.Where(p => p.UserName == userName).OrderBy(p => p.Timestamp).ToArray();
            }
            public static RestreamChatMessage[] GetMessagesOfPlatform(ChatPlatform platform)
            {
                return Memory.Messages.Where(p => p.Platform == platform).OrderBy( p =>p.Timestamp).ToArray();
            }
        }

        public class Memory {

            private static int m_maxMemoryMessages =1000;
            private static Queue<RestreamChatMessage> m_registeredMessages = new Queue<RestreamChatMessage>();
            public static RestreamChatMessage LastMessage { get { return m_registeredMessages.Peek(); } }
            public static IEnumerable<RestreamChatMessage> Messages{ get{ return m_registeredMessages; }}

            public static void SetMaxCapacity(int newMessageCapacity) {
                if (newMessageCapacity < 1) newMessageCapacity = 1;
                m_maxMemoryMessages = newMessageCapacity;
            }
            public static int GetMessagesCount() { return m_registeredMessages.Count; }

            public static void AddMessageToRegister(RestreamChatMessage message) {
                if (message == null)
                    return;
                if (m_maxMemoryMessages <= m_registeredMessages.Count) {
                    m_registeredMessages.Dequeue();
                }
                m_registeredMessages.Enqueue(message);
                Restream.Listener.NotifyNewMessage(message);

            }
            

            public static void Clear() {
                m_registeredMessages.Clear();
            }
        }



    }

}
