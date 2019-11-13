using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public class QuestService : IDisposable
    {
        private IQuestDefsStorage _defStorage;
        private IQuestDataStorage _dataStorage;

        // TO DO заменить на Subject, добавить возможность подписки на модели
        public IReactiveCollection<QuestModel> Models { get; private set; }

        private IDisposable _mergedToken;

        public void Init(IQuestDataStorage dataStorage, IQuestDefsStorage defStorage)
        {
            _dataStorage = dataStorage;
            _defStorage = defStorage;
            Models = new ReactiveCollection<QuestModel>();
            Models.ObserveAdd().Subscribe(SubscribeOnModel);

            var ids = _dataStorage.GetAllQuestIds();
            foreach(var dataId in ids)
            {
                var data = _dataStorage.GetQuestData(dataId);
                var defId = data.Id;
                var processor = _defStorage.InstantiateProcessor(defId, dataId, _dataStorage);
                var model = new QuestModel(processor);
                Models.Add(model);
            }
        }

        private void SubscribeOnModel(CollectionAddEvent<QuestModel> @event)
        {
            @event.Value.Subscribe(OnModelChanged);
        }
        
        private void OnModelChanged(QuestModel qm)
        {
            if(qm.State.Value == Data.QuestState.ReadyToRemove)
            {
                qm.DeleteQuest();
                Models.Remove(qm);
            }
        }

        public QuestModel GenerateNewQuest()
        {
            var defId = GetRandomQuestId();
            if (string.IsNullOrEmpty(defId))
                throw new Exception("Unable to generate new random quest Id");
            var dataId = Guid.NewGuid().ToString();
            var processor = _defStorage.InstantiateProcessor(defId, dataId, _dataStorage);
            var model = new QuestModel(processor);
            Models.Add(model);
            return model;
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
