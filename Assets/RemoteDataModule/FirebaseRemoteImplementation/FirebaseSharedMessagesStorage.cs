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
        private FirebaseAuthModule _auth;

        public override event Action<List<AbstractSharedMessage>> SelfMessagesUpdated;

        public void Init(FirebaseAuthModule auth)
        {
            _auth = auth;
        }

        public override async Task CommitMessage(string userId, AbstractSharedMessage message)
        {
            var serializedData = JsonUtility.ToJson(message);
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(string.Format("SharedMessages/{0}/", userId));
            var newKey = reference.Push().Key;
            await reference.Child(newKey).SetRawJsonValueAsync(serializedData);
        }

        public override async Task<List<AbstractSharedMessage>> FetchAllMessages()
        {
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(string.Format("SharedMessages/{0}/", _auth.CurrentUserId));
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

    }
}
