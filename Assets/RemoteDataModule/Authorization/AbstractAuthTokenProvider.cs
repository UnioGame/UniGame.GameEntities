using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace GBG.Modules.RemoteData.Authorization
{
    public abstract class AbstractAuthTokenProvider<T> where T : class, IAuthToken
    {
        public abstract Task<T> FetchToken();
    }
}
