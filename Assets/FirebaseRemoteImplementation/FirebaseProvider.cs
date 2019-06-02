using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using UnityEngine;
using GOIFirebase.RemoteDataImpl;
using System.Linq;

namespace GBG.Modules.RemoteData.FirebaseImplementation
{
    public class GOIFirebaseProvider
    {
        private readonly FirebaseAuth m_auth = FirebaseAuth.DefaultInstance;

        public bool isLoggedIn
        {
            get
            {
                Assert.IsNotNull(m_auth);
                return m_auth.CurrentUser != null;
            }
        }

        public bool isVerifiedLoggedIn
        {
            get { return isLoggedIn && !m_auth.CurrentUser.IsAnonymous; }
        }

        //public Future Init()
        //{
        //    return Future.Run(() =>
        //    {
        //        m_auth.StateChanged += OnAuthStateChanged;
        //        var completer = new Completer();
        //        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //        {
        //            var status = task.Result;
        //            if (status != DependencyStatus.Available)
        //            {
        //                var msg = string.Format("Failed to init Firebase:\n\n{0}\n\nTry later", status);
        //                completer.CompleteException(new ApplicationException(msg));
        //            }
        //            else
        //            {
        //                OnInitialized();
        //                completer.Complete();
        //            }
        //        });
        //        return completer.future;
        //    });
        //}

        private void OnAuthStateChanged(object sender, EventArgs e)
        {
            UnityEngine.Debug.Log("STATE " + m_auth.CurrentUser?.UserId ?? "none");
        }

        //public Future Auth(IAuthToken token)
        //{
        //    return Future.Run(() =>
        //    {
        //        if (token == null)
        //            throw new ArgumentNullException(nameof(token));

        //        var completer = new Completer();

        //        Action<Task<FirebaseUser>> continuation = task =>
        //        {
        //            if (task.IsCompleted)
        //            {
        //                UnityEngine.Debug.Log("fetched user UID: " + task.Result.UserId);
        //                var objectsProvider = new FirebaseRemoteDataProvider();
        //                RemoteDataProvider = new GOIRemoteDataProvider(objectsProvider);
        //                if (!(token is DummyAuthToken))
        //                {
        //                    var userHandler = RemoteDataProvider.GetUserProfile(task.Result.UserId);
        //                    userHandler.LoadData().ContinueWith((getProfileTask) =>
        //                    {
        //                        Debug.Log("get profile task completed :: ");
        //                        if (getProfileTask.IsCompleted)
        //                        {
        //                            UnityEngine.Debug.Log("Succesfully fetched user profile");
        //                            var account = Locator.Resolve<IAccount>();
        //                            if (getProfileTask.Result.Object == null || getProfileTask.Result.Object.IsEmty())
        //                            {
        //                                //account.SetRemoteUserData(getProfileTask.Result.remoteData);
        //                                UnityEngine.Debug.Log("Remote user profile is Empty");

        //                                userHandler.UpdateRemoteData(account.GetSerializedProfile(task.Result.UserId)).ContinueWith((rewriteTask) =>
        //                                {
        //                                    if (rewriteTask.IsCompleted)
        //                                    {
        //                                        completer.Complete();
        //                                    }
        //                                    else if (rewriteTask.IsCanceled)
        //                                    {
        //                                        completer.CompleteException(new OperationCanceledException());
        //                                    }
        //                                    else if (rewriteTask.IsFaulted)
        //                                    {
        //                                        completer.CompleteException(getProfileTask.Exception);
        //                                    }
        //                                });
        //                            }
        //                            else
        //                            {
        //                                UnityEngine.Debug.Log("Seting local values from fetched profile");
        //                                //account.SetValuesFromSerializedProfile(getProfileTask.Result.profile);
        //                                //account.SetRemoteUserData(getProfileTask.Result.remoteData);
        //                                completer.Complete();
        //                            }
        //                        }
        //                        else if (getProfileTask.IsCanceled)
        //                        {
        //                            Debug.LogError("get profile canceled :: ");
        //                            completer.CompleteException(new OperationCanceledException());
        //                        }
        //                        else if (getProfileTask.IsFaulted)
        //                        {
        //                            Debug.LogError("get profile faulted :: " + getProfileTask.Exception);
        //                            completer.CompleteException(getProfileTask.Exception);
        //                        }
        //                    });
        //                }
        //                else
        //                    completer.Complete();
        //            }
        //            else if (task.IsCanceled)
        //            {
        //                completer.CompleteException(new OperationCanceledException());
        //            }
        //            else if (task.IsFaulted)
        //            {
        //                completer.CompleteException(task.Exception);
        //            }
        //        };

        //        switch (token)
        //        {
        //            case FacebookAuthToken facebookToken:
        //                var credential = FacebookAuthProvider.GetCredential(facebookToken.token);
        //                Debug.Log("Facebook auth");
        //                m_auth.SignInWithCredentialAsync(credential).ContinueWith(continuation);
        //                break;

        //            case EmailAuthToken emailToken:
        //                m_auth.FetchProvidersForEmailAsync(emailToken.Email).ContinueWith((fetchTask) =>
        //                {
        //                    UnityEngine.Debug.Log("Fetched providers :: " + string.Join(" | ", fetchTask.Result));
        //                    foreach (var i in fetchTask.Result)
        //                    {
        //                        m_auth.SignInWithEmailAndPasswordAsync(emailToken.Email, emailToken.Password).ContinueWith(continuation);
        //                        return;
        //                    }
        //                    m_auth.CreateUserWithEmailAndPasswordAsync(emailToken.Email, emailToken.Password).ContinueWith(continuation);
        //                });

        //                UnityEngine.Debug.LogFormat("Email Auth: email={0}, pass={1}", emailToken.Email, emailToken.Password);
        //                break;

        //            case DummyAuthToken _ when isLoggedIn:
        //                UnityEngine.Debug.Log("Dummy auth then logged");
        //                var objectsProvider = new FirebaseRemoteDataProvider();
        //                RemoteDataProvider = new GOIRemoteDataProvider(objectsProvider);
        //                completer.Complete();
        //                break;

        //            case DummyAuthToken _:
        //                UnityEngine.Debug.Log("Dummy auth then not logged");
        //                m_auth.SignInAnonymouslyAsync().ContinueWith(continuation);
        //                break;

        //            default:
        //                completer.CompleteException(new Exception("Unexpected token type " + token.GetType()));
        //                break;
        //        }

        //        return completer.future;
        //    });
        //}

        private void OnInitialized()
        {

        }
    }
}