using GBG.Modules.Quests;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Modules.Quests.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateQuestDef", order = 1)]
    public class SOQuestDef : ScriptableObject
    {
        [SerializeField]
        public List<QuestDef> Quests;
    }
}
