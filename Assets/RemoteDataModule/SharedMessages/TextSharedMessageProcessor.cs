using UnityEngine;
using System.Collections;
using RemoteDataModule.SharedMessages.MessageData;
using System.Threading.Tasks;

namespace RemoteDataModule.SharedMessages
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
