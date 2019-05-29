using GBG.Modules.RemoteData.Authorization;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samples
{
    public class UserProfileControls : MonoBehaviour
    {
        private MutableObjectFactory _factory;
        private IAuthModule _auth;
        private MutableUserProfile _ownProfile;

        [SerializeField]
        private InputField _userIdInput;

        [SerializeField]
        private Text _profileText;

        private string _infoText;

        private void Update()
        {
            _profileText.text = _infoText;
        }

        public void Init(MutableObjectFactory factory, IAuthModule auth)
        {
            this._factory = factory;
            this._auth = auth;
        }

        public void FetchUserProfile()
        {
            this._ownProfile = _factory.CreateUserProfile(_auth.CurrentUserId);
            _ownProfile.LoadRootData(() =>
            {
                return new UserProfileData()
                {
                    Gold = 10,
                    UserName = "Test",
                    Score = 0,
                    KeyToVal = new Dictionary<string, string>()
                    {
                        {"a", "b" },
                        {"b", "c" }
                    }
                };
            }).ContinueWith((_) => { SetInfoText(); });

        }

        public void CommitSomeChanges()
        {
            _ownProfile.Gold = Random.Range(0, 120);
            _ownProfile.Score = Random.Range(0, 75);
            _ownProfile.UserName = _ownProfile.UserName + Random.Range(0, 9).ToString();
            _ownProfile.CommitChanges().ContinueWith((_)=> { SetInfoText(); });
        }

        private void SetInfoText()
        {
            _infoText = string.Format("Name :: {0}\nGold :: {1}\nScore::{2}\n\nKeyToVal :: {3}", 
                _ownProfile.UserName,
                _ownProfile.Gold,
                _ownProfile.Score,
                JsonConvert.SerializeObject(_ownProfile.KeyToVal));
        }
    }
}
