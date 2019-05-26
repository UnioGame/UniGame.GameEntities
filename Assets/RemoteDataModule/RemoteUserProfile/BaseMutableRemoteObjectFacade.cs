using UnityEngine;
using System.Collections;
using RemoteDataModule.RemoteDataAbstracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RemoteDataModule.RemoteUserProfile
{
    public class BaseMutableRemoteObjectFacade<T> where T : class
    {
        private RemoteObjectHandler<T> _objectHandler;

        private List<RemoteDataChange> _pendingChanges;

        public BaseMutableRemoteObjectFacade(RemoteObjectHandler<T> objectHandler)
        {
            this._objectHandler = objectHandler;
            _pendingChanges = new List<RemoteDataChange>();
        }

        public async Task LoadRootData()
        {
            await _objectHandler.LoadData();
        }

        public void UpdateChildData(string childName, object newData)
        {
            var change = _objectHandler.CreateChange(childName, newData);
            _pendingChanges.Add(change);
        }

        public async Task CommitChanges()
        {
            // TO DO Async
            // TO DO transaction
            List<Task> updateTasks = new List<Task>();
            foreach (var change in _pendingChanges)
                updateTasks.Add(_objectHandler.ApplyChange(change));
            await Task.WhenAll(updateTasks.ToArray());
            _pendingChanges.Clear();
        }
    }
}
