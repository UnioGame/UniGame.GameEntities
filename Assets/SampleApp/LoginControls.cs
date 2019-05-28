using Facebook.Unity;
using RemoteDataModule.Authorization;
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
        this._auth = auth;
    }

    public void Update()
    {
        _loginInfoText.text = _loginInfoString; 
    }

    public void OnLoginFb()
    {
        SetInfoText("Start login FB");
        _auth.Login(AuthType.Facebook).ContinueWith((_)=> {
            ShowAuthData();
        });
    }

    public void OnLoginAnonymous()
    {
        _auth.Login(AuthType.Anonimous).ContinueWith((t)=> {
            if (t.IsFaulted)
                SetInfoText("Anonimous auth faulted");
            else
                ShowAuthData();
        });
    }

    public void OnLoginEditor()
    {
        // editor login with auto generated email/pass
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
