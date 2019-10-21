using GBG.Modules.Quests.FsmParts;
using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Samples
{

    public class UpdateQuestLocals : FsmStateAction
    {
        public FsmString QuestId;
        public override void OnEnter()
        {
            base.OnEnter();
            var resultDicitonary = new Dictionary<string, string>();
            var vars = Fsm.Variables.GetAllNamedVariables();
            foreach(var v in vars)
            {
                resultDicitonary.Add(v.Name, v.RawValue.ToString());
            }
            MessageBroker.Default.Publish(new QuestLocalsUpdated(QuestId.Value, resultDicitonary));
            Finish();
        }
    }
}
