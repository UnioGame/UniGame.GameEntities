using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace RemoteDataAbstracts
{
    public abstract class RemoteObjectHandler<T> : IDisposable where T:class
    {
        public T Object { get; protected set; }

        public abstract void Dispose();

        public abstract Task<RemoteObjectHandler<T>> LoadData(bool keepSynched = false);

        public abstract Task UpdateRemoteData(T newData);

        public abstract event Action<RemoteObjectHandler<T>> ValueChanged;

        public abstract RemoteObjectHandler<TChild> GetChild<TChild>(string path) where TChild:class;
    }
}
