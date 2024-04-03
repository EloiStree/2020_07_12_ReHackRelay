
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
    public static class HTML2Messages
    {

        //
        //<div class="chat-line__message" data-a-target="chat-line-message" data-test-selector="chat-line-message" tabindex="0"><div class="Layout-sc-nxg1ff-0 fcPbos"><div data-test-selector="chat-message-highlight" class="Layout-sc-nxg1ff-0 cgEvHY chat-line__message-highlight"></div><div class="Layout-sc-nxg1ff-0 fcPbos chat-line__message-container"><div class="Layout-sc-nxg1ff-0"><div class="Layout-sc-nxg1ff-0 duJXWu chat-line__no-background"><div class="Layout-sc-nxg1ff-0 pqFci chat-line__username-container"><span><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="1257e6b8666a00b2dc474132c82e1d09"><img alt="Moderator" aria-label="Moderator badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/1" srcset="https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/1 1x, https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/2 2x, https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/3 4x"></button></div><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="8a0843cf7275857aea96aa3706f17140"><img alt="16-Month Subscriber (3-Month Badge)" aria-label="16-Month Subscriber (3-Month Badge) badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/1" srcset="https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/1 1x, https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/2 2x, https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/3 4x"></button></div><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="6ab817307849630cd02c6188755f8a80"><img alt="cheer 100" aria-label="cheer 100 badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/1" srcset="https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/1 1x, https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/2 2x, https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/3 4x"></button></div></span><span class="chat-line__username" role="button" tabindex="0"><span><span class="chat-author__display-name" data-a-target="chat-message-username" data-a-user="paulkappa" data-test-selector="message-username" style="color: rgb(220, 0, 0);">paulkappa</span></span></span></div><span aria-hidden="true" data-test-selector="chat-message-separator">: </span><span class="" data-test-selector="chat-line-message-body"><span class="text-fragment" data-a-target="chat-message-text">giff me mod perms xdd</span></span></div></div></div><div class="Layout-sc-nxg1ff-0 ewbqxu chat-line__reply-icon"><div class="InjectLayout-sc-588ddc-0 ktQueN"><button class="ScCoreButton-sc-1qn4ixc-0 ffyxRu ScButtonIcon-sc-o7ndmn-0 nHKTN InjectLayout-sc-588ddc-0" aria-label="Click to reply" data-test-selector="chat-reply-button" aria-describedby="400ecf85a3df6933362ebeea7a50b151"><div class="ButtonIconFigure-sc-1ttmz5m-0 fbCCvx"><div class="ScIconLayout-sc-1bgeryd-0 cXxJjc"><div class="ScAspectRatio-sc-1sw3lwy-1 kPofwJ tw-aspect"><div class="ScAspectSpacer-sc-1sw3lwy-0 dsswUS"></div><svg width="100%" height="100%" version="1.1" viewBox="0 0 20 20" x="0px" y="0px" class="ScIconSVG-sc-1bgeryd-1 ifdSJl"><path d="M8.5 5.5L7 4L2 9L7 14L8.5 12.5L6 10H10C12.2091 10 14 11.7909 14 14V16H16V14C16 10.6863 13.3137 8 10 8H6L8.5 5.5Z"></path></svg></div></div></div></button></div></div></div></div>
        //<span class="text-fragment" data-a-target="chat-message-text">tulog na </span>
        // <span class="chat-author__display-name" data-a-target="chat-message-username" data-a-user="realbaconbean" data-test-selector="message-username" style="color: rgb(0, 123, 0);">realbaconbean</span>

        public static void GetMessagesInHTML(string html, out List<string> messagesFoundAsHtml, out List <RestreamChatMessage> messagesFound)
        {
            messagesFoundAsHtml = new List<string>();
            messagesFound = new List<RestreamChatMessage>();
            GetMessageItemsInThePage(html, out messagesFoundAsHtml);

            for (int i = 0; i < messagesFoundAsHtml.Count; i++)
            {
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
            //<div class="chat-line__message" data-a-target="chat-line-message" data-test-selector="chat-line-message" tabindex="0"><div class="Layout-sc-nxg1ff-0 fcPbos"><div data-test-selector="chat-message-highlight" class="Layout-sc-nxg1ff-0 cgEvHY chat-line__message-highlight"></div><div class="Layout-sc-nxg1ff-0 fcPbos chat-line__message-container"><div class="Layout-sc-nxg1ff-0"><div class="Layout-sc-nxg1ff-0 duJXWu chat-line__no-background"><div class="Layout-sc-nxg1ff-0 pqFci chat-line__username-container"><span><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="1257e6b8666a00b2dc474132c82e1d09"><img alt="Moderator" aria-label="Moderator badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/1" srcset="https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/1 1x, https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/2 2x, https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/3 4x"></button></div><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="8a0843cf7275857aea96aa3706f17140"><img alt="16-Month Subscriber (3-Month Badge)" aria-label="16-Month Subscriber (3-Month Badge) badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/1" srcset="https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/1 1x, https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/2 2x, https://static-cdn.jtvnw.net/badges/v1/d3e0004b-d870-4fb7-8566-ba5f6e8b2335/3 4x"></button></div><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="6ab817307849630cd02c6188755f8a80"><img alt="cheer 100" aria-label="cheer 100 badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/1" srcset="https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/1 1x, https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/2 2x, https://static-cdn.jtvnw.net/badges/v1/09d93036-e7ce-431c-9a9e-7044297133f2/3 4x"></button></div></span><span class="chat-line__username" role="button" tabindex="0"><span><span class="chat-author__display-name" data-a-target="chat-message-username" data-a-user="paulkappa" data-test-selector="message-username" style="color: rgb(220, 0, 0);">paulkappa</span></span></span></div><span aria-hidden="true" data-test-selector="chat-message-separator">: </span><span class="" data-test-selector="chat-line-message-body"><span class="text-fragment" data-a-target="chat-message-text">giff me mod perms xdd</span></span></div></div></div><div class="Layout-sc-nxg1ff-0 ewbqxu chat-line__reply-icon"><div class="InjectLayout-sc-588ddc-0 ktQueN"><button class="ScCoreButton-sc-1qn4ixc-0 ffyxRu ScButtonIcon-sc-o7ndmn-0 nHKTN InjectLayout-sc-588ddc-0" aria-label="Click to reply" data-test-selector="chat-reply-button" aria-describedby="400ecf85a3df6933362ebeea7a50b151"><div class="ButtonIconFigure-sc-1ttmz5m-0 fbCCvx"><div class="ScIconLayout-sc-1bgeryd-0 cXxJjc"><div class="ScAspectRatio-sc-1sw3lwy-1 kPofwJ tw-aspect"><div class="ScAspectSpacer-sc-1sw3lwy-0 dsswUS"></div><svg width="100%" height="100%" version="1.1" viewBox="0 0 20 20" x="0px" y="0px" class="ScIconSVG-sc-1bgeryd-1 ifdSJl"><path d="M8.5 5.5L7 4L2 9L7 14L8.5 12.5L6 10H10C12.2091 10 14 11.7909 14 14V16H16V14C16 10.6863 13.3137 8 10 8H6L8.5 5.5Z"></path></svg></div></div></div></button></div></div></div></div>
            messagesAsHTmlDiv= new List<string>();
            List<int> indexes = AllIndexesOf(html, "data-a-user=\"");
            //"data-a-user=\""
            //"\"chat-line-message-body\""
            //\"chat-reply-button\"
            Console.WriteLine("Chat Line:" + indexes.Count);
            for (int i = 0; i < indexes.Count; i++)
            {
                if (i < indexes.Count - 1)
                {
                    string es = html.Substring(indexes[i], indexes[i + 1] - indexes[i]);
                    int cut = es.IndexOf("\"chat-reply-button\"");
                    if (cut < 0)
                        messagesAsHTmlDiv.Add(es);
                    else messagesAsHTmlDiv.Add(es.Substring(0, cut));
                }
                else {
                    string es = html.Substring(indexes[i]);
                    int cut  = es.IndexOf("\"chat-reply-button\"");
                    if (cut < 0)
                        messagesAsHTmlDiv.Add(es);
                    else messagesAsHTmlDiv.Add(es.Substring(0, cut));
                }
                
            }
        }
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        private static string RemoveScriptsFrom(string html, string v)
        {
            Regex r = new Regex("<script(.*?)>(.*?)</script>");
            return r.Replace(html,"");
        }

        private static void TryConvertMessageItemHtmlDivTOMessage(string messageRaw, out bool hasFoundOne, out bool hadconvertionError, out RestreamChatMessage messageFound )
        {
            //<span class="text-fragment" data-a-target="chat-message-text">tulog na </span>
            // <span class="chat-author__display-name" data-a-target="chat-message-username"
            // data-a-user="realbaconbean" data-test-selector="message-username"
            // style="color: rgb(0, 123, 0);">realbaconbean</span>



            //"data-a-user=\""                  // Enter important part
            //"\"chat-line-message-body\""      //Text message
            //\"chat-reply-button\"             //End of all

            hasFoundOne = false;
            hadconvertionError = false;
            messageFound = null;

            //Console.WriteLine("Message:A: "+ messageRaw);
            if (messageRaw.IndexOf("data-a-user=\"") < 0) return;
            if (messageRaw.IndexOf("\"chat-line-message-body\"") < 0) return;
              
            //Message:A: <div>chat-line-message" tabindex="0"><div class="Layout-sc-nxg1ff-0 fcPbos"><div data-test-selector="chat-message-highlight" class="Layout-sc-nxg1ff-0 cgEvHY chat-line__message-highlight"></div><div class="Layout-sc-nxg1ff-0 fcPbos chat-line__message-container"><div class="Layout-sc-nxg1ff-0"><div class="Layout-sc-nxg1ff-0 duJXWu chat-line__no-background"><div class="Layout-sc-nxg1ff-0 pqFci chat-line__username-container"><span><div class="InjectLayout-sc-588ddc-0 kUvjun"><button data-a-target="chat-badge" aria-describedby="f5300d8a5aaf84281adcd21df6d9028b"><img alt="Listening only" aria-label="Listening only badge" class="chat-badge" src="https://static-cdn.jtvnw.net/badges/v1/199a0dba-58f3-494e-a7fc-1fa0a1001fb8/1" srcset="https://static-cdn.jtvnw.net/badges/v1/199a0dba-58f3-494e-a7fc-1fa0a1001fb8/1 1x, https://static-cdn.jtvnw.net/badges/v1/199a0dba-58f3-494e-a7fc-1fa0a1001fb8/2 2x, https://static-cdn.jtvnw.net/badges/v1/199a0dba-58f3-494e-a7fc-1fa0a1001fb8/3 4x"></button></div></span><span class="chat-line__username" role="button" tabindex="0"><span><span class="chat-author__display-name" data-a-target="chat-message-username" data-a-user="lessmat" data-test-selector="message-username" style="color: rgb(138, 43, 226);">lessmat</span></span></span></div><span aria-hidden="true" data-test-selector="chat-message-separator">: </span><span class="" data-test-selector="</div>
            //Console.WriteLine("Message:B");
            try
            {
                messageFound = new RestreamChatMessage();
                messageFound.SetDateToNow();
                messageFound.Message = FetchTextIn(messageRaw);// 
                messageFound.UserName = FetchNameIn(messageRaw);// GetValueOfBetween(messageRaw, "data-a-user=\"", "</span>");  
                
                messageFound.SetPlatform(ChatPlatform.Twitch);
                hasFoundOne = messageFound.IsValuesDefined();
                hadconvertionError = false;
                //Console.WriteLine("Message ----> #"+ messageFound.UserName+ "#--#" + messageFound.Message+ "#");
            }
            catch (Exception ) {
                messageFound = null;
                hasFoundOne = false;
                hadconvertionError = true;
            }
        }

        private static string FetchNameIn(string messageRaw)
        {
            int indexStart=messageRaw.IndexOf( "data-a-user=\"");
            if (indexStart < 0) return "";
            string s = messageRaw.Substring(indexStart + "data-a-user=\"".Length);

            int indexEnd = s.IndexOf("\"");
            if (indexEnd < 0) return "";
            return s.Substring(0, indexEnd);

        }

        private static string FetchTextIn(string messageRaw)
        {
            //data-a-target="chat-message-text"
            return GetValueOfBetween(messageRaw, "data-a-target=\"chat-message-text\"","</span>"); 
        }

        private static string GetValueOfBetween(string messageRaw, string start, string end)
        {
            int startIndex = messageRaw.IndexOf(start);
            if (startIndex < 0) return "";
            messageRaw = messageRaw.Substring(startIndex+ start.Length);

            int endIndex = messageRaw.IndexOf(end);
            if (endIndex < 0) return "";
            messageRaw = messageRaw.Substring(0, endIndex);
            startIndex = messageRaw.IndexOf(">");
            if (startIndex < 0) return "";
            return  messageRaw.Substring(startIndex);
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