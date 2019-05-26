using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using RemoteDataModule.SharedMessages.MessageData;
using System;

namespace RemoteDataModule.SharedMessages {
    public abstract class BaseSharedMessagesStorage
    {
        public abstract Task<List<AbstractSharedMessage>> FetchAllMessages();

        public abstract Task CommitMessage(string userId, AbstractSharedMessage message);

        public abstract event Action<List<AbstractSharedMessage>> SelfMessagesUpdated;
    }
}
