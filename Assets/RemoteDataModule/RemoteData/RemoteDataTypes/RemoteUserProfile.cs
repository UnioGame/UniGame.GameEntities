using UnityEngine;
using System.Collections;
using System;

namespace GBG.Modules.RemoteData.RemoteDataTypes
{
    [Serializable]
    public class RemoteUserProfile : UserReference
    {
        public RemoteUserProfile(string userId, string userName) : base(userId, userName)
        {
        }
    }
}
