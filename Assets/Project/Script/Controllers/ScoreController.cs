using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.Views;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private float _sequenceSizePowerToElevate = 2;
        [SerializeField] private float _baseTileScoreValue = 5;
        
        private ScoreModel _scoreModel;

        private void Awake()
        {
            _scoreModel = new ScoreModel(0);
            _scoreView.Setup(0);
            _scoreModel.OnScoreChanged += OnScoreChanged;
        }

        public void HandleTilesDestroyed(List<MatchModel> matches)
        {
            int scoreToIncrease = 0;
            foreach (MatchModel match in matches)
            {
                float sequenceSizeMultiplier = Mathf.Pow(match.MatchedTiles.Count, _sequenceSizePowerToElevate);
                scoreToIncrease += Mathf.CeilToInt(sequenceSizeMultiplier * _baseTileScoreValue);
            }
            
            _scoreModel.IncreaseScore(scoreToIncrease);
        }
        
        private void OnScoreChanged(int previousScore, int newScore)
        {
            _scoreView.UpdateScoreView(previousScore, newScore);
        }
    }
}
