using UnityEngine;
using System.Collections;
using System;

namespace GBG.Modules.RemoteData.RemoteDataAbstracts
{
    public class RemoteDataChange
    {
        public string FieldName;
        public string FullPath;
        public object FieldValue;
        public Action<RemoteDataChange> ApplyCallback;
    }
}
