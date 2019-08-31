using GBG.Modules.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples.Quests
{
    public enum ConditionType
    {
        Stars,
        Money
    }

    [Serializable]
    public class SampleQuestDef : QuestDef
    {
        public SampleRewardDef Reward;
        public ConditionDef Condition;
        public override object ConditionDef => Condition;
    }

    [Serializable]
    public class SampleRewardDef
    {
        public enum RewardType
        {
            Gold,
            Stone,
            Iron
        }
        public RewardType Type;
        public int Count;
    }

    [Serializable]
    public class ConditionDef
    {
        public ConditionType Type;
        public int Count;
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateQuestDef", order = 1)]
    public class SOQuestDef : ScriptableObject
    {
        [SerializeField]
        public List<SampleQuestDef> Quests;
    }
}
