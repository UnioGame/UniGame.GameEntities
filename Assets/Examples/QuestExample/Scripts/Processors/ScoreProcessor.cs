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
        private string _questDataId;
        private QuestData _questData;

        private IDisposable _scoreToken;

        public ScoreProcessor(MutableUserProfile profile, SampleQuestDef questDef, IQuestDataStorage dataStorage, string questDataId)
        {
            _profile = profile;
            _questDef = questDef;
            _dataStorage = dataStorage;
            _questDataId = questDataId;

            _progress = new ReactiveProperty<float>();
            MaxProgress = _questDef.Condition.Count;
            InitQuestData(_questDataId);
            UpdateProgress(_profile.ReactiveScore.Value);
            SubscribeOnProfile();
        }

        private void SubscribeOnProfile()
        {
            _scoreToken = _profile.ReactiveScore.Subscribe(OnScoreChanged);
        }

        // TODO лимит времени

        private void OnScoreChanged(int newScore)
        {
            var delta = CalculateProgress(newScore);
            if (delta > _questDef.Condition.Count)
            {
                Debug.Log("QUEST :: " + _questDef.Id + "Success!"); // TO DO Process reward
                _questData.State = QuestState.ReadyToRemove;
                _dataStorage.UpdateQuestData(_questDataId, _questData);
                _scoreToken.Dispose();
            }
            OnStateChanged();
        }

        private void InitQuestData(string questDataId)
        {
            _questDataId = questDataId;
            _questData = _dataStorage.GetQuestData(questDataId);
            if (_questData == null)
            {
                _questData = new QuestData();
                _questData.Id = _questDef.Id;
                _questData.State = QuestState.InProgress;
                _questData.ProgressStorage[SCORE_ON_START_KEY] = _profile.ReactiveScore.Value.ToString();
                _dataStorage.UpdateQuestData(questDataId, _questData);
            }
            OnStateChanged();
        }

        private void UpdateProgress(float value)
        {
            _progress.Value = int.Parse(_questData.ProgressStorage[SCORE_ON_START_KEY]);
            OnStateChanged();
        }

        private float CalculateProgress(float newScore)
        {
            var onStart = int.Parse(_questData.ProgressStorage[SCORE_ON_START_KEY]);
            return newScore - onStart;
        }

        public IReadOnlyReactiveProperty<float> Progress => _progress;
        private IReactiveProperty<float> _progress;

        public float MaxProgress { get; private set; }

        public string QuestDefId => _questDef.Id;

        public string QuestDataId => _questDataId;

        public QuestState QuestState => _questData.State;
        
        public event Action StateChanged;

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }

        public void DeleteData()
        {
            _dataStorage.UpdateQuestData(_questDataId, null);
        }
    } 
}
