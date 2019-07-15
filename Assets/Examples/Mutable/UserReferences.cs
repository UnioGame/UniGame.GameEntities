using UnityEngine;
using System.Collections;
using System;

namespace Samples.Data
{
    [Serializable]
    public class UserReference
    {
        public string Id;
        public string Name;

        public UserReference(string userId, string userName)
        {
            Id = userId;
            Name = userName;
        }
    }

    [SerializeField]
    public class ClanUserReference
    {
        public string Id;
        public string Name;
        public string Role;
    }

    [Serializable]
    public class ScoredUserReference : UserReference
    {
        public ScoredUserReference(int score, string userId, string userName) : base(userId, userName)
        {
            Score = score;
        }

        public int Score;
    }

}

