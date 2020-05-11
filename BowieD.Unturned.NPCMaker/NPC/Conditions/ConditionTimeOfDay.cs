﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionTimeOfDay : Condition
    {
        public int Second { get; set; }
        public Logic_Type Logic { get; set; }
        public override Condition_Type Type => Condition_Type.Time_Of_Day;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Time_Of_Day"]} ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
                        break;
                    case Logic_Type.Greater_Than:
                        sb.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        sb.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        sb.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        sb.Append("<= ");
                        break;
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
                        break;
                }
                sb.Append(Second);
                return sb.ToString();
            }
        }


        public override void Apply(Simulation simulation) { }
        public override bool Check(Simulation simulation)
        {
            return SimulationTool.Compare(simulation.Time, Second, Logic);
        }
        public override string FormatCondition(Simulation simulation)
        {
            if (string.IsNullOrEmpty(Localization))
            {
                return null;
            }

            return string.Format(Localization, SecondToTime(Second));
        }

        public static string SecondToTime(int second)
        {
            int num = second / 3600;
            int num2 = second / 60 - num * 60;
            int num3 = second - num * 3600 - num2 * 60;

            return $"{num:D2}:{num2:D2}:{num3:D2}";
        }
    }
}
