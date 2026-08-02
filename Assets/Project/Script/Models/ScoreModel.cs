using System;

namespace Gazeus.DesafioMatch3.Models
{
    public class ScoreModel
    {
        public int CurrentScore { get; private set; }
        public event Action<int, int> OnScoreChanged;

        public ScoreModel(int currentScore)
        {
            CurrentScore = currentScore;
        }
        
        public void IncreaseScore(int score)
        {
            int previousScore = CurrentScore;
            CurrentScore += score;
            OnScoreChanged?.Invoke(previousScore, CurrentScore);
        }
    }
}
