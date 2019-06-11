using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public class BaseMutableRemoteObjectFacade<T> : IRemoteChangesStorage where T : class
    {
        protected RemoteObjectHandler<T> _objectHandler;

        private List<RemoteDataChange> _pendingChanges;

        private Dictionary<string, INotifyable> _properties;

        private Dictionary<string, IMutableChildBase> _childObjects;

        public BaseMutableRemoteObjectFacade(RemoteObjectHandler<T> objectHandler)
        {
            this._objectHandler = objectHandler;
            _pendingChanges = new List<RemoteDataChange>();
            _properties = new Dictionary<string, INotifyable>();
            _childObjects = new Dictionary<string, IMutableChildBase>();
        }

        /// <summary>
        /// Loads remote data. if not exits sets initialValue
        /// </summary>
        /// <param name="initialDataProvider"></param>
        /// <returns></returns>
        public async Task LoadRootData(Func<T> initialDataProvider = null)
        {

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
            change.ApplyCallback = ApplyChangeOnLocalHandler;
            AddChange(change);
        }

        public void AddChange(RemoteDataChange change)
        {
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
                updateTasks.Add(_objectHandler.ApplyChange(change).ContinueWith((_) => { ChangeApplied(change); }));
            }
            await Task.WhenAll(updateTasks.ToArray());
            _pendingChanges.Clear();
        }

        public List<RemoteDataChange> FlushChanges()
        {
            var result = _pendingChanges;
            _pendingChanges = new List<RemoteDataChange>();
            return result;
        }

        public void ChangeApplied(RemoteDataChange change)
        {
            change.ApplyCallback(change);
        }

        private void ApplyChangeOnLocalHandler(RemoteDataChange change)
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

        public void RegisterMutableChild(string childName, IMutableChildBase child)
        {
            _childObjects.Add(childName, child);
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
    }
}
