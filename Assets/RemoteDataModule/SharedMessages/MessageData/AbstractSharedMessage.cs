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
        private bool Proceeded;

        /// <summary>
        /// TO DO вместо ииспользования этого метода надо настроить
        /// Newtonsoft.Json чтобы он сериализовал инфу о типе в валидные поля
        /// </summary>
        public void AssureType()
        {
            MessageType = this.GetType().Name;
        }

        public static AbstractSharedMessage FromJson(string typeShortName, string data)
        {
            var typeString = string.Join(".", typeof(AbstractSharedMessage).Namespace, typeShortName);
            return JsonConvert.DeserializeObject(data) as AbstractSharedMessage;
        }
    }
}
