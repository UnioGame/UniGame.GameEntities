using GBG.Modules.RemoteData.Authorization;
using RemoteDataImpl.Auth.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RemoteDataImpl.Auth
{
    public class EmailAuth : AbstractAuthTokenProvider<EmailAuthToken>
    {
        private const string EMAIL_PREFS_KEY = "EmailAuth.email";

        private string userEmail
        {
            get => PlayerPrefs.GetString(EMAIL_PREFS_KEY, null);
            set => PlayerPrefs.SetString(EMAIL_PREFS_KEY, value);
        }

        public async override Task<EmailAuthToken> FetchToken()
        {
            if (string.IsNullOrEmpty(userEmail))
                userEmail = CreateEmail();

            var pass = CreatePass(userEmail);
            return new EmailAuthToken(userEmail, pass);
        }

        private static string CreateEmail()
        {
#if UNITY_EDITOR
            var cloudEmail = UnityEditor.CloudProjectSettings.userName;
            var cloudName = cloudEmail.Split('@')[0];

            var email = cloudName + "-" + Guid.NewGuid() + "@editor.editor";
            return email;
#else
            throw new NotImplementedException();
#endif
        }

        private static string CreatePass(string email)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, email + "FTK_pass_salt");
            }
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
