using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GBG.Modules.RemoteData.SharedMessages.MessageData
{
    using System.Linq;

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
        
        private static Dictionary<string, Type> cacheTypes = new Dictionary<string, Type>();

        public static AbstractSharedMessage FromJson(string typeShortName, string data)
        {
            Type GetTypeForDeserialization()
            {
                if (cacheTypes.TryGetValue(typeShortName, out var cachedType)) {
                    return cachedType;
                }
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        if (type.Name == typeShortName) {
                            cacheTypes.Add(typeShortName, type);
                            return type;
                        }
                    }
                }

                return null;
            }
            return (AbstractSharedMessage) JsonConvert.DeserializeObject(data, GetTypeForDeserialization());
        }
    }
}
