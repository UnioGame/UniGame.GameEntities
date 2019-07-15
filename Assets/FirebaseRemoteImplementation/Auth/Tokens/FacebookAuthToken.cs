using Facebook.Unity;
using GBG.Modules.RemoteData.Authorization;

namespace RemoteDataImpl.Auth.Tokens
{
    public class FacebookAuthToken : IAuthToken
    {
        public AccessToken Token { get; private set; }

        public FacebookAuthToken(AccessToken token)
        {
            Token = token;
        }
    }
}