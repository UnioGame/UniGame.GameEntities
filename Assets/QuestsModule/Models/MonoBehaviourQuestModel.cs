using System.Collections;
using System.Collections.Generic;
using GBG.Modules.Quests.Data;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public sealed class MonoBehaviourQuestModel : MonoBehaviour, IQuestModel
    {
        private IQuestModel _wrappedModel;

        public float MaxProgress => _wrappedModel.MaxProgress;

        public float Progress => _wrappedModel.Progress;

        public IReactiveProperty<QuestState> State => _wrappedModel.State;

        public string QuestDefId => _wrappedModel.QuestDefId;

        public string QuestDataId => _wrappedModel.QuestDataId;

        public void DeleteQuest()
        {
            _wrappedModel.DeleteQuest();
        }

        public void Dispose()
        {
            _wrappedModel.Dispose();
        }

        public void Init(IQuestModel wrappedModel)
        {
            _wrappedModel = wrappedModel;
        }
    }

}
