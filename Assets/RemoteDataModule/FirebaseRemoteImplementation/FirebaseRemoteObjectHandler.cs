using Firebase.Database;
using RemoteDataAbstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RemoteDataImpl
{
    public class FirebaseRemoteObjectHandler<T> : RemoteObjectHandler<T> where T:class
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

        public override Task<RemoteObjectHandler<T>> LoadData(bool keepSynched)
        {
            if (keepSynched)
            {
                _reference.KeepSynced(keepSynched);
                _reference.ValueChanged += RemoteValueChanged;
            }
            Debug.Log("Requesting data on path :: " + _reference.ToString());
            var task = _reference.GetValueAsync().ContinueWith<RemoteObjectHandler<T>>((loadTask) =>
            {
                Debug.Log("RAW DATA :: " + loadTask.Result.GetRawJsonValue());
                EnsureLoadSuccess(loadTask);
                var data = loadTask.Result;
                ParseResult(data);
                return this;
            });
            return task;
        }

        private void RemoteValueChanged(object sender, ValueChangedEventArgs e)
        {
            ParseResult(e.Snapshot);
            ValueChanged?.Invoke(this);
        }

        private void ParseResult(DataSnapshot dataSnapshot)
        {
            try
            {
                // Списки не парсятся в качестве корневого объекта потому
                if (typeof(T) == typeof(string[]))
                {
                    List<string> list = new List<string>();
                    foreach(var child in dataSnapshot.Children)
                    {
                        list.Add(child.Value.ToString());
                    }
                    Object = list.ToArray() as T;
                }
                else
                    Object = JsonUtility.FromJson<T>(dataSnapshot.GetRawJsonValue());

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            ValueChanged?.Invoke(this);
        }

        protected void EnsureLoadSuccess(Task<DataSnapshot> task)
        {
            if (task.IsCanceled) throw new TaskCanceledException();
            if (task.Exception != null) throw task.Exception;
            if (task.IsFaulted) throw new AggregateException("Task faulted");
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
    }
}
