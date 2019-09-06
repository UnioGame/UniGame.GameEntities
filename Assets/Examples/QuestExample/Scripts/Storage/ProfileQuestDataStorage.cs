using UnityEngine;
using System.Collections;
using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Quests.Storage
{
    public class ProfileQuestDataStorage : IQuestDataStorage
    {
        private MutableUserProfile _profile;

        public ProfileQuestDataStorage(MutableUserProfile profile)
        {
            _profile = profile;
        }
        
        public List<string> GetAllQuestIds()
        {
            return _profile.Quests.Keys.Select((k) => k).ToList();
        }

        public QuestData GetQuestData(string questId)
        {
            return _profile.Quests.TryGetValue(questId, out QuestData data) ? data : null;
        }

        // TODO Озаботиться изменением квестовой даты а не заменой ее целиком
        public void UpdateQuestData(string questId, QuestData newData)
        {
            _profile.Quests[questId] = newData;
            if (newData == null)
                _profile.Quests.Remove(questId);
            _profile.CommitChanges();
        }
    }
}
