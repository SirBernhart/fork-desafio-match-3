using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.Views;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private float _sequenceSizeMultiplier;
        
        private ScoreModel _scoreModel;

        private void Awake()
        {
            _scoreModel = new ScoreModel(0);
            _scoreView.Setup(0);
            _scoreModel.OnScoreChanged += OnScoreChanged;
        }

        public void HandleTilesDestroyed(int destroyedTiles)
        {
            _scoreModel.IncreaseScore(destroyedTiles * 5);
        }
        
        private void OnScoreChanged(int previousScore, int newScore)
        {
            _scoreView.UpdateScoreView(previousScore, newScore);
        }
    }
}
