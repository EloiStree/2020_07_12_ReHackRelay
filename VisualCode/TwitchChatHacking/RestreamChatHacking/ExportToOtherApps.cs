using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    public class ExportToOtherApps
    {
        public static SignalMessageDetected m_onMessageDetected;

        public static void ThrowMessages(RestreamChatMessage message) {
            if(m_onMessageDetected!=null)
                m_onMessageDetected(message);
        }

        public static void AddListener(SignalMessageDetected actionOnMessageThrow)
        {
            m_onMessageDetected += actionOnMessageThrow;
        }

        public static void Push(List<RestreamChatMessage> newMessageInPage)
        {
            SortFirstFIFO(ref newMessageInPage);
            for (int i = 0; i < newMessageInPage.Count; i++)
            {
                ThrowMessages(newMessageInPage[i]);
            }
        }
        public static void SortFirstFIFO (ref List<RestreamChatMessage> newMessageInPage)
        {
            newMessageInPage = newMessageInPage.OrderBy(k => k.GetWhenAsDateTime()).ToList();
        }
    }
}
