using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UniRx;
using System.Threading;
using UniRx.Async;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public class BaseMutableRemoteObjectFacade<T> where T : class
    {
        protected RemoteObjectHandler<T> _objectHandler;

        private List<RemoteDataChange> _pendingChanges;

        private Dictionary<string, INotifyable> _properties;

        public BaseMutableRemoteObjectFacade(RemoteObjectHandler<T> objectHandler)
        {
            this._objectHandler = objectHandler;
            _pendingChanges = new List<RemoteDataChange>();
            _properties = new Dictionary<string, INotifyable>();
        }

        /// <summary>
        /// Loads remote data. if not exits sets initialValue
        /// </summary>
        /// <param name="initialDataProvider"></param>
        /// <returns></returns>
        public async Task LoadRootData(Func<T> initialDataProvider = null){

            await _objectHandler.LoadData(initialDataProvider: initialDataProvider);
            AllPropertiesChanged();
        }

        public string GetId()
        {
            return _objectHandler.GetDataId();
        }

        public void UpdateChildData(string childName, object newData)
        {
            var change = _objectHandler.CreateChange(childName, newData);
            _pendingChanges.Add(change);
        }
        
        public async Task CommitChanges()
        {
            // TO DO transaction
            List<Task> updateTasks = new List<Task>();
            var changes = _pendingChanges;
            _pendingChanges = new List<RemoteDataChange>();
            foreach (var change in changes)
            {
                var fieldName = change.FieldName;
                updateTasks.Add(_objectHandler.ApplyChange(change).ContinueWith((_)=> { PropertyChanged(fieldName); }));
            }
            await Task.WhenAll(updateTasks.ToArray());
            _pendingChanges.Clear();
        }

        public List<RemoteDataChange> FlushChanges()
        {
            var result = _pendingChanges;
            _pendingChanges = new List<RemoteDataChange>();
            foreach(var change in result)
            {
                change.ApplyCallback = ChangeAppliedOutside;
            }
            return result;
        }

        public void ChangeAppliedOutside(RemoteDataChange change)
        {
            _objectHandler.ApplyChangeLocal(change);
            PropertyChanged(change.FieldName);
        }

        public MutableObjectReactiveProperty<Tvalue> CreateReactiveProperty<Tvalue>(Func<Tvalue> getter, Action<Tvalue> setter, string fieldName)
        {
            var property = new MutableObjectReactiveProperty<Tvalue>(getter, setter);
            _properties.Add(fieldName, property);
            return property;
        }

        protected void PropertyChanged(string name)
        {
            _properties[name].NotifyOnChange();
        }

        protected void AllPropertiesChanged()
        {
            foreach (var property in _properties.Values)
                property.NotifyOnChange();
        }
    }
}
