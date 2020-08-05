﻿using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    public class NE_0003 : CharacterMistake
    {
        public NE_0003()
        {
            MistakeName = "NE_0003";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_0003(string displayName, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0003_Desc", displayName, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCCharacter _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.EditorName.Any(d => "<>:\"/\\|?*".Contains(d)))
                {
                    yield return new NE_0003(_char.DisplayName, _char.ID);
                }
            }
        }
    }
}
