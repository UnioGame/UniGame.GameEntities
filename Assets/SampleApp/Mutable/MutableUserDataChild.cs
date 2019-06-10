using UnityEngine;
using System.Collections;
using GBG.Modules.RemoteData.MutableRemoteObjects;
using System;
using UniRx;
using Newtonsoft.Json;

namespace Samples
{
    public class MutableUserDataChild : MutableChild<SomeUserDataChild>
    {
        public IReactiveProperty<string> FieldA { get; private set; }
        public IReactiveProperty<int> FieldB { get; private set; }

        public MutableUserDataDeepChild DeepChild { get; private set; }

        public MutableUserDataChild(Func<SomeUserDataChild> getter, string fullPath, IRemoteChangesStorage storage) : base(getter, fullPath, storage)
        {
            this.FieldA = CreateReactiveProperty<string>(
                () => Object.FieldA,
                (v) => UpdateChildData(nameof(Object.FieldA), v),
                nameof(Object.FieldA));
            this.FieldB = CreateReactiveProperty<int>(
                () => Object.FieldB,
                (v) => UpdateChildData(nameof(Object.FieldB), v),
                nameof(Object.FieldB));
            this.DeepChild = new MutableUserDataDeepChild(
                () => Object.DeepChild,
                this.FullPath + nameof(Object.DeepChild) + "/",
                this);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Object);
        }
    }
}
