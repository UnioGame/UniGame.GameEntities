using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System.Threading.Tasks;
using GBG.Modules.RemoteData.SharedMessages;

namespace Samples.Messages
{
    public class TextSharedMessageProcessor : ISharedMessageProcessor
    {
        public void ProcessMessage(AbstractSharedMessage message)
        {
            var textMessage = message as TextSharedMessage;
            Debug.Log("RECEIVED TEXT MESSAGE :: " + textMessage.MessageText);
        }
    }
}
