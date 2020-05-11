﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionQuest : Condition
    {
        public override Condition_Type Type => Condition_Type.Quest;
        public ushort ID { get; set; }
        public Quest_Status Status { get; set; }
        public Logic_Type Logic { get; set; }
        public override string UIText
        {
            get
            {
                string outp = LocalizationManager.Current.Condition[$"Type_Quest"] + $" [{ID}] ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                }
                outp += LocalizationManager.Current.Condition[$"Quest_Status_{Status}"];
                return outp;
            }
        }

        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                switch (simulation.GetQuestStatus(ID))
                {
                    case Quest_Status.Completed:
                        {
                            simulation.Flags.Remove(ID);
                        }
                        break;
                    case Quest_Status.Active:
                        {
                            simulation.Quests.Remove(ID);
                        }
                        break;
                    case Quest_Status.Ready:
                        {
                            simulation.Quests.Remove(ID);
                            simulation.Flags[ID] = 1;

                            NPCQuest questAsset = MainWindow.CurrentProject.data.quests.Single(d => d.id == ID);

                            foreach (Condition c in questAsset.conditions)
                            {
                                c.Apply(simulation);
                            }

                            foreach (Rewards.Reward r in questAsset.rewards)
                            {
                                r.Give(simulation);
                            }
                        }
                        break;
                    default:
                    case Quest_Status.None:
                        break;
                }
            }
        }
        public override bool Check(Simulation simulation)
        {
            return SimulationTool.Compare(simulation.GetQuestStatus(ID), Status, Logic);
        }
    }
}
