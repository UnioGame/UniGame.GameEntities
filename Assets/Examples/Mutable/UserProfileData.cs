using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GBG.Modules.Quests.Data;

namespace Samples
{
    [Serializable]
    public class UserProfileData
    {
        public string UserName;
        public int Score;
        public int Gold;
        public string Clan;
        public Dictionary<string, string> KeyToVal;
        public SomeUserDataChild SomeChild;
        public List<string> SomeList;
        public Dictionary<string, QuestData> Quests = new Dictionary<string, QuestData>();
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
