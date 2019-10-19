using GBG.Modules.Quests.Data;
using GBG.Modules.Quests.FsmParts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public class QuestModel: IObservable<QuestModel>, IDisposable
    {
        private readonly IQuestProcessor _processor;
        private readonly ReactiveCommand<QuestModel> _onChanged;
        private readonly ReactiveProperty<QuestState> _questState;
        private PlayMakerFSM _correspondingFSM;

        private List<IDisposable> _disposables = new List<IDisposable>();

        public QuestModel(IQuestProcessor processor, PlayMakerFSM correspondingFsm)
        {
            _processor = processor;
            _onChanged = new ReactiveCommand<QuestModel>();
            _disposables.Add(_onChanged);
            _questState = new ReactiveProperty<QuestState>();
            _disposables.Add(_questState);
            _questState.Value = _processor.QuestState;
            _processor.StateChanged += ProcessorStateChanged;
            _correspondingFSM = correspondingFsm;
            var disposable = MessageBroker.Default.Receive<QuestProcessingFinishedMessage>()
                .Where(v => v.QuestDataId == QuestDataId)
                .Subscribe(message => Dispose());
            // возможно нужно вытирать дату полностью
            _disposables.Add(disposable);
        }

        private void ProcessorStateChanged()
        {
            _questState.Value = _processor.QuestState;
            _onChanged.Execute(this);
        }

        public IDisposable Subscribe(IObserver<QuestModel> observer)
        {
            return _onChanged.Subscribe(observer);
        }

        public void DeleteQuest()
        {
            _processor.DeleteData();
            Dispose();
        }

        public void Dispose()
        {
            _processor.StateChanged -= ProcessorStateChanged;
            GameObject.Destroy(_correspondingFSM.gameObject);
            _correspondingFSM = null;
            _disposables.ForEach((o) => o.Dispose());
            _disposables.Clear();
        }

        public float MaxProgress => _processor.MaxProgress;
        public IReadOnlyReactiveProperty<float> Progress => _processor.Progress;
        public IReactiveProperty<QuestState> State => _questState; 
        public string QuestDefId => _processor.QuestDefId;
        public string QuestDataId => _processor.QuestDataId;
    }
}