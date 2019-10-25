using GBG.Modules.Quests.Data;
using System;
using UniRx;

namespace GBG.Modules.Quests
{
    public interface IQuestModel : IDisposable
    {
        float MaxProgress { get; }
        float Progress { get; }
        QuestState State { get; }
        string QuestDefId { get; }
        string QuestDataId { get; }
        void DeleteQuest();
    }
}