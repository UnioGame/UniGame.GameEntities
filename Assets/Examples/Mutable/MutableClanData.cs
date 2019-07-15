using GBG.Modules.RemoteData.MutableRemoteObjects;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using Samples.Data;
using System.Collections.Generic;

namespace Samples
{
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
        /// List modifications not allowed, only setting new list
        /// </summary>
        public List<ClanUserReference> userReference{
            get { return _objectHandler.Object.Users; }
            set { UpdateChildData(nameof(_objectHandler.Object.Users), value); }
        }

    }
}
