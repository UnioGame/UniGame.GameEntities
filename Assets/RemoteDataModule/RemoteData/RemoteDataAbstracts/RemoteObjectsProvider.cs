using UnityEngine;
using System.Collections;
using System;

namespace RemoteDataAbstracts
{
    public abstract class RemoteObjectsProvider : IDisposable
    {
        public abstract void Dispose();

        public abstract RemoteObjectHandler<T> GetRemoteObject<T>(string path) where T : class;
    }
}
