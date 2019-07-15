using Facebook.Unity;
using RemoteDataImpl.Auth.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RemoteDataImpl.Auth.Providers
{
    public class DummyFacebookAuth : AbstractAuthTokenProvider<FacebookAuthToken>
    {
        public async override Task<FacebookAuthToken> FetchToken()
        {
            var fbLoginCompletionSrc = new TaskCompletionSource<ILoginResult>();
            FB.LogInWithReadPermissions(new List<string>() { "public_profile" }, callback: (result) =>
            {
                Debug.Log("Fb login operation finished. Result:" + result.RawResult);
                fbLoginCompletionSrc.SetResult(result);
            });
            await(fbLoginCompletionSrc.Task);
            if (fbLoginCompletionSrc.Task.IsFaulted)
            {
                Debug.Log("Fb login task faulted");
                return null;
            }
            Debug.Log("Fb login task completed");
            return new FacebookAuthToken(fbLoginCompletionSrc.Task.Result.AccessToken);
        }
    }
}
