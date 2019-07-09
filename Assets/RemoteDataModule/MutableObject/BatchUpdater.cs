using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GBG.Modules.RemoteData.RemoteDataAbstracts;

namespace GBG.Modules.RemoteData.MutableRemoteObjects
{
    public abstract class BatchUpdater
    {
        public abstract Task PerformBatchUpdate(IEnumerable<RemoteDataChange> changes);
    }
}
