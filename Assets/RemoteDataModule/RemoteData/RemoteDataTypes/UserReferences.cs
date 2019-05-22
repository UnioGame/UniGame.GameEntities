using UnityEngine;
using System.Collections;
using System;
namespace RemoteDataTypes
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
