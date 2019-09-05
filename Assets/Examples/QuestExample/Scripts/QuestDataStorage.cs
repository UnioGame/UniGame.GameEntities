using UnityEngine;
using System.Collections;
using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using System.Collections.Generic;

public class QuestDataStorage : IQuestDataStorage
{
    public List<QuestData> GetAllQuest()
    {
        throw new System.NotImplementedException();
    }

    public List<string> GetAllQuestIds()
    {
        throw new System.NotImplementedException();
    }

    public QuestData GetQuestData(string questId)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateQuestData(string questId, QuestData newData)
    {
        throw new System.NotImplementedException();
    }
}
