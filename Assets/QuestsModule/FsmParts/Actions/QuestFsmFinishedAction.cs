using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests.FsmParts
{
    public class QuestProcessingFinishedMessage
    {
        public QuestProcessingFinishedMessage(string dataId)
        {
            this.QuestDataId = dataId;
        }
        public string QuestDataId { get; private set; }
    }

    public class QuestLocalsUpdated
    {
        public readonly string QuestDataId;
        public readonly Dictionary<string, string> Values;
        public QuestLocalsUpdated(string dataId, Dictionary<string, string> values)
        {
            QuestDataId = dataId;
            Values = values;
        }
    }
    public class QuestFsmFinishedAction : FsmStateAction
    {
        public FsmString QuestId;
        public override void OnEnter()
        {
            base.OnEnter();
            MessageBroker.Default.Publish(new QuestProcessingFinishedMessage(QuestId.Value));
            Finish();
        }
    }
}
