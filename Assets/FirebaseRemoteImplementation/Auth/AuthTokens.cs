using Facebook.Unity;
using GBG.Modules.RemoteData.Authorization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RemoteDataImpl.Auth.Tokens
{
    public class FacebookAuthToken : IAuthToken
    {
        public AccessToken Token { get; private set; }

        public FacebookAuthToken(AccessToken token)
        {
            this.Token = token;
        }
    }

    public class AnonymousAuthToken : IAuthToken
    {

    }

    public class EmailAuthToken : IAuthToken
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public EmailAuthToken(string email, string pass)
        {
            this.Email = email;
            this.Password = pass;
        }
    }
}
