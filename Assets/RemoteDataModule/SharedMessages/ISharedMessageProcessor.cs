using UnityEngine;
using System.Collections;
using RemoteDataModule.SharedMessages.MessageData;
using System.Threading.Tasks;

namespace RemoteDataModule.SharedMessages
{
    public interface ISharedMessageProcessor
    {
        Task ProcessMessage(AbstractSharedMessage message);
    }
}
