using System;

namespace GBG.Modules.Quests
{
    [Serializable]
    public class QuestDef
    {
        public string Id;
        public string Description;
        public int Weight = 1;
        public PlayMakerFSM QuestFsm;
    }
}