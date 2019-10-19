using System;

namespace GBG.Modules.Quests
{
    [Serializable]
    public abstract class QuestDef
    {
        public string Id;
        public string Description;
        public PlayMakerFSM QuestFsm;
        public abstract object ConditionDef { get; }
    }
}