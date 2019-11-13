using GBG.Modules.RemoteData.Authorization;

namespace RemoteDataImpl.Auth.Tokens
{
    public class EmailAuthToken : IAuthToken
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public EmailAuthToken(string email, string pass)
        {
            Email = email;
            Password = pass;
        }
    }
}
