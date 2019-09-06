using GBG.Modules.Quests.Data;
using System;
using UniRx;

namespace GBG.Modules.Quests
{
    public interface IQuestProcessor
    {
        event Action StateChanged;
        
        QuestState QuestState { get; }
        IReadOnlyReactiveProperty<float> Progress { get; }
        float MaxProgress { get; }
        string QuestDefId { get; }
        string QuestDataId { get; }
        void DeleteData();
    }
}