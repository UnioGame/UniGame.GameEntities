using UnityEngine;
using System.Collections;
using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using Samples.Quests;
using System.Collections.Generic;
using System;
using System.Linq;
using Samples;
using GBG.Modules.Quests.FsmParts;

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

    public PlayMakerFSM InstantiateFSM(string defId, string questId)
    {
        var def = GetQuestDef(defId);
        var fsm = def.QuestFsm;
        var result = GameObject.Instantiate(fsm);
        result.gameObject.name = $"Quest :: {defId} :: {questId}";
        result.FsmVariables.GetFsmString(StorageConstants.FSM_QUEST_ID_NAME).Value = questId;
        return result;
    }

    public float QuestWeightFunction(string questId)
    {
        return GetQuestDef(questId).Weight;
    }
}
