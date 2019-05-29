using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Samples
{
    [Serializable]
    public class UserProfileData
    {
        public string UserName;
        public int Score;
        public int Gold;
        public Dictionary<string, string> KeyToVal;
    }
}
