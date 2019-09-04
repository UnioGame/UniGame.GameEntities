using UnityEngine;
using System.Collections;
using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using Samples.Quests;

public class QuestDefStorage : IQuestDefsStorage
{
    private SOQuestDef _data;

    public QuestDefStorage(SOQuestDef data)
    {
        _data = data;
    }

    public QuestDef GetQuestDef(string questId)
    {
        return _data.Quests.Find((q) => q.Id == questId);
    }

    public IQuestCondition InstantiateCondition(QuestDef def, QuestData data)
    {
        throw new System.NotImplementedException();
    }

    public IQuestProcessor InstantiateProcessor(QuestDef def, QuestData data)
    {
        throw new System.NotImplementedException();
    }
}
