using RemoteDataModule.Authorization;
using RemoteDataModule.SharedMessages;
using RemoteDataModule.SharedMessages.MessageData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RemoteDataModule.SharedMessages
{
    public class SharedMessagesService
    {
        private Dictionary<Type, ISharedMessageProcessor> _processors = new Dictionary<Type, ISharedMessageProcessor>();

        private IAuthModule _authModule;
        private BaseSharedMessagesStorage _storage;

        public void Init(IAuthModule authModule, BaseSharedMessagesStorage storage)
        {
            this._authModule = authModule;
            this._storage = storage;
            _storage.SelfMessagesUpdated += SelfMessagesUpdated;
        }

        public void Run()
        {
            _storage.FetchAllMessages().ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogException(t.Exception);
                    return;
                }
                NotifyListeners(t.Result);
            });            
        }

        private void SelfMessagesUpdated(List<AbstractSharedMessage> obj)
        {
            NotifyListeners(obj);
        }

        private void NotifyListeners(List<AbstractSharedMessage> messages)
        {
            foreach(var message in messages)
            {
                var type = message.GetType();
                if (_processors.ContainsKey(type))
                    _processors[type].ProcessMessage(message);
                else
                    Debug.LogWarning("No processors registered for message type :: " + type.Name);
            }
        }

        public async Task PushMessage(string userId, AbstractSharedMessage message)
        {
            await _storage.CommitMessage(userId, message);
        }

        public void RegisterProcessor<T>(ISharedMessageProcessor processor) where T : AbstractSharedMessage
        {
            if (_processors.ContainsValue(processor))
                throw new InvalidOperationException("Repeated processor registration for type :: " + processor.GetType().FullName);
            
            _processors.Add(typeof(T), processor);
        }

        public void UnregisterProcessor(ISharedMessageProcessor processor)
        {
            Type keyToRemove = null;
            foreach(var kvp in _processors)
            {
                if (kvp.Value == processor)
                {
                    keyToRemove = kvp.Key;
                    break;
                }
            }
            _processors.Remove(keyToRemove);
        }


        // TO DO commit message as read
    }
}
