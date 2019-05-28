using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace RemoteDataModule.RemoteDataAbstracts
{
    public abstract class RemoteObjectHandler<T> : IDisposable where T : class
    {
        public T Object { get; protected set; }

        public abstract void Dispose();

        public abstract Task<RemoteObjectHandler<T>> LoadData(bool keepSynched = false, Func<T> initialDataProvider = null);

        public abstract Task UpdateRemoteData(T newData);

        public abstract event Action<RemoteObjectHandler<T>> ValueChanged;

        public abstract RemoteObjectHandler<TChild> GetChild<TChild>(string path) where TChild : class;

        public abstract RemoteDataChange CreateChange(string fieldName, object fieldValue);

        public async Task ApplyChange(RemoteDataChange change)
        {
            ApplyChangeLocal(change);
            await ApplyChangeRemote(change);
        }

        public void ApplyChangeLocal(RemoteDataChange change)
        {
            var fieldInfo = typeof(T).GetField(change.FieldName);
            fieldInfo.SetValue(Object, change.FieldValue);
        }

        public abstract string GetDataId();

        protected abstract Task ApplyChangeRemote(RemoteDataChange change);
    }
}
