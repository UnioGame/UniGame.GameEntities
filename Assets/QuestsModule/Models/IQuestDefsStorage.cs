using GBG.Modules.Quests.Data;
using System.Collections.Generic;

namespace GBG.Modules.Quests
{
    public interface IQuestDefsStorage
    {
        QuestDef GetQuestDef(string questId);
        IQuestProcessor InstantiateProcessor(string defId, string dataId, IQuestDataStorage storage);
        List<string> GetAllQuestIds();
        float QuestWeightFunction(string questId);
    }
}