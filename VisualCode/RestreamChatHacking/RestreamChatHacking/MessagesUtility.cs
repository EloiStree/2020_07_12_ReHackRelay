using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    class MessagesUtility
    {

        public static void MaxMessagesSize(ref RestreamChatMessage message, int maxSize=250) 
         {
            if(message.Message.Length >=maxSize)
                message.Message = message.Message.Substring(0, maxSize);
         }  
             
        
    }
}
