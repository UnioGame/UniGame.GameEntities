using System;
using UniRx;

namespace GBG.Modules.Quests
{
    public class QuestModel: IObservable<QuestModel>
    {
        private readonly IQuestProcessor _processor;
        private readonly ReactiveCommand<QuestModel> _onChanged;

        public QuestModel(IQuestProcessor processor)
        {
            _processor = processor;
            _onChanged = new ReactiveCommand<QuestModel>();
        }

        public IDisposable Subscribe(IObserver<QuestModel> observer)
        {
            return _onChanged.Subscribe(observer);
        }

        public float MaxProgress => _processor.MaxProgress;
        public IReadOnlyReactiveProperty<float> Progress => _processor.Progress;
        public string QuestDefId => _processor.QuestDefId;
        public string QuestDataId => _processor.QuestDataId;
    }
}