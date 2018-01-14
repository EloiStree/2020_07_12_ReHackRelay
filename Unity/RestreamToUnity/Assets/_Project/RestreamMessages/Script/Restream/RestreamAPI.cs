using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

namespace RestreamChatHacking
{

    public class Restream {


        #region MESSAGE DETECTOR
        public delegate void OnMessageAction(RestreamChatMessage newMessage);
        [Serializable]
        public class MessageEvent : UnityEvent<RestreamChatMessage> { }


        private static OnMessageAction _onNewMessage;
        public static  void AddNewMessageListener(OnMessageAction toDo)
        {
            _onNewMessage += toDo;
        }
        public static void RemoveNewMessageListener(OnMessageAction toDo)
        {
            _onNewMessage -= toDo;
        }
        public static void NotifynewMessage(RestreamChatMessage message) {
            if (message != null && _onNewMessage != null)
                _onNewMessage(message);
        }
        #endregion

        #region NOTIFY
        public static RestreamChatMessage Create(string pseudo, string message) {
            RestreamChatMessage msg = new RestreamChatMessage(pseudo, message);

            return msg;
        }

        internal static void AddMessages(params RestreamChatMessage[] messages)
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
            public static RestreamChatMessage[] GetMessagesOfPlatform(RestreamChatMessage.ChatPlatform platform)
            {
                return Memory.Messages.Where(p => p.Platform == platform).OrderBy( p =>p.Timestamp).ToArray();
            }
        }

        public class Memory {

            private static int _maxMemoryMessages =1000;
            private static Queue<RestreamChatMessage> _registeredMessages = new Queue<RestreamChatMessage>();
            public static RestreamChatMessage LastMessage { get { return _registeredMessages.Peek(); } }
            public static IEnumerable<RestreamChatMessage> Messages{ get{ return _registeredMessages; }}

            public static void SetMaxCapacity(int newMessageCapacity) {
                if (newMessageCapacity < 1) newMessageCapacity = 1;
                _maxMemoryMessages = newMessageCapacity;
            }
            public static int GetMessagesCount() { return _registeredMessages.Count; }

            public static void AddMessageToRegister(RestreamChatMessage message) {
                if (message == null)
                    return;
                if (_maxMemoryMessages <= _registeredMessages.Count) {
                    _registeredMessages.Dequeue();
                }
                _registeredMessages.Enqueue(message);
                _onNewMessage.Invoke(message);

            }
            

            public static void Clear() {
                _registeredMessages.Clear();
            }
        }
       
    }

}
