using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System;

namespace GBG.Modules.RemoteData.SharedMessages {
    public abstract class BaseSharedMessagesStorage
    {
        public abstract Task<List<AbstractSharedMessage>> FetchAllMessages();

        public abstract Task CommitMessage(string userId, AbstractSharedMessage message);

        public abstract event Action<List<AbstractSharedMessage>> SelfMessagesUpdated;
    }
}
