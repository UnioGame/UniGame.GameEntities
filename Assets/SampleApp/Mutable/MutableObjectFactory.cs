using UnityEngine;
using System.Collections;
using RemoteDataModule.Authorization;
using RemoteDataModule.RemoteDataAbstracts;

namespace Samples
{
    public class MutableObjectFactory
    {
        private RemoteObjectsProvider _objectsProvider;

        public MutableObjectFactory(RemoteObjectsProvider objectProvider )
        {
            this._objectsProvider = objectProvider;
        }

        public MutableUserProfile CreateUserProfile(string UserId)
        {
            var obj = _objectsProvider.GetRemoteObject<UserProfileData>(string.Format("Users/{0}", UserId));
            return new MutableUserProfile(obj);
        }
    }
}
