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
    public class QuestFsmFinishedAction : FsmStateAction
    {
        public FsmString QuestId;
        public override void OnEnter()
        {
            base.OnEnter();
            MessageBroker.Default.Publish(new QuestProcessingFinishedMessage(QuestId.Value));
        }
    }
}
