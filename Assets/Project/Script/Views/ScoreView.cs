using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        private TweenerCore<int, int, NoOptions> _currentTween;

        public void Setup(int startingScore)
        {
            _scoreText.text = startingScore.ToString();
        }
        
        public void UpdateScoreView(int previousScore, int newScore)
        {
            if (_currentTween != null)
            {
                _currentTween.Complete();
            }
            
            _currentTween = DOTween.To(() => previousScore, IncreaseScoreText, newScore, 0.2f);

            void IncreaseScoreText(int currentScoreValue)
            {
                _scoreText.text = currentScoreValue.ToString();
            }
        }
    }
}
