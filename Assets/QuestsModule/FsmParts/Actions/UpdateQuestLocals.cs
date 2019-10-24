using HutongGames.PlayMaker;
using System.Collections.Generic;
using UniRx;

namespace GBG.Modules.Quests.FsmParts.Actions
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
