using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Facebook.Unity;

public class FirebaseAuthModule
{
    // TO DO other Auth Type
    public enum AuthType
    {
        Anonimous,
        Facebook
    }

    public string CurrentUserId
    {
        get { return FirebaseAuth.DefaultInstance.CurrentUser.UserId; }
    }

    public bool IsLogged
    {
        get { return FirebaseAuth.DefaultInstance.CurrentUser != null; }
    }

    public void Init()
    {
        FB.Init();
    }

    public async Task Login(AuthType authType)
    {
        switch (authType)
        {
            case AuthType.Anonimous:
                await LoginAnonimously();
                break;
            case AuthType.Facebook:
                await LoginFacebook();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public async Task LoginAnonimously()
    {
        await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
    }

    public async Task LoginFacebook()
    {
        var fbLoginCompletionSrc = new TaskCompletionSource<ILoginResult>();
        FB.LogInWithReadPermissions(new List<string>() { "public_profile" }, callback: (result) =>
         {
             Debug.Log("Fb login operation finished. Result:" + result.RawResult);
             fbLoginCompletionSrc.SetResult(result);
         });
        await (fbLoginCompletionSrc.Task);
        if (fbLoginCompletionSrc.Task.IsFaulted)
        {
            Debug.Log("Fb login task faulted");
            // TO DO spaw event
            return;
        }
        Debug.Log("Fb login task completed");
        var token = fbLoginCompletionSrc.Task.Result.AccessToken;
        var credential = FacebookAuthProvider.GetCredential(token.TokenString);
        await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
        Debug.Log("Firebase fb login complete");
        // TO DO
        // Получить фбшный токен и по нему логинится в фаербейс
        //await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync()
    }
}
