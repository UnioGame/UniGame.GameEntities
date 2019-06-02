using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GBG.Modules.RemoteData.RemoteDataAbstracts;

namespace GBG.Modules.RemoteData.Transaction
{
    public abstract class BatchUpdater
    {
        public abstract Task PerformBatchUpdate(List<RemoteDataChange> changes);
    }
}
