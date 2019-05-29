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

        public BaseMutableRemoteObjectFacade(RemoteObjectHandler<T> objectHandler)
        {
            this._objectHandler = objectHandler;
            _pendingChanges = new List<RemoteDataChange>();
        }

        /// <summary>
        /// Loads remote data. if not exits sets initialValue
        /// </summary>
        /// <param name="initialDataProvider"></param>
        /// <returns></returns>
        public async Task LoadRootData(Func<T> initialDataProvider = null){

            await _objectHandler.LoadData(initialDataProvider: initialDataProvider);
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
                updateTasks.Add(_objectHandler.ApplyChange(change));
            await Task.WhenAll(updateTasks.ToArray());
            _pendingChanges.Clear();
        }
    }
}
