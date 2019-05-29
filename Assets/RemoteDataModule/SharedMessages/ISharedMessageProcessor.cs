using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System.Threading.Tasks;

namespace GBG.Modules.RemoteData.SharedMessages
{
    public interface ISharedMessageProcessor
    {
        Task ProcessMessage(AbstractSharedMessage message);
    }
}
