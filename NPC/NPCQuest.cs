﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCQuest : IHasDisplayName
    {
        public NPCQuest()
        {
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            title = "";
            description = "";
            guid = Guid.NewGuid().ToString("N");
            comment = "";
        }
        
        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        public string comment;

        public ushort id;
        public List<Condition> conditions;
        public List<Reward> rewards;
        public string title;
        public string description;

        public string DisplayName => $"[{id}] {title}";
    }
}