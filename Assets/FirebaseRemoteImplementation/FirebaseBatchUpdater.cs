using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using UniGreenModules.UniCore.Runtime.ObjectPool;

namespace GBG.Modules.RemoteData.FirebaseImplementation
{
    public class FirebaseBatchUpdater : BatchUpdater
    {
        /// <summary>
        /// WARNING complex types for field values not supported
        /// </summary>
        /// <param name="changes"></param>
        /// <returns></returns>
        public override async Task PerformBatchUpdate(IEnumerable<RemoteDataChange> changes)
        {
            var changeDictionary = ClassPool.SpawnOrCreate<Dictionary<string, object>>(() => new Dictionary<string, object>());
            foreach(var change in changes)
            {
                changeDictionary.Add(change.FullPath, change.FieldValue);
            }
            var rootRef = FirebaseDatabase.DefaultInstance.RootReference;
            await rootRef.UpdateChildrenAsync(changeDictionary);
            foreach(var change in changes) {
                change.ApplyCallback?.Invoke(change);
                change.Dispose();
            }
            ClassPool.Despawn<Dictionary<string, object>>(changeDictionary, null);
        }
    }
}
