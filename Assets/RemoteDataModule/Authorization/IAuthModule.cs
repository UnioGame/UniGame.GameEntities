using System.Threading.Tasks;

namespace GBG.Modules.RemoteData.Authorization
{
    public enum AuthType
    {
        Anonimous,
        Facebook
    }
    public interface IAuthModule
    {
        // TO DO other Auth Type
        // Init with providers for different networks

        string CurrentUserId { get; }
        bool IsLogged { get; }

        void Init();
        Task Login(AuthType authType);
        Task Logout();
    }
}