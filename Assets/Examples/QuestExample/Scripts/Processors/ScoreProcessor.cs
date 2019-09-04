using GBG.Modules.Quests;
using GBG.Modules.Quests.Data;
using System;
using UniRx;
using UnityEngine;

namespace Samples.Quests
{
    public class ScoreProcessor : IQuestProcessor
    {
        private const string GOLD_NAME = "Gold";
        private const string SCORE_ON_START_KEY = "ScoreOnStart";

        private MutableUserProfile _profile;
        private SampleQuestDef _questDef;
        private IQuestDataStorage _dataStorage;
        private QuestData _questData;

        public ScoreProcessor(MutableUserProfile profile, SampleQuestDef questDef, IQuestDataStorage dataStorage)
        {
            _progress = new ReactiveProperty<float>();
            MaxProgress = _questDef.Condition.Count;

            _profile = profile;
            _questDef = questDef;
            _dataStorage = dataStorage;

            InitQuestData();
            UpdateProgress(_profile.ReactiveScore.Value);
            SubscribeOnProfile();
        }

        private void SubscribeOnProfile()
        {
            _profile.ReactiveScore.Subscribe(OnScoreChanged);
        }

        // TODO лимит времени

        private void OnScoreChanged(int newScore)
        {
            var delta = CalculateProgress(newScore);
            if (delta > _questDef.Condition.Count)
                Debug.Log("QUEST :: " + _questDef.Id + "Success!"); // TO DO Process reward
        }

        private void InitQuestData()
        {
            _questData = _dataStorage.GetQuestData(_questDef.Id);
            if (_questData == null)
            {
                _questData = new QuestData();
                _questData.State = QuestState.InProgress;
                _questData.ProgressStorage[SCORE_ON_START_KEY] = _profile.ReactiveScore.Value.ToString();
                _dataStorage.UpdateQuestData(_questDef.Id, _questData);
            }
        }

        private void UpdateProgress(float value)
        {
            _progress.Value = int.Parse(_questData.ProgressStorage[SCORE_ON_START_KEY]);
        }

        private float CalculateProgress(float newScore)
        {
            var onStart = int.Parse(_questData.ProgressStorage[SCORE_ON_START_KEY]);
            return newScore - onStart;
        }

        public IReadOnlyReactiveProperty<float> Progress => _progress;
        private IReactiveProperty<float> _progress;

        public float MaxProgress { get; private set; }

        // TO DO Call on change
        public event Action QuestStateChanged;

        // TO DO Remove
        public void ProcessStateChange()
        {
            throw new NotImplementedException();
        }
    } 
}
