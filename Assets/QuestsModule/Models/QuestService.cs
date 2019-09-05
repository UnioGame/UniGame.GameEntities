using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public class QuestService
    {
        private IQuestDefsStorage _defStorage;
        private IQuestDataStorage _dataStorage;

        // TO DO заменить на Subject, добавить возможность подписки на модели
        public IReactiveCollection<QuestModel> Models { get; private set; }

        public void Init(IQuestDataStorage dataStorage, IQuestDefsStorage defStorage)
        {
            _dataStorage = dataStorage;
            _defStorage = defStorage;
            // Прочитать все квесты из даты
            // создать модели под все квесты
        }

        public IQuestModel GenerateNewQuest()
        {
            var defId = GetRandomQuestId();
            if (string.IsNullOrEmpty(defId))
                throw new Exception("Unable to generate new random quest Id");
            var dataId = Guid.NewGuid().ToString();
            _defStorage.InstantiateProcessor(defId, dataId, _dataStorage);

            return null;
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
    }
}
