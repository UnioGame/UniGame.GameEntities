using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using UniRx;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Samples
{
    public class MutableUserProfile : BaseMutableRemoteObjectFacade<UserProfileData>
    {
        public MutableUserProfile(RemoteObjectHandler<UserProfileData> objectHandler) : base(objectHandler)
        {
            ReactiveUserName = CreateReactiveProperty<string>(
                () => _objectHandler.Object.UserName,
                (value) => UpdateChildData(nameof(_objectHandler.Object.UserName), value),
                nameof(_objectHandler.Object.UserName));

            ReactiveScore = CreateReactiveProperty<int>(
                () => _objectHandler.Object.Score,
                (value) => UpdateChildData(nameof(_objectHandler.Object.Score), value),
                nameof(_objectHandler.Object.Score));

            ReactiveGold = CreateReactiveProperty<int>(
                () => _objectHandler.Object.Gold,
                (value) => UpdateChildData(nameof(_objectHandler.Object.Gold), value),
                nameof(_objectHandler.Object.Gold));
        }

        public IReactiveProperty<string> ReactiveUserName { get; private set; }
        public IReactiveProperty<int> ReactiveScore { get; private set; }
        public IReactiveProperty<int> ReactiveGold { get; private set; }

        public string UserName
        {
            get { return _objectHandler.Object.UserName; }
            set { UpdateChildData(nameof(_objectHandler.Object.UserName), value); }
        }

        public int Score
        {
            get { return _objectHandler.Object.Score; }
            set { UpdateChildData(nameof(_objectHandler.Object.Score), value); }
        }

        public int Gold
        {
            get { return _objectHandler.Object.Gold; }
            set { UpdateChildData(nameof(_objectHandler.Object.Gold), value); }
        }

        // TO DO create dictionary dispatching changes
        public IDictionary<string, string> KeyToVal
        {
            get { return _objectHandler.Object.KeyToVal; }
            set { UpdateChildData(nameof(_objectHandler.Object.KeyToVal), value); }
        }

    }
}
