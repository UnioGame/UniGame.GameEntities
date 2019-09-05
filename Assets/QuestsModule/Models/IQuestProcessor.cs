using System;
using UniRx;

namespace GBG.Modules.Quests
{
    public interface IQuestProcessor
    {
        event Action QuestStateChanged;

        void ProcessStateChange();

        IReadOnlyReactiveProperty<float> Progress { get; }
        float MaxProgress { get; }
        string QuestDefId { get; }
        string QuestDataId { get; }
    }
}