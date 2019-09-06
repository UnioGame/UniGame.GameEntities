using GBG.Modules.RemoteData.SharedMessages;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System;
using System.Threading.Tasks;
using Firebase.Database;
using GBG.Modules.RemoteData.Authorization;
using Newtonsoft.Json;

namespace GBG.Modules.RemoteData.FirebaseImplementation.SharedMessages
{
    public sealed class FirebaseSharedMessagesStorage : BaseSharedMessagesStorage
    {
        private IAuthModule _auth;
        
        public override event Action<AbstractSharedMessage> MessageAdded;

        private DatabaseReference SelfMessagesReference
        {
            get
            {
                if(_selfMessagesReference == null)
                {
                    _selfMessagesReference = FirebaseDatabase.DefaultInstance.RootReference.Child($"SharedMessages/{_auth.CurrentUserId}/");
                }
                return _selfMessagesReference;
            }
        }
        private DatabaseReference _selfMessagesReference;


        public void Init(IAuthModule auth)
        {
            _auth = auth;
        }

        public override async Task CommitMessage(string userId, AbstractSharedMessage message)
        {
            message.AssureType();
            var serializedData = JsonConvert.SerializeObject(message);
#if USERDATA_MODULE_DEBUG
            if (Debug.isDebugBuild || Application.isEditor) {
            Debug.Log("Commiting serialized message :: " + serializedData);
            }
#endif
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child($"SharedMessages/{userId}/");
            var newKey = reference.Push().Key;
            await reference.Child(newKey).SetRawJsonValueAsync(serializedData);
        }

        public override void StartListen()
        {
            SelfMessagesReference.ChildAdded += OnMessageAdded;
        }
        
        private void OnMessageAdded(object sender, ChildChangedEventArgs e)
        {
            var messageData = e.Snapshot;
            var type = (string)messageData.Child("MessageType").Value;
            var message = AbstractSharedMessage.FromJson(type, messageData.GetRawJsonValue());
            message.SetPath(messageData.Reference.ToString().Substring(messageData.Reference.Root.ToString().Length));
            if (MessageAdded != null)
                MessageAdded(message);
        }

        public override void Dispose()
        {
            SelfMessagesReference.ChildAdded -= OnMessageAdded;
            SelfMessagesReference.KeepSynced(false);
        }
    }
}
