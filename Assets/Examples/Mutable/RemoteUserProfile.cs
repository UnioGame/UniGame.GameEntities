using UnityEngine;
using System.Collections;
using System;

namespace Samples.Data
{
    [Serializable]
    public class RemoteUserProfile : UserReference
    {
        public RemoteUserProfile(string userId, string userName) : base(userId, userName)
        {
        }
    }
}
