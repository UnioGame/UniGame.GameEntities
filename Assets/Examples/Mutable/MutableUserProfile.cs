using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using UniRx;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using GBG.Modules.Quests.Data;

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

            SomeChild = new MutableUserDataChild(
                () => _objectHandler.Object.SomeChild,
                _objectHandler.GetFullPath() + nameof(_objectHandler.Object.SomeChild) + "/",
                this);
            RegisterMutableChild(nameof(_objectHandler.Object.SomeChild), SomeChild);

            KeyToVal = new MutableDictionary<string>(
                () => _objectHandler.Object.KeyToVal,
                _objectHandler.GetFullPath() + nameof(_objectHandler.Object.KeyToVal) + "/",
                this);
            RegisterMutableChild(nameof(_objectHandler.Object.KeyToVal), KeyToVal);

            Quests = new MutableDictionary<QuestData>(
                () => _objectHandler.Object.Quests,
                _objectHandler.GetFullPath() + nameof(_objectHandler.Object.Quests) + "/",
                this);

            var someList = new MutableList<string>(
                () => _objectHandler.Object.SomeList,
                _objectHandler.GetFullPath() + nameof(_objectHandler.Object.SomeList) + "/",
                this);
            RegisterMutableChild(nameof(_objectHandler.Object.SomeList), someList);
            SomeList = someList;
        }

        public IReactiveProperty<string> ReactiveUserName { get; private set; }
        public IReactiveProperty<int> ReactiveScore { get; private set; }
        public IReactiveProperty<int> ReactiveGold { get; private set; }

        public MutableUserDataChild SomeChild { get; private set; }

        public IReactiveCollection<string> SomeList { get; private set; }

        public MutableDictionary<QuestData> Quests { get; private set; }

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

        public MutableDictionary<string> KeyToVal { get; private set; }

    }
}
