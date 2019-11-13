using Facebook.Unity;
using GBG.Modules.RemoteData.Authorization;
using RemoteDataImpl.Auth;
using RemoteDataImpl.Auth.Providers;
using RemoteDataImpl.Auth.Tokens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginControls : MonoBehaviour
{
    [SerializeField]
    private Text _loginInfoText;

    private string _loginInfoString;

    private IAuthModule _auth;

    private void Start()
    {
        SetInfoText("...");
    }

    public void Init(IAuthModule auth)
    {
        _auth = auth;
    }

    public void Update()
    {
        _loginInfoText.text = _loginInfoString;
    }

    public void OnLoginFb()
    {
        SetInfoText("Start login FB");
        var FacebookAuth = new DummyFacebookAuth();
        FacebookAuth.FetchToken().ContinueWith((task) =>
        {
            _auth.Login(task.Result).ContinueWith((_) =>
            {
                ShowAuthData();
            });
        });
    }

    public void OnLoginAnonymous()
    {
        _auth.Login(new AnonymousAuthToken()).ContinueWith((t) =>
        {
            if (t.IsFaulted)
                SetInfoText("Anonimous auth faulted");
            else
                ShowAuthData();
        });
    }

    public void OnLoginEditor()
    {
        var emailAuth = new EmailAuth();
        emailAuth.FetchToken().ContinueWith((task) =>
        {
            _auth.Login(task.Result).ContinueWith((t) =>
            {
                ShowAuthData();
            });
        });
    }

    public void OnLogout()
    {
        // Logout from firebase and all connected login sources
    }

    private void ShowAuthData()
    {
        SetInfoText(string.Format("Auth data :: \nUserId :: {0}", _auth.CurrentUserId));
    }

    private void SetInfoText(string text)
    {
        _loginInfoString = text;
    }
}
