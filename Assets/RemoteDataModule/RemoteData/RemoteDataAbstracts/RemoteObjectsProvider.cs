using UnityEngine;
using System.Collections;
using System;

namespace RemoteDataModule.RemoteDataAbstracts
{
    public abstract class RemoteObjectsProvider : IDisposable
    {
        public abstract void Dispose();

        public abstract RemoteObjectHandler<T> GetRemoteObject<T>(string path) where T : class;
    }
}
