using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System;

namespace GBG.Modules.RemoteData.SharedMessages {
    public abstract class BaseSharedMessagesStorage : IDisposable
    {
        public abstract void StartListen();

        public abstract Task CommitMessage(string userId, AbstractSharedMessage message);

        public abstract void Dispose();

        public abstract event Action<AbstractSharedMessage> MessageAdded;
    }
}
