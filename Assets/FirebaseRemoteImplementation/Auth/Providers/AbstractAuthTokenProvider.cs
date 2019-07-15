using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemoteDataImpl.Auth.Tokens;
using System.Threading.Tasks;
using GBG.Modules.RemoteData.Authorization;

namespace RemoteDataImpl.Auth.Providers
{
    public abstract class AbstractAuthTokenProvider<T> where T : class, IAuthToken
    {
        public abstract Task<T> FetchToken();
    }
}
