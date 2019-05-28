using UnityEngine;
using System.Collections;
using RemoteDataModule.MutableRemoteObjects;
using RemoteDataModule.RemoteDataAbstracts;
using System.Collections.Generic;
using RemoteDataModule.RemoteDataTypes;

namespace Samples {
    public class MutableClanData : BaseMutableRemoteObjectFacade<ClanData>
    {
        public MutableClanData(RemoteObjectHandler<ClanData> objectHandler) : base(objectHandler)
        {
        }

        public string ClanName
        {
            get { return _objectHandler.Object.ClanName; }
            set { UpdateChildData(nameof(_objectHandler.Object.ClanName), value); }
        }

        public int ClanRating
        {
            get { return _objectHandler.Object.ClanRating; }
            set { UpdateChildData(nameof(_objectHandler.Object.ClanRating), value); }
        }

        /// <summary>
        /// DO NOT MODIFY LIST
        /// </summary>
        public List<ClanUserReference> userReference{
            get { return _objectHandler.Object.Users; }
            set { UpdateChildData(nameof(_objectHandler.Object.Users), value); }
        }

    }
}
