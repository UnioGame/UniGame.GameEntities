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
        public SomeUserDataChild SomeChild;
    }

    public class SomeUserDataChild
    {
        public string FieldA;
        public int FieldB;
        public DeeperUserDataChild DeepChild;
    }

    public class DeeperUserDataChild
    {
        public string FieldX;
        public string FieldY;
    }
}
