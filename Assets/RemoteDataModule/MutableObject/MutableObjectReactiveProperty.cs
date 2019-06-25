using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Threading;
using System.Collections.Generic;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public interface INotifyable
    {
        void NotifyOnChange();
    }

    public class MutableObjectReactiveProperty<T> : IReactiveProperty<T>, INotifyable
    {
        private Func<T> _getter;
        private Action<T> _setter;
        private IRemoteChangesStorage _storage;

        private LinkedList<ObserverData<T>> _observers = new LinkedList<ObserverData<T>>();

        public void NotifyOnChange()
        {
            foreach (var _observer in _observers)
            {
                _observer._observer.OnNext(Value);
            }
        }

        public MutableObjectReactiveProperty(Func<T> getter, Action<T> setter, IRemoteChangesStorage storage)
        {
            _getter = getter;
            _setter = setter;
            _storage = storage;
        }

        public T Value { get => _getter(); set => _setter(value); }

        public bool HasValue => true;

        T IReadOnlyReactiveProperty<T>.Value => Value;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var token = new ObserverData<T>(observer);
            _observers.AddLast(token);
            token.SetParentData(_observers);
            if (_storage.IsRootLoaded())
                observer.OnNext(Value);
            return token;
        }
    }

    internal class ObserverData<TData> : IDisposable
    {
        private LinkedList<ObserverData<TData>> _parentList;
        public IObserver<TData> _observer;

        public ObserverData(IObserver<TData> observer)
        {
            this._observer = observer;
        }

        public void SetParentData(LinkedList<ObserverData<TData>> parentList)
        {
            _parentList = parentList;
        }

        public void Dispose()
        {
            _observer = null;
            lock (_parentList)
            {
                _parentList.Remove(this);
            }
        }
    }
}
