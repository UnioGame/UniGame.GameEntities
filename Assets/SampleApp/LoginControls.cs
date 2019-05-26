using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginControls : MonoBehaviour
{
    [SerializeField]
    private Text _loginInfoText;

    private string _loginInfoString;

    private FirebaseAuthModule _auth;

    private void Start()
    {
        SetInfoText("...");
    }

    public void Init(FirebaseAuthModule auth)
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
        _auth.Login(FirebaseAuthModule.AuthType.Facebook).ContinueWith((_)=> {
            ShowAuthData();
        });
    }

    public void OnLoginAnonymous()
    {
        _auth.LoginAnonimously().ContinueWith((t)=> {
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
