using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private LoginControls _loginControls;
        
        private FirebaseAuthModule _authModule;

        void Start()
        {
            _authModule = new FirebaseAuthModule();
            _authModule.Init();
            _loginControls.Init(_authModule);
        }


        public void Auth(FirebaseAuthModule.AuthType authType)
        {
            var task = _authModule.Login(authType);
        }
    }
}
