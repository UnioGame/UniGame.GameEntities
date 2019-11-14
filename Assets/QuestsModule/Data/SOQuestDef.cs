using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBG.Modules.Quests.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateQuestDef", order = 1)]
    public class SOQuestDef : ScriptableObject
    {
        [SerializeField]
        public List<QuestDef> Quests;
        public IEnumerable<string> GetAllIds()
        {
            return this.Quests.Select((quest) => quest.Id);
        }

        [NotNull]
        public string GetRandomQuestId()
        {
            var totalWeight = 0f;
            for (int i = 0; i < Quests.Count; i++)
                totalWeight += Quests[i].Weight;
            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            var accWeight = 0f;
            for (int i = 0; i < Quests.Count; i++)
            {
                accWeight += Quests[i].Weight;
                if (accWeight >= randomValue)
                    return Quests[i].Id;
            }
            throw new Exception("Unable to generate random quest Id");
        }
    }
}
