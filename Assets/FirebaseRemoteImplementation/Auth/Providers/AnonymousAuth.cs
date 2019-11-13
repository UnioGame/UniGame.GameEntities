using UnityEngine;
using System.Collections;
using RemoteDataImpl.Auth.Tokens;
using System.Threading.Tasks;

namespace RemoteDataImpl.Auth.Providers
{
    public class AnonymousAuth : AbstractAuthTokenProvider<AnonymousAuthToken>
    {
        public async override Task<AnonymousAuthToken> FetchToken()
        {
            return new AnonymousAuthToken();
        }
    }
}
