using Facebook.Unity;
using GOIFirebase.RemoteDataImpl;
using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.FirebaseImplementation;
using GBG.Modules.RemoteData.FirebaseImplementation.SharedMessages;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using GBG.Modules.RemoteData.SharedMessages;
using GBG.Modules.RemoteData.SharedMessages.MessageData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using Samples.Messages;

namespace Samples
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private LoginControls _loginControls;
        [SerializeField]
        private MessageControls _messageControls;
        [SerializeField]
        private UserProfileControls _profileControls;
        [SerializeField]
        private TransactionControls _transactionControls;
        [SerializeField]
        private FunctionControls _functionControls;

        private IAuthModule _authModule;
        private SharedMessagesService _messagesService;
        private BaseSharedMessagesStorage _messageStorage;
        private TextSharedMessageProcessor _textProcessor;
        private RemoteObjectsProvider _remoteObjectsProvider;
        private MutableObjectFactory _mutableObjectFactory;
        private BatchUpdater _batchUpdater;
        private FirebasePvpQueries _queries;

        void Start()
        {
            _authModule = new FirebaseAuthModule();
            FB.Init();
            _authModule.Init();
            _messagesService = new SharedMessagesService();
            var firebaseStorage = new FirebaseSharedMessagesStorage();
            firebaseStorage.Init(_authModule);
            _messageStorage = firebaseStorage;
            _textProcessor = new TextSharedMessageProcessor();
            _messagesService.RegisterProcessor<TextSharedMessage>(_textProcessor);
            _messagesService.Init(_authModule, _messageStorage);
            _messageControls.Init(_messagesService, _authModule);

            _loginControls.Init(_authModule);

            _remoteObjectsProvider = new FirebaseRemoteDataProvider();
            _mutableObjectFactory = new MutableObjectFactory(_remoteObjectsProvider);
            _batchUpdater = new FirebaseBatchUpdater();
            _queries = new FirebasePvpQueries(_remoteObjectsProvider);
            _profileControls.Init(_mutableObjectFactory, _authModule, _batchUpdater);
            _functionControls.Init(_queries);
            _transactionControls.Init(_remoteObjectsProvider, _authModule);
        }
    }
}
