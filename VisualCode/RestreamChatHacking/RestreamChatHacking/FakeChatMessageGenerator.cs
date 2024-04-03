using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{

    public class FakeChatMessageGenerator
    {



        public static  RestreamChatMessage CreateFakeMessage(string userName, string message, ChatPlatform mockup)
        {
            RestreamChatMessage chatMessage = new RestreamChatMessage(userName, message);
            chatMessage.SetPlatform(mockup);
            chatMessage.SetDateToNow();
            return chatMessage;
        }
    }
}
