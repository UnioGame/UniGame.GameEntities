using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace GBG.Modules.RemoteData.Pvp
{
    public interface IPvpRegistrationApi
    {
        Task RegisterUserForPvp(string userId);
        Task UnregisterUserForPvp(string userId);
    }
}
