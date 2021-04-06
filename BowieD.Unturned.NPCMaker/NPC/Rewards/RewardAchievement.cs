﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardAchievement : Reward
    {
        public override RewardType Type => RewardType.Achievement;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Achievement"]} [{ID}]";
        public string ID { get; set; }

        public override void Give(Simulation simulation) { }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToText();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteString(ID);
        }
    }
}
