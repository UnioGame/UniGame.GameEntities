using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GBG.Modules.Quests
{
    public class QuestService
    {
        // TO DO заменить на Subject, добавить возможность подписки на модели
        public IReactiveCollection<QuestModel> Models { get; private set; }

        public void Init(IQuestDataStorage dataStorage, IQuestDefsStorage defStorage)
        {
            // Прочитать все квесты из даты
            // создать модели под все квесты
        }

        public IQuestModel GenerateNewQuest()
        {
            throw new NotImplementedException();
        }
    }
}
