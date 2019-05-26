using UnityEngine;
using System.Collections;
using RemoteDataModule.SharedMessages;
using RemoteDataModule.SharedMessages.MessageData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;

namespace RemoteDataModule.FirebaseImplementation.SharedMessages
{
    public sealed class FirebaseSharedMessagesStorage : BaseSharedMessagesStorage
    {
        private string _ownerUserId = null;

        public override event Action<List<AbstractSharedMessage>> SelfMessagesUpdated;

        public void Init(string ownerId)
        {
            _ownerUserId = ownerId;
        }

        public override async Task CommitMessage(string userId, AbstractSharedMessage message)
        {
            EnsureInitialized();
            var serializedData = JsonUtility.ToJson(message);
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(string.Format("SharedMessages/{0}/", userId));
            var newKey = reference.Push().Key;
            await reference.Child(newKey).SetRawJsonValueAsync(serializedData);
        }

        public override async Task<List<AbstractSharedMessage>> FetchAllMessages()
        {
            EnsureInitialized();
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(string.Format("SharedMessages/{0}/", _ownerUserId));
            var data = await reference.GetValueAsync();
            List<AbstractSharedMessage> result = new List<AbstractSharedMessage>();
            foreach(var messageData in data.Children)
            {
                var type = (string)messageData.Child("MessageType").Value;
                var message = AbstractSharedMessage.FromJson(type, messageData.GetRawJsonValue());
                result.Add(message);
            }
            return result;
        }

        private void EnsureInitialized()
        {
            if (_ownerUserId == null)
                throw new InvalidOperationException(nameof(FirebaseSharedMessagesStorage) + " not initialized");
        }
    }
}
