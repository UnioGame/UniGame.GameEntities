using Facebook.Unity;
using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.FirebaseImplementation;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using GOIFirebase.RemoteDataImpl;
using Samples.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples
{
    public class QuestsApplication : MonoBehaviour
    {
        [SerializeField]
        private LoginControls _loginControls;
        [SerializeField]
        private UserProfileControls _profileControls;
        [SerializeField]
        private QuestControls _questControls;

        private IAuthModule _authModule;
        private TextSharedMessageProcessor _textProcessor;
        private RemoteObjectsProvider _remoteObjectsProvider;
        private MutableObjectFactory _mutableObjectFactory;
        private BatchUpdater _batchUpdater;

        void Start()
        {
            _authModule = new FirebaseAuthModule();
            FB.Init();
            _authModule.Init();

            _loginControls.Init(_authModule);

            _remoteObjectsProvider = new FirebaseRemoteDataProvider();
            _mutableObjectFactory = new MutableObjectFactory(_remoteObjectsProvider);
            _batchUpdater = new FirebaseBatchUpdater();
            _profileControls.Init(_mutableObjectFactory, _authModule, _batchUpdater);
        }

    }
}
