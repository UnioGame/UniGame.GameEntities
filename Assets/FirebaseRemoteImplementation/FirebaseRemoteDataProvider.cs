using Firebase.Database;
using RemoteDataImpl;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOIFirebase.RemoteDataImpl
{
    public class FirebaseRemoteDataProvider : RemoteObjectsProvider
    {
        public override void Dispose()
        {
            // TO DO

        }

        // TO DO variative bases

        public override string GetIdForNewObject(string path)
        {
            return FirebaseDatabase.DefaultInstance.RootReference.Child(path).Push().Key;
        }

        public override RemoteObjectHandler<T> GetRemoteObject<T>(string path)
        {
            var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(path);
            return new FirebaseRemoteObjectHandler<T>(reference);
        }
    }
}
