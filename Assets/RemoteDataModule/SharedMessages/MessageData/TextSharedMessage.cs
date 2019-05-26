using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RemoteDataModule.SharedMessages.MessageData
{
    public class TextSharedMessage : AbstractSharedMessage
    {
        [SerializeField]
        public string MessageText;
    }
}
