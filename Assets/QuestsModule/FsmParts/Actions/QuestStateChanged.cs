using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests.FsmParts.Actions
{
    public class QuestStateChanged : FsmStateAction
    {
        public FsmString questId;
        public override void OnEnter()
        {
            base.OnEnter();
            MessageBroker.Default.Publish(new QuestStateChangedAction(questId.Value));
            Finish();
        }
    }

    public class QuestStateChangedAction
    {
        public string QuestId;
        public QuestStateChangedAction(string questId)
        {
            QuestId = questId;
        }
    }
}
