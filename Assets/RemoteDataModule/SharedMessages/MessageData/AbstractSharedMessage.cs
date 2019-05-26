using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RemoteDataModule.SharedMessages.MessageData
{
    [Serializable]
    public abstract class AbstractSharedMessage : ISerializationCallbackReceiver
    {
        /// <summary>
        /// User id отправителя
        /// </summary>
        [SerializeField]
        public string MessageOwner;

        [SerializeField]
        public string MessageType;

        [SerializeField]
        private bool Proceeded;

        public void OnAfterDeserialize()
        {
            this.MessageType = this.GetType().Name;
        }

        public void OnBeforeSerialize()
        {
            this.MessageType = this.GetType().Name;
        }

        public static AbstractSharedMessage FromJson(string typeShortName, string data)
        {
            var typeString = string.Join(".", typeof(AbstractSharedMessage).Namespace, typeShortName);
            return JsonUtility.FromJson(data, Type.GetType(typeString)) as AbstractSharedMessage;
        }
    }
}
