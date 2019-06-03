using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.SharedMessages;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using GBG.Modules.RemoteData.Authorization;
using Newtonsoft.Json;

namespace GBG.Modules.RemoteData.FirebaseImplementation.SharedMessages
{
    public sealed class FirebaseSharedMessagesStorage : BaseSharedMessagesStorage
    {
        private IAuthModule _auth;
        
        public override event Action<List<AbstractSharedMessage>> SelfMessagesUpdated;

        public void Init(IAuthModule auth)
        {
            _auth = auth;
        }

        public override async Task CommitMessage(string userId, AbstractSharedMessage message)
        {
            message.AssureType();
            var serializedData = JsonConvert.SerializeObject(message);
            Debug.Log("Commiting serialized message :: " + serializedData);
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
                message.SetPath(messageData.Reference.ToString().Substring(messageData.Reference.Root.ToString().Length));
                result.Add(message);
            }
            return result;
        }

    }
}
