using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    class SimulateFakeMessage
    {


        public static Thread m_thread;
        public static void LaunchThread() {
            StopThread();
            if (m_thread == null) {
                m_thread = new Thread(new ThreadStart(UseMockUData));
                m_thread.Start();
            }
        }
        public static void StopThread() {
            if (m_thread != null) {
                m_thread.Abort();
                m_thread = null;
            
            }
        }

        private static void UseMockUData()
        {
            int count = 0;
            while (m_thread!=null)
            {
                RestreamChatMessage chatMessage = FakeChatMessageGenerator.CreateFakeMessage("Fake User", "I am a message test " + count++, ChatPlatform.Mockup);
                ExportToOtherApps.ThrowMessages(chatMessage);
                Thread.Sleep(1000);
            }

        }
    }
}
