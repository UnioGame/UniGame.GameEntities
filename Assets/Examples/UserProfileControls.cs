using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Samples
{
    public class UserProfileControls : MonoBehaviour
    {
        private MutableObjectFactory _factory;
        private IAuthModule _auth;
        public MutableUserProfile _ownProfile;
        private BatchUpdater _batchUpdater;

        [SerializeField]
        private InputField _userIdInput;

        [SerializeField]
        private Text _profileText;

        [Space]
        [SerializeField]
        private InputField _scoreInput;
        [SerializeField]
        private Button _scoreInputSubmit;

        private string _infoText;

        private void Awake()
        {
            _scoreInputSubmit.onClick.AddListener(SubmitScore);
        }

        private void SubmitScore()
        {
            var value = int.Parse(_scoreInput.text);
            _ownProfile.ReactiveScore.Value += value;
            _ownProfile.CommitChanges().ContinueWith((_) => { SetInfoText(); });
        }

        private void Update()
        {
            _profileText.text = _infoText;
            if (_ownProfile != null)
                SetInfoText();
        }

        public void Init(MutableObjectFactory factory, IAuthModule auth, BatchUpdater batchUpdater)
        {
            _factory = factory;
            _auth = auth;
            _batchUpdater = batchUpdater;
        }

        public void FetchUserProfile()
        {
            _ownProfile = _factory.CreateUserProfile(_auth.CurrentUserId);
            _ownProfile.KeyToVal.Subscribe((v) => { KeyToValItemChanged(v); });
            _ownProfile.ReactiveGold.Subscribe((v) => Debug.LogError("VALUE :: " + v.ToString()));
            _ownProfile.SomeList.ObserveReplace().Subscribe((e) => Debug.LogError(string.Format("LIST :: old = {0} :: new = {1} ", e.OldValue, e.NewValue)));
            _ownProfile.LoadRootData(initialDataProvider: () => new UserProfileData
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
                },
                SomeList = new List<string>()
                {
                    "Bob",
                    "Sam",
                    "Homer"
                }

            }).ContinueWith(_ => SetInfoText());
            StartCoroutine(subsCoroutine());
        }

        private IEnumerator subsCoroutine()
        {
            yield return new WaitForSeconds(8.0f);
            _ownProfile.ReactiveUserName.Subscribe((v) => Debug.LogError("SASAT " + v.ToString()));
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

            _ownProfile.SomeList[Random.Range(0, 2)] = "Add" + Random.Range(0, 2).ToString();

            _ownProfile.CommitChanges().ContinueWith((_) => { SetInfoText(); });
        }

        public void CommitSomeChangesBatch()
        {
            _ownProfile.ReactiveGold.Value = Random.Range(0, 120);
            _ownProfile.ReactiveScore.Value = Random.Range(0, 75);
            _ownProfile.ReactiveUserName.Value = _ownProfile.ReactiveUserName.Value + Random.Range(0, 9).ToString();
            var changes = _ownProfile.FlushChanges();
            _batchUpdater.PerformBatchUpdate(changes).ContinueWith((_) => SetInfoText());
        }

        private void SetInfoText()
        {
            _infoText = string.Format("Name :: {0}\nGold :: {1}\nScore::{2}\n\nKeyToVal :: {3}\nSomeChild :: {4}\nQuests :: {5}",
                   _ownProfile.UserName,
                   _ownProfile.Gold,
                   _ownProfile.Score,
                   JsonConvert.SerializeObject(_ownProfile.KeyToVal),
                   JsonConvert.SerializeObject(_ownProfile.SomeChild.ToString()),
                   JsonConvert.SerializeObject(_ownProfile.Quests));
        }
    }
}
