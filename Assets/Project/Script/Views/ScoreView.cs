using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        public void Setup(int startingScore)
        {
            _scoreText.text = startingScore.ToString();
        }
        
        public void UpdateScoreView(int previousScore, int newScore)
        {
            int currentAnimatedScore = previousScore;
            DOTween.To(() => currentAnimatedScore, IncreaseScoreAndUpdateText, newScore, 0.2f);

            void IncreaseScoreAndUpdateText(int currentScoreValue)
            {
                currentAnimatedScore += currentScoreValue;
                _scoreText.text = currentAnimatedScore.ToString();
            }
        }
    }
}
