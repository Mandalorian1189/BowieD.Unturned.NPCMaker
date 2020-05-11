﻿using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItem : Reward
    {
        public override RewardType Type => RewardType.Item;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Item"]} {ID} x{Amount}";
        public ushort ID { get; set; }
        public byte Amount { get; set; }
        [RewardOptional(0, 0)]
        public ushort Sight { get; set; }
        [RewardOptional(0, 0)]
        public ushort Tactical { get; set; }
        [RewardOptional(0, 0)]
        public ushort Grip { get; set; }
        [RewardOptional(0, 0)]
        public ushort Barrel { get; set; }
        [RewardOptional(0, 0)]
        public ushort Magazine { get; set; }
        [RewardOptional(0, 0)]
        public byte Ammo { get; set; }
        public bool Auto_Equip { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Items.Add(new Simulation.Item()
            {
                Amount = (byte)(Ammo == 0 ? 1 : Ammo),
                ID = ID,
                Quality = 100
            });
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Item");
            }
            return string.Format(text, Amount, ID);
        }
    }
}
