using GBG.Modules.Quests.Data;
using System.Collections.Generic;

namespace GBG.Modules.Quests
{
    public interface IQuestDataStorage
    {
        List<QuestData> GetAllQuest();
        QuestData GetQuestData(string questId);
        void UpdateQuestData(string questId, QuestData newData);
    }
}