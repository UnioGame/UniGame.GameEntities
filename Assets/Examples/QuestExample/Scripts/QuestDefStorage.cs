using UnityEngine;
using System.Collections;
using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using Samples.Quests;
using System.Collections.Generic;
using System;
using System.Linq;
using Samples;

public class QuestDefStorage : IQuestDefsStorage
{
    private SOQuestDef _data;
    private MutableUserProfile _profile;

    public QuestDefStorage(SOQuestDef data, MutableUserProfile profile)
    {
        _data = data;
        _profile = profile;
    }

    public List<string> GetAllQuestIds()
    {
        return _data.Quests.Select((def) => def.Id).ToList();
    }

    public QuestDef GetQuestDef(string questId)
    {
        return _data.Quests.Find((q) => q.Id == questId);
    }

    public IQuestProcessor InstantiateProcessor(string defId, string dataId, IQuestDataStorage storage)
    {
        var def = GetQuestDef(defId) as SampleQuestDef;

        if (def == null) throw new ArgumentException("Quest def with ID = " + defId + ", not exist!");

        if(def.Condition.Type == ConditionType.Score)
            return new ScoreProcessor(_profile, def, storage, dataId);

        throw new InvalidOperationException("Unable to create processor for :: " + defId);
    }

    public float QuestWeightFunction(string questId)
    {
        return 1;
    }
}
