using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Modules.Quests.Data
{
    public enum QuestState
    {
        InProgress = 10,
        RewardAvailable = 20,
        Done = 30,
        Failed = 40,
        ReadyToRemove = 50
    }

    /// <summary>
    /// АХТУНГ! При использовании вместе с Firebase в progressStorage нельзя пихать данные 
    /// которые будут похожи на массив (например целочисленные ключи) черевато обосрамсами 
    /// при десериализации 
    /// </summary>
    public class QuestData
    {
        public string Id;
        public QuestState State;
        public Dictionary<string, string> ProgressStorage = new Dictionary<string, string>();
    }
}
