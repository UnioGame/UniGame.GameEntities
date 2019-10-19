using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Samples.Quests.Fsm
{
    [ActionCategory(ActionCategory.World)]
    public class AddScoreAction : FsmStateAction
    {
        public FsmInt Count;

        public FsmString Reason;
        public override void OnEnter()
        {
            // возможно в этом акшоне надо будет понимать, что за квест происходит и
            // исходя из него собирать Reason
            base.OnEnter();
            MessageBroker.Default.Publish(new AddScoreMessage(Count.Value, Reason.Value));
        }
    }
}
