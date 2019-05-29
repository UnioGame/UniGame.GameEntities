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
        }

        public string UserName {
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
