using GBG.Modules.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Quests
{
    public class QuestControls : MonoBehaviour
    {
        [SerializeField]
        private Button _generateNewButton;
        [SerializeField]
        private Button _initButton;
        [SerializeField]
        private Text _questDataText;

        private QuestService _questService;

        public void Awake()
        {
            _generateNewButton.onClick.AddListener(GenerateNewQuest);
        }

        public void Init(QuestService questService)
        {
            _questService = questService;
        }


        public void GenerateNewQuest()
        {
            _questService.GenerateNewQuest();
        }
    }
}
