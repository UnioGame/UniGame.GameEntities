using UnityEngine;
using System.Collections;
using System;

namespace RemoteDataTypes
{
    [Serializable]
    public class RemoteUserProfile : UserReference
    {
        // SomeTestFields

        public RemoteUserProfile(string userId, string userName) : base(userId, userName)
        {
        }
    }
}
