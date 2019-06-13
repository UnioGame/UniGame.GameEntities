using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IObservable<TKey> { };

    public class MutableDictionary<TValue> : MutableChild<Dictionary<string, TValue>>, IObservableDictionary<string, TValue>
    {
        public MutableDictionary(Func<Dictionary<string, TValue>> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {
        }

        public TValue this[string key] { get => Object[key]; set => AddUpdateChange(key, value); }

        public ICollection<string> Keys => Object.Keys;

        public ICollection<TValue> Values => Object.Values;

        public int Count => Object.Count;

        public bool IsReadOnly => false;

        public void Add(string key, TValue value)
        {
            if (Object.ContainsKey(key))
                throw new InvalidOperationException(string.Format("Element with key :: {0} exists in Dictionary", key));
            AddUpdateChange(key, value);
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            if (Object.ContainsKey(item.Key))
                throw new InvalidOperationException(string.Format("Element with key :: {0} exists in Dictionary", item.Key));
            AddUpdateChange(item.Key, item.Value);
        }

        public void Clear()
        {
            var clearChange = new RemoteDataChange()
            {
                FullPath = FullPath,
                FieldName = string.Empty,
                FieldValue = null,
                ApplyCallback = ClearApply
            };
            AddChange(clearChange);

        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return Object.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return Object.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            Debug.LogError("1");

            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return Object.GetEnumerator() as IEnumerator<KeyValuePair<string, TValue>>;
        }

        public bool Remove(string key)
        {
            AddRemoveChange(key);
            return true;
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            if (Object.Contains(item))
            {
                AddRemoveChange(item.Key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return Object.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Object.GetEnumerator();
        }

        private void ClearApply(RemoteDataChange change)
        {
            var keys = Object.Keys.ToArray();
            Object.Clear();
            foreach (var key in keys)
                OnItemChanged(key);
        }

        private void OnItemChanged(string key)
        {
            foreach (var observer in _observers)
            {
                observer._observer.OnNext(key);
            }
        }

        private void AddRemoveChange(string key)
        {
            var remove = new RemoteDataChange()
            {
                FullPath = FullPath + key,
                FieldName = key,
                FieldValue = null,
                ApplyCallback = RemoveApply
            };
            AddChange(remove);
        }

        private void RemoveApply(RemoteDataChange change)
        {
            Object.Remove(change.FieldName);
            OnItemChanged(change.FieldName);
        }

        private void AddUpdateChange(string key, TValue value)
        {
            var update = new RemoteDataChange()
            {
                FullPath = FullPath + key,
                FieldName = key,
                FieldValue = value,
                ApplyCallback = UpdateApply
            };
            AddChange(update);
        }

        private void UpdateApply(RemoteDataChange change)
        {
            Object[change.FieldName] = (TValue)change.FieldValue;
            OnItemChanged(change.FieldName);
        }

        #region Observable logic
        private LinkedList<ObserverData<string>> _observers = new LinkedList<ObserverData<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            var token = new ObserverData<string>(observer);
            _observers.AddLast(token);
            token.SetParentData(_observers);
            return token;
        }

        #endregion
    }
}
