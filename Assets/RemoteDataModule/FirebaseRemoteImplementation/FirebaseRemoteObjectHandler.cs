using Firebase.Database;
using RemoteDataModule.RemoteDataAbstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RemoteDataImpl
{
    public class FirebaseRemoteObjectHandler<T> : RemoteObjectHandler<T> where T : class
    {
        DatabaseReference _reference;

        public override event Action<RemoteObjectHandler<T>> ValueChanged;

        public FirebaseRemoteObjectHandler(DatabaseReference reference)
        {
            _reference = reference;
        }

        public override void Dispose()
        {
            _reference.KeepSynced(false);
        }

        public async override Task<RemoteObjectHandler<T>> LoadData(bool keepSynched, Func<T> initialDataProvider)
        {
            if (keepSynched)
            {
                _reference.KeepSynced(keepSynched);
                _reference.ValueChanged -= RemoteValueChanged;
                _reference.ValueChanged += RemoteValueChanged;
            }
            Debug.Log("Requesting data on path :: " + _reference.ToString());
            var data = await _reference.GetValueAsync();
            Debug.Log("RAW DATA :: " + data.GetRawJsonValue());
            if (!data.Exists && initialDataProvider != null)
            {
                var initialData = initialDataProvider();
                await UpdateRemoteData(initialData);
                Object = initialData;
            }
            else
                ParseResult(data);
            return this;
        }

        public override Task UpdateRemoteData(T newObject)
        {
            var jsonValue = JsonUtility.ToJson(newObject);
            return _reference.SetRawJsonValueAsync(jsonValue);
        }

        public override RemoteObjectHandler<TChild> GetChild<TChild>(string path)
        {
            var childRef = _reference.Child(path);
            return new FirebaseRemoteObjectHandler<TChild>(childRef);
        }

        public override string GetDataId()
        {
            return _reference.Key;
        }

        public override RemoteDataChange CreateChange(string fieldName, object fieldValue)
        {
            return new RemoteDataChange()
            {
                FieldName = fieldName,
                FieldValue = fieldValue,
                FullPath = _reference.ToString().Substring(_reference.Root.ToString().Length - 1)
            };
        }

        protected void EnsureLoadSuccess(Task<DataSnapshot> task)
        {
            if (task.IsCanceled) throw new TaskCanceledException();
            if (task.Exception != null) throw task.Exception;
            if (task.IsFaulted) throw new AggregateException("Task faulted");
        }

        protected override async Task ApplyChangeRemote(RemoteDataChange change)
        {
            var changeType = change.FieldValue.GetType();
            if (changeType.IsValueType || change.FieldValue is String)
                await _reference.Child(change.FieldName).SetValueAsync(change.FieldValue);
            else
                await _reference.Child(change.FieldName).SetRawJsonValueAsync(JsonUtility.ToJson(change.FieldValue));
        }

        private void RemoteValueChanged(object sender, ValueChangedEventArgs e)
        {
            ParseResult(e.Snapshot);
            ValueChanged?.Invoke(this);
        }

        private void ParseResult(DataSnapshot dataSnapshot)
        {
            Object = JsonUtility.FromJson<T>(dataSnapshot.GetRawJsonValue());
            ValueChanged?.Invoke(this);
        }

    }
}
