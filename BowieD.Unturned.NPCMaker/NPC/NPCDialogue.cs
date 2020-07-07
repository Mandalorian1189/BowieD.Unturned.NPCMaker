﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCDialogue : IHasUIText
    {
        public NPCDialogue()
        {
            messages = new List<NPCMessage>();
            responses = new List<NPCResponse>();
            guid = Guid.NewGuid().ToString("N");
            Comment = "";
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute]
        public ushort id;
        public List<NPCMessage> messages;
        [XmlIgnore]
        public int MessagesAmount => messages == null ? 0 : messages.Count;
        [XmlIgnore]
        public int ResponsesAmount => responses == null ? 0 : responses.Count;
        public List<NPCResponse> responses;
        public List<NPCResponse> GetVisibleResponses(NPCMessage message)
        {
            int messageIndex = messages.IndexOf(message);
            if (messageIndex == -1)
            {
                return null;
            }

            return responses.Where(d => d.VisibleInAll || d.visibleIn[messageIndex] == 1).ToList();
        }
        public string UIText
        {
            get
            {
                if (messages == null || messages.Count < 1 || messages[0].pages.Count < 1)
                {
                    return $"[{id}]";
                }
                else
                {
                    string t = messages[0].pages[0];
                    if (!string.IsNullOrEmpty(t))
                    {
                        return TextUtil.Shortify($"[{id}] - {t}", 24);
                    }

                    return $"[{id}]";
                }
            }
        }
    }
}
