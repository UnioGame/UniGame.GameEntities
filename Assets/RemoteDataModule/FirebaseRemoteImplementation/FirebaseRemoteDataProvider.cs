using Firebase.Database;
using RemoteDataImpl;
using RemoteDataModule.RemoteDataAbstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOIFirebase.RemoteDataImpl
{
    public class FirebaseRemoteDataProvider : RemoteObjectsProvider
    {
        private Dictionary<string, DatabaseReference> _cachedReferences = new Dictionary<string, DatabaseReference>();

        public override void Dispose()
        {
            _cachedReferences.Clear();
        }

        public override RemoteObjectHandler<T> GetRemoteObject<T>(string path)
        {
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(path);
           // _cachedReferences.Add(path, reference);
            return new FirebaseRemoteObjectHandler<T>(reference);
        }
    }
}
