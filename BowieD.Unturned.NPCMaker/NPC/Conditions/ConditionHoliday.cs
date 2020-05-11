﻿using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionHoliday : Condition
    {
        public ENPCHoliday Value { get; set; }
        public override Condition_Type Type => Condition_Type.Holiday;
        public override string UIText => LocalizationManager.Current.Condition["Type_Holiday"] + " " + LocalizationManager.Current.Condition[$"Holiday_Value_{Value}"];

        public override void Apply(Simulation simulation)
        {

        }
        public override bool Check(Simulation simulation)
        {
            return simulation.Holiday == Value;
        }
    }
}
