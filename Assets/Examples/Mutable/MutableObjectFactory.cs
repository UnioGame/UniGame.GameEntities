using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.RemoteDataAbstracts;

namespace Samples
{
    public class MutableObjectFactory
    {
        private RemoteObjectsProvider _objectsProvider;

        public MutableObjectFactory(RemoteObjectsProvider objectProvider )
        {
            _objectsProvider = objectProvider;
        }

        public MutableUserProfile CreateUserProfile(string UserId)
        {
            var obj = _objectsProvider.GetRemoteObject<UserProfileData>(string.Format("Users/{0}", UserId));
            return new MutableUserProfile(obj);
        }

        public MutableClanData GetClanProfile(string clanId = null)
        {
            if (clanId == null)
                clanId =_objectsProvider.GetIdForNewObject("Clans/");
            var obj = _objectsProvider.GetRemoteObject<ClanData>(string.Format("Clans/{0}", clanId));
            return new MutableClanData(obj);
        }
    }
}
