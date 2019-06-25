using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public interface IMutableChildBase : IRemoteChangesStorage
    {

    }

    public class MutableChild<T> : IMutableChildBase
    {
        public string FullPath { get; private set; }
        protected T Object { get => _getter(); }

        private Func<T> _getter;
        protected IRemoteChangesStorage _storage;
        private Dictionary<string, INotifyable> _properties;
        private Dictionary<string, IMutableChildBase> _childObjects;

        public MutableChild(Func<T> getter, string fullPath, IRemoteChangesStorage storage)
        {
            this._getter = getter;
            this.FullPath = fullPath;
            this._storage = storage;
            _properties = new Dictionary<string, INotifyable>();
            _childObjects = new Dictionary<string, IMutableChildBase>();
        }

        public void UpdateChildData(string fieldName, object newValue)
        {
            _storage.AddChange(new RemoteDataChange()
            {
                FieldName = fieldName,
                FullPath = FullPath + fieldName,
                FieldValue = newValue,
                ApplyCallback = ApplyChangeLocal
            });
        }

        private void ApplyChangeLocal(RemoteDataChange change)
        {
            var fieldInfo = typeof(T).GetField(change.FieldName);
            fieldInfo.SetValue(Object, change.FieldValue);
            PropertyChanged(change.FieldName);
        }

        protected MutableObjectReactiveProperty<Tvalue> CreateReactiveProperty<Tvalue>(Func<Tvalue> getter, Action<Tvalue> setter, string fieldName)
        {
            var property = new MutableObjectReactiveProperty<Tvalue>(getter, setter, this);
            _properties.Add(fieldName, property);
            return property;
        }

        protected void PropertyChanged(string name)
        {
            if (_properties.ContainsKey(name))
                _properties[name].NotifyOnChange();
        }

        protected void AllPropertiesChanged()
        {
            foreach (var property in _properties.Values)
                property.NotifyOnChange();
        }

        public void AddChange(RemoteDataChange change)
        {
            _storage.AddChange(change);
        }

        public bool IsRootLoaded()
        {
            return _storage.IsRootLoaded();
        }
    }
}
