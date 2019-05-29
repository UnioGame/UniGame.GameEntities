using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Threading;
using UniRx.Async;
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

        private LinkedList<ObserverData<T>> _observers = new LinkedList<ObserverData<T>>();

        public void NotifyOnChange()
        {
            foreach(var _observer in _observers)
            {
                _observer._observer.OnNext(Value);
            }
        }

        public MutableObjectReactiveProperty(Func<T> getter, Action<T> setter )
        {
            _getter = getter;
            _setter = setter;
        }

        public T Value { get => _getter(); set => _setter(value); }

        public bool HasValue => true;

        T IReadOnlyReactiveProperty<T>.Value => Value;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var token = new ObserverData<T>(observer);
            _observers.AddLast(token);
            token.SetParentData(_observers);
            return token;
        }

        public UniTask<T> WaitUntilValueChangedAsync(CancellationToken cancellationToken)
        {
            // TO DO
            Debug.LogError("Not implemented");
            throw new NotImplementedException();
        }

        private class ObserverData<T> : IDisposable
        {
            private LinkedList<ObserverData<T>> _parentList;
            public IObserver<T> _observer;

            public ObserverData(IObserver<T> observer)
            {
                this._observer = observer;
            }

            public void SetParentData(LinkedList<ObserverData<T>> parentList)
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
}
