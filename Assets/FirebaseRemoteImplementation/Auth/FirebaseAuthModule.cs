using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using GBG.Modules.RemoteData.Authorization;
using RemoteDataImpl.Auth.Tokens;
using System.Linq;
using Facebook.Unity;

namespace GBG.Modules.RemoteData.FirebaseImplementation
{
    public class FirebaseAuthModule : IAuthModule
    {
        private Firebase.FirebaseApp _app;
        private Firebase.Auth.FirebaseAuth _auth;

        public string CurrentUserId
        {
            get { return FirebaseAuth.DefaultInstance.CurrentUser.UserId; }
        }

        public bool IsLogged
        {
            get { return FirebaseAuth.DefaultInstance.CurrentUser != null; }
        }

        public async Task Init()
        {
            await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((_) =>
            {
                _app = Firebase.FirebaseApp.DefaultInstance;
                _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            });
        }

        public async Task Login(IAuthToken authToken)
        {
            switch (authToken)
            {
                case AnonymousAuthToken _:
                    await LoginAnonymously();
                    break;
                case FacebookAuthToken facebookToken:
                    await LoginFacebook(facebookToken.Token);
                    break;
                case EmailAuthToken emailToken:
                    await LoginEmail(emailToken);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task Logout()
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }

        protected async Task LoginEmail(EmailAuthToken emailToken)
        {
            var providers = await FirebaseAuth.DefaultInstance.FetchProvidersForEmailAsync(emailToken.Email);
            if (providers.Any())
                await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailToken.Email, emailToken.Password);
            else
                await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailToken.Email, emailToken.Password);

            UnityEngine.Debug.LogFormat("Email Auth: email={0}, pass={1}", emailToken.Email, emailToken.Password);
        }

        protected async Task LoginAnonymously()
        {
            await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
        }

        protected async Task LoginFacebook(AccessToken facebookToken)
        {
            var credential = FacebookAuthProvider.GetCredential(facebookToken.TokenString);
            await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
            Debug.Log("Firebase fb login complete :: UserID :: " + CurrentUserId);
        }
    }
}
