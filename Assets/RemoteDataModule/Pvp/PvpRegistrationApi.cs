using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections.Generic;

namespace GBG.Modules.RemoteData.Pvp
{
    public interface IPvpRegistrationApi
    {
        Task RegisterUserForPvp(string userId);
        Task UnregisterUserForPvp(string userId);
        RemoteObjectHandler<PvpPoolData> GetPoolHandler();
        RemoteObjectHandler<Dictionary<string, bool>> GetRoomHandler(int id);
    }
}
