using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System.Threading.Tasks;

namespace GBG.Modules.RemoteData.SharedMessages
{
    public class TextSharedMessageProcessor : ISharedMessageProcessor
    {
        public async Task ProcessMessage(AbstractSharedMessage message)
        {
            var textMessage = message as TextSharedMessage;
            Debug.Log("RECEIVED TEXT MESSAGE :: " + textMessage.MessageText);
        }
    }
}
