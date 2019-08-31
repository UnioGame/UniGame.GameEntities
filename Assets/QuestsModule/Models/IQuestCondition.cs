using UniRx;

namespace GBG.Modules.Quests
{
    public interface IQuestCondition
    {
        IReactiveCommand<IQuestCondition> RelevantStateChanged { get; }
    }
}