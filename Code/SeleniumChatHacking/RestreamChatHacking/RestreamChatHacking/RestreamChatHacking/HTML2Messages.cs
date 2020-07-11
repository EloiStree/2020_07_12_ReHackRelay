
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Linq;
using Bonsai.Expressions;
using System.IO;

namespace RestreamChatHacking
{
    public class HTML2Messages
    {

        public static void GetMessagesInHTML(string html, out List<string> messagesFoundAsHtml, out List <RestreamChatMessage> messagesFound)
        {
            messagesFoundAsHtml = new List<string>();
            messagesFound = new List<RestreamChatMessage>();
            GetMessageItemsInThePage(html, out messagesFoundAsHtml);

            for (int i = 0; i < messagesFoundAsHtml.Count; i++)
            {
 //               string messageRaw = "<div>" + messagesFoundAsHtml[i].GetAttribute("innerHTML") + "</div>";
                string messageRaw = "<div>" + messagesFoundAsHtml[i]+ "</div>";
                bool foundMessage;
                bool hasConvertionError;
                RestreamChatMessage messageFound;
                TryConvertMessageItemHtmlDivTOMessage(messageRaw, out foundMessage, out hasConvertionError, out messageFound);
                if (foundMessage & !hasConvertionError)
                    messagesFound.Add(messageFound);
            }
        }

        private static void GetMessageItemsInThePage(string html, out List<string> messagesAsHTmlDiv)
        {
            messagesAsHTmlDiv = new List<string>();
            int bodyStart = html.ToLower().LastIndexOf("<body>");
            int bodyEnd = html.ToLower().LastIndexOf("</body>");
            if (bodyStart < 0) return;
            if (bodyEnd < 0) return;
            bodyEnd += "</body>".Length;
            html = "<html>"+ html.Substring(bodyStart, bodyEnd - bodyStart)+ "</html>";
            html = CorrectImagesFormatXMLFrom(html, "");
            html = RemoveScriptsFrom(html, "");
            var xPathDoc = new XPathDocument(new StringReader(html));
            var nav = xPathDoc.CreateNavigator();
            bool hasNext = true;
            int antiLoop = 50;
            var sender = nav.Select("//*[contains(@class, 'message-item')]");
            while (hasNext && antiLoop>0)
            {
                hasNext = sender.MoveNext();
                 //Console.Out.WriteLine("\n\nElement:\n" + sender.Current.InnerXml);
                messagesAsHTmlDiv.Add(sender.Current.InnerXml);
                antiLoop--;
            }

        }

        private static string RemoveScriptsFrom(string html, string v)
        {
            Regex r = new Regex("<script(.*?)>(.*?)</script>");
            return r.Replace(html,"");
        }

        private static void TryConvertMessageItemHtmlDivTOMessage(string messageRaw, out bool hasFoundOne, out bool hadconvertionError, out RestreamChatMessage messageFound )
        {
            hasFoundOne = false;
            hadconvertionError = false;
            messageFound = null;
            if (messageRaw.IndexOf("message-text") < 0) return;
            if (messageRaw.IndexOf("message-sender") < 0) return;
            if (messageRaw.IndexOf("message-time") < 0) return;

            try
            {
                messageFound = new RestreamChatMessage();
                messageFound.SetDateToNow();
                messageFound.Message = GetValueOf(messageRaw, "message-text");
                messageFound.UserName = GetValueOf(messageRaw, "message-sender");
                messageFound.When = GetValueOf(messageRaw, "message-time");
                messageFound.SetPlatform(GetPlatformId(messageRaw));
                hasFoundOne = messageFound.IsValuesDefined();
                hadconvertionError = false;
            }
            catch (Exception ) {
                messageFound = null;
                hasFoundOne = false;
                hadconvertionError = true;
            }
        }

        private static  ChatPlatform GetNumberIn(string text)
        {
            text = text.ToLower();
            if (text.IndexOf("restream-icon-orange.svg") >= 0)
                return ChatPlatform.Restream;
            if (text.IndexOf("platform-1001.png") >= 0)
                return ChatPlatform.Discord;
            if (text.IndexOf("platform-37.png") >= 0)
                return ChatPlatform.Facebook;
            if (text.IndexOf("platform-38.png") >= 0)
                return ChatPlatform.Periscope;
            if (text.IndexOf("platform-57.png") >= 0)
                return ChatPlatform.DLive;
            //if (text.IndexOf("platform-xxx.png") >= 0)
            //    return RestreamChatMessage.ChatPlatform.LinkedIn;
            if (text.IndexOf("platform-1.png") >= 0)
                return ChatPlatform.Twitch;
            if (text.IndexOf("platform-25.png") >= 0 ||
                text.IndexOf("platform-5.png") >= 0)
                return ChatPlatform.Youtube;
            return ChatPlatform.Unknow;
        }

        private static string GetValueOf(string messageRaw, string attribute)
        {
            string messageRawWithoutImage = RemoveImagesFrom(messageRaw, "");

            try
            {

                string value;
                var xPathDoc = new XPathDocument(new StringReader(messageRawWithoutImage));
                var nav = xPathDoc.CreateNavigator();
                var sender = nav.Select("//*[contains(@class, '" + attribute + "')]");
                sender.MoveNext();
                value = sender.Current.Value;
                return value;
            }
            catch (Exception e)
            {
                if (ChatHackerConfiguration.Instance.DebugOption.DisplayMessage)
                    Console.WriteLine("" + messageRaw);
                    Console.WriteLine("############################# Message ###################################");
                    Console.WriteLine("" + messageRaw);
                    Console.WriteLine("############################# EXCEPTION ###################################");
                    Console.Error.WriteLine(e);
                    throw new MessageNotReadableException();
            }
            finally
            {
            }
        }
        private static ChatPlatform GetPlatformId(string messageRaw)
        {
            string messageRawWithoutImage = RemoveImagesFrom(messageRaw, "");
            return GetNumberIn(messageRaw);

        }

        public static string urlInSrcPatter = "src\\s*=\\s*\"(.+?)\"";
        //        public static string htmlImageTagPattern = "<img\\s[^>]*?src\\s*=\\s*['\\\"]([^'\\\"]*?)['\\\"][^>]*?>";
        public static string htmlImageTagPattern = "<img[^>]*>";
        private static string RemoveImagesFrom(string messageRaw, string replacement = "[img]")
        {

            return Regex.Replace(messageRaw, htmlImageTagPattern, replacement);
        }
        private static string CorrectImagesFormatXMLFrom(string messageRaw, string replacement = "[img]")
        {
            MatchCollection mc= Regex.Matches(messageRaw, htmlImageTagPattern);

            for (int i = 0, l = mc.Count; i < l; i++)
            {
                string oldValue = mc[i].Value;

                string newValue = "";

                if (oldValue[oldValue.Length - 2] != '/') { 
                    newValue = oldValue.Substring(0, oldValue.Length - 1) + "/>";
                    messageRaw= messageRaw.Replace(oldValue, newValue);
                }
            }

            return messageRaw;
        }
        private static bool FindImagesHtmlCode(string messageRaw, out string[] imagesFound)
        {

            List<string> results = new List<string>();
            foreach (Match match in Regex.Matches(messageRaw, htmlImageTagPattern))
            {
                // Console.WriteLine("Found '{0}' at position {1}",match.Value, match.Index);
                results.Add(match.Value);
            }
            imagesFound = results.ToArray<string>();
            return imagesFound.Length > 0;
        }

    }
}