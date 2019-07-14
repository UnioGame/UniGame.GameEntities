using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using System;
using UniRx;

namespace Samples
{
    public class MutableUserDataDeepChild : MutableChild<DeeperUserDataChild>
    {
        public IReactiveProperty<string> FieldX;
        public IReactiveProperty<string> FieldY;

        public MutableUserDataDeepChild(Func<DeeperUserDataChild> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {
            FieldX = CreateReactiveProperty<string>(
                () => Object.FieldX,
                (val) => UpdateChildData(nameof(Object.FieldX), val),
                nameof(Object.FieldX));
            FieldY = CreateReactiveProperty<string>(
                () => Object.FieldY,
                (val) => UpdateChildData(nameof(Object.FieldY), val),
                nameof(Object.FieldY));
        }
    }
}
