using GBG.Modules.RemoteData.Pvp;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GBG.Modules.RemoteData.FirebaseImplementation
{
    public class FirebasePvpQueries : IPvpRegistrationApi
    {
        private const string REGISTER_FUNCTION_NAME = "registerUserForPvp";
        private const string UNREGISTER_FUNCTION_NAME = "unregisterUserForPvp";
        private const string USER_ID_PARAMETER = "UserId";

        private RemoteObjectsProvider _provider;

        public FirebasePvpQueries(RemoteObjectsProvider provider)
        {
            _provider = provider;
        }

        public RemoteObjectHandler<PvpPoolData> GetPoolHandler()
        {
            return _provider.GetRemoteObject<PvpPoolData>("PvpPool/RoomsMeta");
        }

        public RemoteObjectHandler<Dictionary<string, bool>> GetRoomHandler(int id)
        {
            return _provider.GetRemoteObject<Dictionary<string, bool>>("PvpPool/Rooms/"+id.ToString());
        }

        public async Task RegisterUserForPvp(string userId)
        {
            var callable = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable(REGISTER_FUNCTION_NAME);
            var @params = new Dictionary<string, object>();
            @params.Add(USER_ID_PARAMETER, userId);
            await callable.CallAsync(@params);
        }

        public async Task UnregisterUserForPvp(string userId)
        {
            var callable = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable(UNREGISTER_FUNCTION_NAME);
            var @params = new Dictionary<string, object>();
            @params.Add(USER_ID_PARAMETER, userId);
            await callable.CallAsync(@params);
        }
    }
}
