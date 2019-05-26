using Facebook.Unity;
using RemoteDataModule.FirebaseImplementation.SharedMessages;
using RemoteDataModule.SharedMessages;
using RemoteDataModule.SharedMessages.MessageData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private LoginControls _loginControls;
        [SerializeField]
        private MessageControls _messageControls;

        private FirebaseAuthModule _authModule;
        private SharedMessagesService _messagesService;
        private BaseSharedMessagesStorage _messageStorage;
        private TextSharedMessageProcessor _textProcessor;

        void Start()
        {
            _authModule = new FirebaseAuthModule();
            _authModule.Init();
            _messagesService = new SharedMessagesService();
            var firebaseStorage = new FirebaseSharedMessagesStorage();
            firebaseStorage.Init(_authModule);
            _messageStorage = firebaseStorage;
            _textProcessor = new TextSharedMessageProcessor();
            _messagesService.RegisterProcessor<TextSharedMessage>(_textProcessor);
            _messagesService.Init(_authModule, _messageStorage);
            _loginControls.Init(_authModule);
            _messageControls.Init(_messagesService, _authModule);
        }


        public void Auth(FirebaseAuthModule.AuthType authType)
        {
            var task = _authModule.Login(authType);
        }
    }
}
