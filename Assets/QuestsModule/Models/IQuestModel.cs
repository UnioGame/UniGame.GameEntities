using GBG.Modules.Quests.Data;
using System;
using UniRx;

namespace GBG.Modules.Quests
{
    public class QuestModel: IObservable<QuestModel>, IDisposable
    {
        private readonly IQuestProcessor _processor;
        private readonly ReactiveCommand<QuestModel> _onChanged;
        private readonly ReactiveProperty<QuestState> _questState;

        public QuestModel(IQuestProcessor processor)
        {
            _processor = processor;
            _onChanged = new ReactiveCommand<QuestModel>();
            _questState = new ReactiveProperty<QuestState>();
            _questState.Value = _processor.QuestState;
            _processor.StateChanged += ProcessorStateChanged;
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
            _questState.Dispose();
            _onChanged.Dispose();
        }

        public float MaxProgress => _processor.MaxProgress;
        public IReadOnlyReactiveProperty<float> Progress => _processor.Progress;
        public IReactiveProperty<QuestState> State => _questState; 
        public string QuestDefId => _processor.QuestDefId;
        public string QuestDataId => _processor.QuestDataId;
    }
}