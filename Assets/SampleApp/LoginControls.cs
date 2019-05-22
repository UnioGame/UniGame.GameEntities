using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginControls : MonoBehaviour
{
    [SerializeField]
    private Text _loginInfoText;

    private FirebaseAuthModule _auth;

    private void Start()
    {
        SetInfoText("...");
    }

    public void Init(FirebaseAuthModule auth)
    {
        this._auth = auth;
    }

    public void OnLoginFb()
    {
        SetInfoText("Start login FB");
        _auth.Login(FirebaseAuthModule.AuthType.Facebook).ContinueWith((_)=> { SetInfoText("FB login task completed"); });
    }

    public void OnLoginAnonymous()
    {

    }

    public void OnLoginEditor()
    {

    }

    public void OnLogout()
    {

    }

    private void SetInfoText(string text)
    {
        _loginInfoText.text = text;
    }
}
