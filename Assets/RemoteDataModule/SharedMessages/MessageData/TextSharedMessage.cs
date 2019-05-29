using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Modules.RemoteData.SharedMessages.MessageData
{
    public class TextSharedMessage : AbstractSharedMessage
    {
        [SerializeField]
        public string MessageText;
    }
}
