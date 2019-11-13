using GBG.Modules.RemoteData.Authorization;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samples
{
    public class TransactionControls : MonoBehaviour
    {
        [SerializeField]
        private InputField _resourceNameField;
        [SerializeField]
        private InputField _resourceCountField;

        private RemoteObjectsProvider _objectProvider;
        private IAuthModule _auth;

        public void Init(RemoteObjectsProvider objectProvider, IAuthModule authModule)
        {
            _objectProvider = objectProvider;
            _auth = authModule;
        }

        public void OnAddButtonClick()
        {
            var resName = _resourceNameField.text;
            var resCount = int.Parse(_resourceCountField.text);
            var path = string.Format("SharedResources/{0}/{1}", _auth.CurrentUserId, resName);
            var objectHandler = _objectProvider.GetRemoteObject<int>(path);
            objectHandler.UpdateRemoteData(resCount).ContinueWith((_) => { Debug.Log("Resources added"); });
        }

        private bool SpendResource(object oldValueObj, out int newValue)
        {
            var oldValue = oldValueObj != null? int.Parse(oldValueObj.ToString()) : 0;
            Debug.Log("OLD : " + oldValue);
            if (oldValue > 0)
            {
                var res = oldValue;
                res--;
                newValue = res;
                Debug.Log("NEW 2 : " + newValue.ToString());
                return true;
            }
            else
            {
                newValue = oldValue;
                Debug.Log("NEW 1 : " + newValue.ToString());
                return false;
            }
        }

        public void OnSpendButtonClick()
        {
            var resName = _resourceNameField.text;
            var path = string.Format("SharedResources/{0}/{1}", _auth.CurrentUserId, resName);
            var objectHandler = _objectProvider.GetRemoteObject<int>(path);
            objectHandler.PerformTransaction<bool>(SpendResource).ContinueWith((t) =>
            {
                if (t.Result)
                    Debug.Log("Resource removed");
                else
                    Debug.Log("No resource to remove");
            });
        }
    }
}
