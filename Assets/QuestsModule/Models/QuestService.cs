using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public sealed class QuestService : IDisposable
    {
        private IQuestDefsStorage _defStorage;
        private IQuestDataStorage _dataStorage;

        // TO DO заменить на Subject, добавить возможность подписки на модели
        public IReactiveCollection<QuestModel> Models { get; private set; }

        private IDisposable _mergedToken;

        private Transform _modelsParent;

        public void Init(IQuestDataStorage dataStorage, IQuestDefsStorage defStorage, Transform modelsParent)
        {
            _dataStorage = dataStorage;
            _defStorage = defStorage;
            _modelsParent = modelsParent;
            Models = new ReactiveCollection<QuestModel>();
            Models.ObserveAdd().Subscribe(SubscribeOnModel);

            var ids = _dataStorage.GetAllQuestIds();
            foreach(var dataId in ids)
            {
                var data = _dataStorage.GetQuestData(dataId);
                var defId = data.Id;
                CreateQuestModel(dataId, defId);
            }
        }

        private QuestModel CreateQuestModel(string dataId, string defId)
        {
            var fsm = _defStorage.InstantiateFSM(defId, dataId);
            var go = new GameObject($"QuestModel :: {dataId}");
            go.transform.SetParent(_modelsParent);
            var model = go.AddComponent<QuestModel>();
            model.Init(fsm, _dataStorage, _defStorage, defId, dataId);
            Models.Add(model);
            return model;
        }

        private void SubscribeOnModel(CollectionAddEvent<QuestModel> @event)
        {
            @event.Value.Subscribe(OnModelChanged);
        }
        
        private void OnModelChanged(QuestModel qm)
        {
            if(qm.State == Data.QuestState.ReadyToRemove)
            {
                qm.DeleteQuest();
                Models.Remove(qm);
            }
        }

        public QuestModel GenerateNewQuest(string defId = null)
        {
            defId = defId != null ? defId : GetRandomQuestId();
            if (string.IsNullOrEmpty(defId))
                throw new Exception("Unable to generate new random quest Id");
            var dataId = Guid.NewGuid().ToString();
            return CreateQuestModel(dataId, defId);
        }

        private string GetRandomQuestId()
        {
            var allIds = _defStorage.GetAllQuestIds();
            var totalWeight = 0f;
            // TO DO возможно к весовой функции добавятся доп проверки
            // например на активность аналогичного квеста
            for (int i = 0; i < allIds.Count; i++)
                totalWeight += _defStorage.QuestWeightFunction(allIds[i]);
            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            var accWeight = 0f;
            for (int i = 0; i < allIds.Count; i++)
            {
                accWeight += _defStorage.QuestWeightFunction(allIds[i]);
                if (accWeight >= randomValue)
                    return allIds[i];
            }
            return null;
        }

        public void Dispose()
        {
            _mergedToken.Dispose();
        }
    }
}
