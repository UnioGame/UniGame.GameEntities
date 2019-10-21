using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples
{
    [ActionCategory(ActionCategory.World)]
    public class UpdateTimeAction : FsmStateAction
    {
        public FsmInt _timeVar;

        public override void OnUpdate()
        {
            base.OnUpdate();
            _timeVar.Value = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            // TODO globalTime -> long/double
        }

    }
}
