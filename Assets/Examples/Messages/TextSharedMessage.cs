using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples.Messages
{
    [Serializable]
    public class TextSharedMessage : AbstractSharedMessage
    {
        [SerializeField]
        public string MessageText;
    }
}
