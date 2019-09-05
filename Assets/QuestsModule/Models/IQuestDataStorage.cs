using GBG.Modules.Quests.Data;
using System.Collections.Generic;

namespace GBG.Modules.Quests
{
    public interface IQuestDataStorage
    {
        List<string> GetAllQuestIds();

        /// <summary>
        /// Если квеста нет вернет null
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        QuestData GetQuestData(string questId);
        void UpdateQuestData(string questId, QuestData newData);
    }
}