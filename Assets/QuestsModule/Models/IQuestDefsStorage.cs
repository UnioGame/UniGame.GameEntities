using GBG.Modules.Quests.Data;
using System.Collections.Generic;

namespace GBG.Modules.Quests
{
    public interface IQuestDefsStorage
    {
        QuestDef GetQuestDef(string questId);
        PlayMakerFSM InstantiateFSM(string defId, string questId);
        List<string> GetAllQuestIds();
        float QuestWeightFunction(string questId);
    }
}