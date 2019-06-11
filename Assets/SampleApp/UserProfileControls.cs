using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.MutableRemoteObjects;
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
        private BatchUpdater _batchUpdater;

        [SerializeField]
        private InputField _userIdInput;

        [SerializeField]
        private Text _profileText;

        private string _infoText;

        private void Update()
        {
            _profileText.text = _infoText;
        }

        public void Init(MutableObjectFactory factory, IAuthModule auth, BatchUpdater batchUpdater)
        {
            this._factory = factory;
            this._auth = auth;
            this._batchUpdater = batchUpdater;
        }

        public void FetchUserProfile()
        {
            this._ownProfile = _factory.CreateUserProfile(_auth.CurrentUserId);
            _ownProfile.KeyToVal.ItemChangedHandler += KeyToValItemChanged;
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
                    },
                    SomeChild = new SomeUserDataChild()
                    {
                        FieldA = "A",
                        FieldB = 0,
                        DeepChild = new DeeperUserDataChild()
                        {
                            FieldX = "X",
                            FieldY = "Y"
                        }                
                    }

                };
            }).ContinueWith((_) => { SetInfoText(); });

        }

        private void KeyToValItemChanged(string key)
        {
            Debug.Log("KeyVal changed :: Key :: " + key);
        }

        public void CommitSomeChanges()
        {
            _ownProfile.ReactiveGold.Value = Random.Range(0, 120);
            _ownProfile.ReactiveScore.Value = Random.Range(0, 75);
            _ownProfile.ReactiveUserName.Value = _ownProfile.ReactiveUserName.Value + Random.Range(0, 9).ToString();
            _ownProfile.SomeChild.FieldA.Value += _ownProfile.SomeChild.FieldA.Value + Random.Range(0, 9).ToString();
            _ownProfile.SomeChild.FieldB.Value = Random.Range(450, 715);
            _ownProfile.SomeChild.DeepChild.FieldX.Value += Random.Range(0, 30).ToString();
            _ownProfile.SomeChild.DeepChild.FieldY.Value += Random.Range(480, 920).ToString();

            _ownProfile.KeyToVal["Add" + Random.Range(0, 2).ToString()] = "!_()_!";

            _ownProfile.CommitChanges().ContinueWith((_)=> { SetInfoText(); });
        }

        public void CommitSomeChangesBatch()
        {
            _ownProfile.ReactiveGold.Value = Random.Range(0, 120);
            _ownProfile.ReactiveScore.Value = Random.Range(0, 75);
            _ownProfile.ReactiveUserName.Value = _ownProfile.ReactiveUserName.Value + Random.Range(0, 9).ToString();
            var changes = _ownProfile.FlushChanges();
            _batchUpdater.PerformBatchUpdate(changes).ContinueWith((_)=> SetInfoText());
        }

        private void SetInfoText()
        {
            _infoText = string.Format("Name :: {0}\nGold :: {1}\nScore::{2}\n\nKeyToVal :: {3}\nSomeChild :: {4}",
                _ownProfile.UserName,
                _ownProfile.Gold,
                _ownProfile.Score,
                JsonConvert.SerializeObject(_ownProfile.KeyToVal),
                JsonConvert.SerializeObject(_ownProfile.SomeChild.ToString()));
        }
    }
}
