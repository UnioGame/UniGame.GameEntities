using GBG.Modules.Quests.Data;

namespace GBG.Modules.Quests
{
    public interface IQuestDefsStorage
    {
        QuestDef GetQuestDef(string questId);
        IQuestCondition InstantiateCondition(QuestDef def, QuestData data);
        IQuestProcessor InstantiateProcessor(QuestDef def, QuestData data);
    }
}