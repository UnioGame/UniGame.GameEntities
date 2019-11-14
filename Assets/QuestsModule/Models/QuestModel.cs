using GBG.Modules.Quests.Data;
using GBG.Modules.Quests.FsmParts;
using GBG.Modules.Quests.FsmParts.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public sealed class QuestModel : MonoBehaviour, IQuestModel, IObservable<QuestModel>
    {
        private IQuestDataStorage _dataStorage;
        private IQuestDefsStorage _defStorage;
        private ReactiveCommand<QuestModel> _onChanged;
        private string _questDefId;
        private string _questDataId;
        private PlayMakerFSM _correspondingFSM;
        private QuestState _state;

        public float MaxProgress => _correspondingFSM.FsmVariables.GetFsmFloat(StorageConstants.FSM_QUEST_MAX_PROGRESS_NAME).Value;
        // TO DO Reactive
        public float Progress => _correspondingFSM.FsmVariables.GetFsmFloat(StorageConstants.FSM_QUEST_PROGRESS_NAME).Value;
        public QuestState State => (QuestState)_correspondingFSM.FsmVariables.GetFsmEnum(StorageConstants.FSM_QUEST_STATE_NAME).Value;
        public string QuestDefId => _questDefId;
        public string QuestDataId => _questDataId;
        public string Description => _defStorage.GetQuestDef(QuestDefId).Description;
        //TODO Localize

        private List<IDisposable> _disposables = new List<IDisposable>();

        public void Init(
            PlayMakerFSM correspondingFsm,
            IQuestDataStorage dataStorage,
            IQuestDefsStorage defStorage,
            string defId,
            string dataId)
        {
            _questDefId = defId;
            _questDataId = dataId;
            _dataStorage = dataStorage;
            _defStorage = defStorage;
            InitQuestData();

            _onChanged = new ReactiveCommand<QuestModel>();
            _disposables.Add(_onChanged);

            _correspondingFSM = correspondingFsm;
            InitFSMVariables(_correspondingFSM);
            // возможно нужно вытирать дату полностью
            var disposable = MessageBroker.Default.Receive<QuestProcessingFinishedMessage>()
                .Where(v => v.QuestDataId == QuestDataId)
                .Subscribe(message => Dispose());
            _disposables.Add(disposable);

            disposable = MessageBroker.Default.Receive<QuestLocalsUpdated>()
                .Where(v => v.QuestDataId == QuestDataId)
                .Subscribe(message =>
                {
                    var data = _dataStorage.GetQuestData(QuestDataId);
                    data.ProgressStorage = message.Values;
                    _dataStorage.UpdateQuestData(QuestDataId, data);
                });
            _disposables.Add(disposable);

            disposable = MessageBroker.Default.Receive<QuestStateChangedAction>()
                .Where(message => message.QuestId == QuestDataId)
                .Subscribe(message =>
                {
                    var data = _dataStorage.GetQuestData(QuestDataId);
                    data.State = (int)State;
                    _onChanged.Execute(this);
                });
            _disposables.Add(disposable);
        }

        private void InitQuestData()
        {
            var questData = _dataStorage.GetQuestData(QuestDataId);
            if (questData == null)
            {
                questData = new QuestData();
                questData.Id = _questDefId;
                questData.State = (int)QuestState.InProgress;
                _dataStorage.UpdateQuestData(QuestDataId, questData);
            }
        }

        private void InitFSMVariables(PlayMakerFSM fsm)
        {
            var data = _dataStorage.GetQuestData(QuestDataId);
            if (data != null && data.ProgressStorage != null)
            {
                foreach (var kvp in data.ProgressStorage)
                {
                    var fsmVar = fsm.FsmVariables.GetVariable(kvp.Key);
                    if (fsmVar != null)
                    {
                        var varType = fsmVar.RawValue.GetType();
                        var converter = TypeDescriptor.GetConverter(varType);
                        fsmVar.RawValue = converter.ConvertFromString(kvp.Value);
                    }
                }
            }

        }


        public IDisposable Subscribe(IObserver<QuestModel> observer)
        {
            return _onChanged.Subscribe(observer);
        }

        public void DeleteQuest()
        {
            _dataStorage.UpdateQuestData(_questDataId, null);
            Dispose();
        }

        public string GetDescription()
        {
            return Description;
        }

        public void Dispose()
        {
            GameObject.Destroy(_correspondingFSM.gameObject);
            _correspondingFSM = null;
            _disposables.ForEach((o) => o.Dispose());
            _disposables.Clear();
        }
    }
}