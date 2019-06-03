using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GBG.Modules.RemoteData.SharedMessages.MessageData
{
    [Serializable]
    public abstract class AbstractSharedMessage
    {
        /// <summary>
        /// User id отправителя
        /// </summary>
        [SerializeField]
        public string MessageOwner;

        [SerializeField]
        public string MessageType;

        [SerializeField]
        public bool Proceeded;

        public string FullPath { get; private set; }

        /// <summary>
        /// TO DO вместо ииспользования этого метода надо настроить
        /// Newtonsoft.Json чтобы он сериализовал инфу о типе в валидные поля
        /// </summary>
        public void AssureType()
        {
            MessageType = this.GetType().Name;
            FullPath = null;
        }

        public void SetPath(string path)
        {
            FullPath = path;
        }

        public static AbstractSharedMessage FromJson(string typeShortName, string data)
        {
            var typeString = string.Join(".", typeof(AbstractSharedMessage).Namespace, typeShortName);
            return JsonConvert.DeserializeObject(data) as AbstractSharedMessage;
        }
    }
}
