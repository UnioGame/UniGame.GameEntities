using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.RemoteDataAbstracts;
using System.Collections.Generic;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public interface IRemoteChangesStorage
    {
        void AddChange(RemoteDataChange change);
        bool IsRootLoaded();
    }
}
