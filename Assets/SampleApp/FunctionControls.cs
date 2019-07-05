using GBG.Modules.RemoteData.Pvp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samples {
    public class FunctionControls : MonoBehaviour
    {
        [SerializeField]
        private InputField _userId;
        [SerializeField]
        private Button _addButton;
        [SerializeField]
        private Button _removeButton;
        [SerializeField]
        private Button _poolButton;

        private IPvpRegistrationApi _api;

        public void Init(IPvpRegistrationApi api)
        {
            _api = api;
        }

        private void Awake()
        {
            _addButton.onClick.AddListener(OnAddClick);
            _removeButton.onClick.AddListener(OnRemoveClick);
            _poolButton.onClick.AddListener(OnLoadMetaClick);
        }

        private void OnAddClick()
        {
            _api.RegisterUserForPvp(_userId.text);
        }

        private void OnRemoveClick()
        {
            _api.UnregisterUserForPvp(_userId.text);
        }

        private void OnLoadMetaClick()
        {
            var handler = _api.GetPoolHandler();
            handler.LoadData()
                .ContinueWith((task) =>
                {
                    Debug.LogError("DATA LOADED :: " + task.ToString());
                });
            var roomHandler = _api.GetRoomHandler(0);
            roomHandler.LoadData()
                .ContinueWith((task) =>
                {
                    Debug.LogError("ROOM LOADED :: " + task.ToString());
                });
        }
    }
}
